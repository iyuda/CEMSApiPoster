using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
namespace WebApiPoster.PCR
{

    public class Pcr:PcrBase 
    {

        public Pcr(string id)
            : base(id, "pcr")
            {
            }
        public Pcr(string TableName, JsonInputSection PcrObj)
        {
            this.TableName = TableName;
            this.id = PcrObj["id"];
            this.pcr_dispatch_id = PcrObj["pcr_dispatch_id"]; ;
            this.pcr_demographic_id = PcrObj["pcr_demographic_id"];
            this.pcr_assessment_id = PcrObj["pcr_assessment_id"];
            this.pcr_narrative_notes_id = PcrObj["pcr_narrative_notes_id"];
            this.pcr_rma_id = PcrObj["pcr_rma_id"];
            this.pcr_authorization_id = PcrObj["pcr_authorization_id"];
            this.pcr_apcf_id = PcrObj["pcr_apcf_id"];
            this.pcr_disposition_id = PcrObj["pcr_disposition_id"];
            this.ems_run = PcrObj["ems_run"];
            this.pcr_narcotics_id = PcrObj["pcr_narcotics_id"];
            this.cad_number_id = PcrObj["cad_number_id"];
            this.pcr_type = PcrObj["pcr_type"];
            this.pcr_number = PcrObj["pcr_number"];
            this.printed = PcrObj["printed"];
            this.admitted = PcrObj["admitted"];
            this.agency_id = PcrObj["agency_id"];
            this.qa = PcrObj["qa"];

        }

          public Pcr()
        {
             this.TableName = "pcr";
           
        }
          public void MapIntoIOSJson(object JsonData, string pcr_id=null)
          {

         
               OutgoingJson = JsonData;
               this.id = pcr_id;
               if (!this.Exists()) 
                    return;
        
               this.Retrieve();
               object UserNameObj = JsonMaker.GetIOSJsonExtract("$.auth.user", JsonData);
               string UserName = UserNameObj + "";
               UserName = UserName == "jrubin" ? "4732" : UserName;
               string agency_id = Users.GetAgencyIDByName(UserName);
              
               Demographic demographic = new Demographic(this.pcr_demographic_id);
               demographic.MapIntoIOSJson();

               Dispatch dispatch = new Dispatch(this.pcr_dispatch_id);
               dispatch.MapIntoIOSJson();


 //  Map Dispatch Members
               JToken WorkJson = (JToken) Pcr.OutgoingJson;
               JArray MembersArray = (JArray)WorkJson.SelectToken("$.Dispatch.Members.model.buttons");
               if (MembersArray!=null) 
                    foreach (JToken token in MembersArray) 
                    {
                         token["choice"]="off";
                    }

               string SelectQuery = "(select m.*, user, last_name, first_name from pcr_members m inner join users u on m.user_id=u.id inner join person p on u.person_id=p.id  where pcr_id = '" + this.id + "')";
               List<Members> MembersList = Utilities.GetClassList<Members>(SelectQuery);
               int DriverNo = 1;
               foreach (Members member in MembersList)
               {
                    if (member.driver == "1")
                    {
                         member.MapIntoIOSJson("$.Dispatch.Members.model.buttons", DriverNo);
                         DriverNo++;
                    }
                    else
                         member.MapIntoIOSJson("$.Dispatch.Members.model.buttons");
               }

               JArray ButtonsArray = (JArray)WorkJson.SelectToken("$.Dispatch.Members.model.buttons");
               if (ButtonsArray != null)
                    foreach (JToken token in ButtonsArray)
                    {
                         token["choice"] = "off";
                    }


//  Map Buttons
               SelectQuery = "(select p.*, ButtonID, name, multi_id, type_name  from pcr_buttons p inner join all_buttons a on p.button_id=a.id left outer join dynamic_button_types d on a.dynamic_button_id=d.id where  pcr_id = '" + this.id + "')";
               List<Buttons> ButtonsList = Utilities.GetClassList<Buttons>(SelectQuery);
               foreach (Buttons button in ButtonsList)
               {
                    if ((button._type_name+"").StartsWith("800 "))
                    {
                         button.LoadButton("$." + button._type_name.Split(' ')[1].Trim()+"."+button._name);
                    }
                    else if  (button._ButtonID.StartsWith("Vitals") && button._ButtonID.Contains("Set"))
                         button.LoadVital();
                    else { 
                         bool result=false;
                         foreach (JToken token in ((IEnumerable<JToken>)OutgoingJson).Children().Where(t => t.Path.StartsWith("GEN:")))
                         {
                              result =button.LoadButton(token.Path);
                              if (result) break;
                         }
                         if (!result)
                              Logger.LogError("Could not load from button " + button._ButtonID + "; id: " + button.button_id + "; pcr: " + this.id);
                    }
               }

//  Map Inputs
               SelectQuery = "(select p.*, ButtonID from pcr_inputs p inner join all_buttons a on p.input_id=a.id where  pcr_id = '" + this.id + "')";
               List<Inputs> InputsList = Utilities.GetClassList<Inputs>(SelectQuery);
               foreach (Inputs input in InputsList)
               {
                    bool result = false;
                    foreach (JToken token in ((IEnumerable<JToken>)OutgoingJson).Children().Where(t => t.Path.StartsWith("GEN:")))
                    {
                         result = JsonMaker.UpdateJsonValue("$.['" + token.Path + "'].['" + input.input_id + "'].['choice']", input.value, MustExist:true);

                         if (result) break;
                    }
                    if (!result)
                         Logger.LogError("Could not load from input " + input._ButtonID + "; id: " + input.input_id + "; pcr: " + this.id);
               }

          }
          public void MapMemberButtonsIntoIOSJson(string JsonData, Dispatch dispatch)
          {
               
          }
          public void MapFromIOSJson(object JsonData, string pcr_id=null)
        {
             try
             {
                  object UserNameObj = JsonMaker.GetIOSJsonExtract("$.auth.user", JsonData);
                  string UserName = UserNameObj+"";
                  UserName = UserName == "jrubin" || UserName =="" ? "4732" : UserName;
                  string agency_id = Users.GetAgencyIDByName(UserName);

                  object WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Demographics", JsonData.ToString());
                  Demographic demographic=null;
                  if (WorkJson != null)
                  {
                       demographic = new Demographic();
                       demographic.MapFromIOSJson(WorkJson, agency_id);
                  }  
                  WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch", JsonData.ToString());
                  Dispatch dispatch=null;
                  if (WorkJson != null)
                  {
                       dispatch = new Dispatch();
                       dispatch.MapFromIOSJson(WorkJson);
                  }


                  if (UserName!="") 
                  {
                       //Utilities.GetClassList<Members>("pcr_members", pcr_id, "pcr_id");
                       this.id = pcr_id;
                       this.pcr_demographic_id = demographic.id;
                       this.pcr_dispatch_id = dispatch.id;
                       this.pcr_assessment_id = Guid.NewGuid().ToString();
                       this.pcr_narcotics_id = Guid.NewGuid().ToString();
                       this.pcr_narrative_notes_id = Guid.NewGuid().ToString();
                       this.pcr_rma_id = Guid.NewGuid().ToString();
                       this.pcr_authorization_id = Guid.NewGuid().ToString();
                       this.pcr_apcf_id = Guid.NewGuid().ToString();
                       this.pcr_disposition_id = Guid.NewGuid().ToString();

                       Ems_run ems_run = new Ems_run();
                       ems_run.HandleWithUserName(UserName, agency_id);
                       this._form_800_id=ems_run._form_800_id;
                       this.agency_id = agency_id;
                       this.ems_run = pcr_id == null ? "326c2888-defd-4e39-ab4c-174ee039c4fe" : ems_run.id;
                       
                       //this.pcr_narcotics_id =  Guid.NewGuid().ToString();

                       //this.pcr_type = "10-82";
                       //this.printed = "0";
                       //this.qa = "0";
                       //this.cad_number_id = PcrObj["cad_number_id"];
                       //this.pcr_type = PcrObj["pcr_type"];
                       //this.pcr_number = PcrObj["pcr_number"];
                       //this.printed = PcrObj["printed"];
                       //this.admitted = PcrObj["admitted"];
                       //this.agency_id = PcrObj["agency_id"];
                       //this.qa = PcrObj["qa"];
                    //pt_person_object = new Person(JsonData, "PatientInfo@PatientInfo", true);
                    //emr_contact_person_object = new Person(JsonData, "EmergencyContact@EmergencyContact");
                    //phy_contact_person_object = new Person(JsonData, "PhysicianInfo@PhysicianInfo");
                    //primary_policy_object = new Insurance_Policy(JsonData, "PrimaryInsurance@PrimaryInsurance");
                    //secondary_policy_object = new Insurance_Policy(JsonData, "SecondaryInsurance@SecondaryInsurance");
                    //terciary_insurance_object = new Insurance_Policy(JsonData, "TerciaryInsurance@TerciaryInsurance");

                    //this.pt_person = pt_person_object.id;
                    //this.emr_contact_person = emr_contact_person_object.id;
                    //this.phy_contact_person = phy_contact_person_object.id;
                    //this.primary_policy = primary_policy_object.id;
                    //this.secondary_policy = secondary_policy_object.id;
                    //this.terciary_insurance = secondary_policy_object.id;
                    //this.agency_id = "90b16e1c-aa76-11e5-b94a-842b2b4bbc99";
                    //string agency_id = PcrObj["agency_id"];
                    //this.agency_id = String.IsNullOrEmpty(agency_id) ? null : agency_id;
                    this.HandleRecord(new List<string> { "Dispatch", "Demographic", "Ems_run" });
               //this.InsertUpdateAction();
               }
               WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch", JsonData.ToString());
               JObject JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
               List<string> DriverList = new List<string>();
               foreach (JToken token in JsonObject.SelectToken(""))
               {
                    if (token.Path.StartsWith("Members@"))
                    {
                         DriverList.Add(JsonMaker.GetIOSJsonExtract("$." + token.Path, WorkJson));
                    }
               }
              // this.id = pcr_id==null?"00251ac0-58e7-4cad-8760-8911aa156d0b": pcr_id;;
               WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch.Members.model.buttons", JsonData.ToString());
               if (WorkJson != null)
               {
                    JArray MembersArray = (JArray)JsonConvert.DeserializeObject(WorkJson.ToString());
                    //   JObject JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
                    //List<object> MembersList = JsonMaker.GetListFromJSON<object>(WorkJson.ToString()); // jso.DeserializeObject<List<Members>(WorkJson.ToString());
                    //this.id = "00251ac0-58e7-4cad-8760-8911aa156d0b";
                    foreach (JToken token in MembersArray)
                    {
                         Members member = new Members();
                         member.pcr_id = pcr_id;
                         member.MapFromIOSJson((object)token, DriverList);
                    }
               }
               WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.NEMSIS", JsonData.ToString());
               if (WorkJson != null)
               {
                    JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
                    foreach (JToken token in ((IEnumerable<JToken>)WorkJson).Children())
                    {
                         var ButtonsArray = (IEnumerable<JToken>)JsonMaker.GetIOSJsonExtract("$.model.buttons", token.ToString());
                         if (ButtonsArray != null)
                              foreach (JToken button in ButtonsArray)
                              {
                                   Buttons pcr_button = new Buttons();
                                   pcr_button.SaveButton(button, pcr_id);
                              }
                    }

               }

               foreach (string LS_Type in "ALS,BLS,Vehicle".Split(',').Where ((LS_Type) => {WorkJson = (object)JsonMaker.GetIOSJsonExtract("$."+LS_Type, JsonData.ToString()); return WorkJson != null;}))
               {
                  //  WorkJson = (object)JsonMaker.GetIOSJsonExtract("$."+LS_Type, JsonData.ToString());
                    JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
                    //var q = from token in JsonObject.SelectToken("").Where(button => { Buttons pcr_button = new Buttons(); string button_id = pcr_button.GetLSButtonID("ALS", button.Path); return button_id != null; });)
                    //        where (button => { Buttons pcr_button = new Buttons(); string button_id = pcr_button.GetLSButtonID("ALS", button.Path); });
                    //        select token
                    foreach (JToken token in JsonObject.SelectToken(""))
                    {
                         string button_id;
                    //     WorkJson = (object)JsonMaker.GetIOSJsonExtract("$." + token.Path, "{" + token.ToString() + "}");
                      //   JObject JsonObject2 = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
                         foreach (JToken Field in token) // JsonObject2.SelectTokens(""))
                         {
                              
                              if (LS_Type != "Vehicle" || token.Path != "Vehicle")
                              {
                                  // var Property = Field as JProperty;
                                  // var p = token.SelectTokens("").Select(o => o.First);
                                   button_id = Utilities.GetLSButtonID(LS_Type, token.Path);
                                   //Property = token as JProperty;
                                   Form_800_Buttons form_800_button = new Form_800_Buttons();
                                   if (button_id != null)
                                   {
                                        form_800_button.form_800_id = this._form_800_id;
                                        form_800_button.all_buttons = button_id;
                                        if (token.First["choice"] + "" == "on")
                                        {
                                             form_800_button.HandleRecord();
                                        }
                                        else
                                             form_800_button.Delete(new string[] { "form_800_id", "all_buttons" }, new string[] { this._form_800_id, button_id });
                                   }
                              }
                              else
                              {
                                   button_id = Utilities.GetForm800ButtonID(LS_Type, token.Path);
                                   object obj=null;
                                   //var InnerToken = token.First.ToString();
                                  // var value = JsonConvert.DeserializeObject("$."+token.Path{" + token.ToString() + "}");
                                   object Signature;
                                   if (token.First["sig1_Encoding"] !=null) {
                                        Signature = (object)JsonMaker.GetIOSJsonExtract("$." + LS_Type + ".sig1_Encoding", WorkJson.ToString());
                                        var var1 = new
                                        {
                                             id = button_id,
                                             xml_ambulance = 0 //Utilities.BinaryToString( Signature.ToString())
                                        };
                                        obj = var1;
                                        break;
                                   }
                                   else if (token.First["sig2_Encoding"] !=null) {
                                        var var2 = new
                                        {
                                             id = button_id,
                                             xml_sig = 0
                                        };
                                        obj = var2;
                                        break;
                                   }
                                   else if (token.First["sig3_Encoding"] !=null) {
                                        var var3 = new
                                        {
                                             id = button_id,
                                             technical_signature = 0
                                        };
                                        obj = var3;
                                        break;
                                   }
                                 //  Utilities.Update("Form_800", obj);
                              }
                      }
                      
                              
                    }
                   
               }
               for (int i = 1; i <= 7; i++)
               {
                    WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Vitals_"+i.ToString(), JsonData.ToString());
                    if (WorkJson != null)
                    {
                     //    JArray VitalsArray = (JArray)JsonConvert.DeserializeObject(WorkJson.ToString());
                         //   JObject JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
                         //List<object> MembersList = JsonMaker.GetListFromJSON<object>(WorkJson.ToString()); // jso.DeserializeObject<List<Members>(WorkJson.ToString());
                         //this.id = "00251ac0-58e7-4cad-8760-8911aa156d0b";
                         foreach (JToken VitalSet in ((IEnumerable<JToken>)WorkJson).Children())
                         {
                              var ButtonsArray = (IEnumerable<JToken>)JsonMaker.GetIOSJsonExtract("$.model.buttons", VitalSet.ToString());
                              if (ButtonsArray!=null) 
                                   foreach (JToken button in ButtonsArray)
                                   {
                                        Buttons pcr_button = new Buttons();
                                        pcr_button.SaveVitalByButton(button, i.ToString(), VitalSet.Path, pcr_id);
                                   }
                              else
                              {
                                   string Range = "";
                                   string VitalName=VitalSet.Path.Split('.')[1];
                                   switch (VitalName)
                                   {
                                        case "Color":
                                             Range = "1-6";
                                             break;
                                        case "LeftLungIns":
                                             Range = "1-6";
                                             break;
                                        case "LeftLungExp":
                                             Range = "1-6";
                                             break;
                                        case "RightLungIns":
                                             Range = "1-6";
                                             break;
                                        case "RightLungExp":
                                             Range = "1-6";
                                             break;
                                        case "Temperature":
                                             Range = "1-5";
                                             break;
                                        case "MotorResponse":
                                             Range = "1-6";
                                             break;
                                        case "VerbalResponse":
                                             Range = "1-5";
                                             break;
                                        case "Condition":
                                             Range = "1-4";
                                             break;
                                        case "EyeOpening":
                                             Range = "1-4";
                                             break;
                                        case "TakenBy":
                                             Range = "1-2";
                                             break;
                                   }
                                   Buttons pcr_button = new Buttons();
                                   pcr_button.SaveVitalBySelection(VitalSet, i.ToString(), VitalName, Range, pcr_id);
                              }
                         }
                    }
               }
               var ParseToken = JToken.Parse(JsonData.ToString());
               foreach (JToken token in ((IEnumerable<JToken>)ParseToken).Children().Where(t => t.Path.StartsWith("GEN:")))
               {
                  //  WorkJson = (object)JsonMaker.GetIOSJsonExtract("$."+token.Path, token.ToString());
//                    JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
                    foreach (JToken Guid_Token in ((IEnumerable<JToken>)token).Children())
                    {
                         var ButtonsArray = (IEnumerable<JToken>)JsonMaker.GetIOSJsonExtract("$.model.buttons", Guid_Token.ToString());
                         if (ButtonsArray != null)
                              foreach (JToken button in ButtonsArray)
                              {
                                   Buttons pcr_button = new Buttons();
                                   pcr_button.button_id = JsonMaker.GetIOSJsonExtract("$.id", button.ToString()).ToString();
                                   pcr_button.SaveButton(button, pcr_id, pcr_button.button_id);
                              }
                         else
                         {
                              Inputs pcr_input = new Inputs();
                              pcr_input.pcr_id = pcr_id;
                              pcr_input.input_id = Guid_Token.Path.Split('.')[1];
                              pcr_input.value = JsonMaker.GetIOSJsonExtract("$.choice", Guid_Token)+"~";
                              pcr_input.Delete(new string[] { "pcr_id", "input_id" }, new string[] { pcr_id, pcr_input.input_id });
                              pcr_input.HandleRecord();
                         }

                    }
                    
               }
               //WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch", JsonData.ToString());
               //MembersArray = (JArray)JsonConvert.DeserializeObject(WorkJson.ToString());
               ////string distance = jObject.SelectToken("routes[0].legs[0].distance.text").ToString();
               //foreach (JToken token in MembersArray)
               //{
               //   //  if token.
                   
               //}

               //for (int i = 1; i <= 4; i++)
               //{
               //     Members member = new Members();
               //     member.pcr_id = this.id;
               //     member.MapFromIOSJson(JsonData, i);


               //}
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public static object OutgoingJson;

        public string pcr_dispatch_id { get; set; }
        public string pcr_demographic_id { get; set; }
        public string pcr_assessment_id { get; set; }
        public string pcr_narrative_notes_id { get; set; }
        public string pcr_rma_id { get; set; }
        public string pcr_authorization_id { get; set; }
        public string pcr_apcf_id { get; set; }
        public string pcr_disposition_id { get; set; }
        public string ems_run { get; set; }
        public string pcr_narcotics_id { get; set; }
        public string cad_number_id { get; set; }
        public string cad_number { get { return null; } set { ; } }
        public string pcr_type { get; set; }
        public string pcr_number { get; set; }
        public string printed { get; set; }
        public string admitted { get; set; }
        public string agency_id { get; set; }
        public string qa { get; set; }
        public string _form_800_id { get; set; }
        public void HandleRecord( List<string> ExcludeList=null)
            {
               ValidateFields(ExcludeList);
               this.InsertUpdateAction();
            //if (this.Exists())
            //    this.Update();
            //else
            //    this.Insert();
            }
        public void ValidateFields(List<string> ExcludeList=null)
            {
            List<string> List1= ExcludeList ?? new List<string>();
            if (!List1.Contains("Dispatch")) {
                 Dispatch dispatch = new Dispatch(pcr_dispatch_id);            
                 dispatch.ValidateField();
            }
            if (!List1.Contains("Demographic")) {
                 Demographic demographic = new Demographic(pcr_demographic_id);
                 demographic.ValidateField();
            }
            if (!List1.Contains("Narrative_Notes")) {
                 Narrative_Notes narrative_notes = new Narrative_Notes(pcr_narrative_notes_id);
                 narrative_notes.ValidateField();
            }   
            if (!List1.Contains("Rma")) {
                 Rma rma = new Rma(pcr_rma_id);
                 rma.ValidateField();
            }   
            if (!List1.Contains("Disposition")) {
                 Disposition disposition = new Disposition(pcr_disposition_id);
                 disposition.ValidateField();
            }   
            if (!List1.Contains("Narcotic")) {
                 Narcotic narcotic = new Narcotic(pcr_narcotics_id);
                 narcotic.ValidateField();
            }   
            if (!List1.Contains("Authorization")) {
                 Authorization authorization = new Authorization(pcr_authorization_id);
                 authorization.ValidateField();
            }
            if (!List1.Contains("Assessment"))  Utilities.ValidateField("pcr_assessment", pcr_assessment_id);
            if (!List1.Contains("Apcf"))  Utilities.ValidateField("pcr_Apcf", pcr_apcf_id);
            if (!List1.Contains("Ems_run"))  Utilities.ValidateField("ems_run", ems_run);
            if (!List1.Contains("Agency"))  Utilities.ValidateField("agency", agency_id);
            //Utilities.ValidateField(new Dispatch("pcr_dispatch", pcr_dispatch_id));
            //Utilities.ValidateField(new Demographic("pcr_demographic", pcr_demographic_id));
            //Utilities.ValidateField("pcr_assessment", pcr_assessment_id);
            //Utilities.ValidateField(new Narrative_Notes("pcr_narrative_notes", pcr_narrative_notes_id));
            //Utilities.ValidateField(new Rma("pcr_rma", pcr_rma_id));
            //Utilities.ValidateField("pcr_Apcf", pcr_apcf_id);
            //Utilities.ValidateField(new Disposition("pcr_disposition", pcr_disposition_id));
            //Utilities.ValidateField("ems_run", ems_run);
            //Utilities.ValidateField(new Narcotic("pcr_narcotic", pcr_narcotics_id));
            //Utilities.ValidateField(new Authorization("pcr_authorization", pcr_authorization_id));
            }
        
        //public void ValidateField(string TableName, string[] FieldNames, string[] Values)
        //    {
        //    if (!Utilities.Exists(TableName, id))
        //        Utilities.Insert(TableName, FieldNames, Values);
        //    }

        //public Boolean Exists()
        //    {
        //   
        //    using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
        //        {
        //        cn.Open();
        //        string SqlString = "select count(*) from pcr where id = '" + this.id + "'";
        //        MySqlCommand cmd = new MySqlCommand(SqlString, cn);
        //        int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //        return rc > 0;
        //        }
        //    }
        //public Boolean Insert()
        //    {
        //    try
        //        {
        //        string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //        using (MySqlConnection cn = new MySqlConnection(strConnect))
        //            {
        //            cn.Open();

        //            StringBuilder ColumnList = new StringBuilder();
        //            StringBuilder ValueList = new StringBuilder();
        //            for (int i = 0; i <= this.GetType().GetProperties().Length - 1; i++)
        //                {
        //                ColumnList.Append(this.GetType().GetProperties()[i].Name);

        //                var Value = this.GetType().GetProperties()[i].GetValue(this, null);
        //                Boolean boolValue;
        //                if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
        //                    Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

        //                DateTime dateValue;
        //                if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
        //                    Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

        //                ValueList.Append(Value == null ? "null" : "'" + Value + "'");
        //                if (i < this.GetType().GetProperties().Length - 1)
        //                    {
        //                    ColumnList.Append("," + System.Environment.NewLine);
        //                    ValueList.Append("," + System.Environment.NewLine);
        //                    }
        //                }
        //            string InsertString = "insert into pcr (" + ColumnList.ToString() + ") values (" + ValueList.ToString() + ")";
        //            MySqlCommand cmd = new MySqlCommand(InsertString, cn);
        //            int rows = cmd.ExecuteNonQuery();
        //            return rows > 0;
        //            }

        //        }
        //    catch (Exception ex) { Logger.LogException(ex); return false; }
        //    }

        //public Boolean Update(string KeyField = "id")
        //    {
        //    try
        //        {
        //        string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //        using (MySqlConnection cn = new MySqlConnection(strConnect))
        //            {
        //            cn.Open();
        //            string UpdateString = "update pcr set " + System.Environment.NewLine;
        //            StringBuilder Assignments = new StringBuilder();
        //            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
        //                {
        //                string Value = prop.GetValue(this, null) == null ? "null" : "'" + prop.GetValue(this, null) + "'";
        //                if (prop.Name != KeyField) Assignments.Append(prop.Name + " = " + Value + System.Environment.NewLine);
        //                }
        //            Assignments.Append("where id ='" + this.id + "'");
        //            UpdateString += Assignments.ToString();
        //            MySqlCommand cmd = new MySqlCommand(UpdateString, cn);
        //            int rows = cmd.ExecuteNonQuery();
        //            return rows > 0;
        //            }

        //        }
        //    catch (Exception ex) { Logger.LogException(ex); return false; }
        //    }
    }


    //public class Dispatch_In
    //{

    //    public string id { get; set; }
    //    public string facility_name { get; set; }
    //    public string name { get; set; }
    //    public string address { get; set; }
    //    public string apartment { get; set; }
    //    public string room { get; set; }
    //    public string zip { get; set; }
    //    public string city { get; set; }
    //    public string state { get; set; }
    //    public string responded_from { get; set; }
    //    public string call_type { get; set; }
    //    public string cad { get; set; }
    //    public string dispatch_method { get; set; }
    //    public string transported_from { get; set; }
    //    public string borough { get; set; }
    //    public string assigned { get; set; }
    //    public string en_route { get; set; }
    //    public string on_scene { get; set; }
    //    public string pt_contact { get; set; }
    //    public string from_scene { get; set; }
    //    public string at_destination { get; set; }
    //    public string in_service { get; set; }
    //    public string mileage_begin { get; set; }
    //    public string mileage_end { get; set; }
    //}
    //public class Dispatch_Out
    //{
    //    public string id { get; set; }
    //    public string date { get; set; }
    //    public string cad { get; set; }
    //    public string transported_from { get; set; }
    //    public string town_id { get; set; }
    //    public string cross_street { get; set; }
    //    public string assigned { get; set; }
    //    public string en_route_63 { get; set; }
    //    public string on_scene_84 { get; set; }
    //    public string pt_contact { get; set; }
    //    public string from_scene_82 { get; set; }
    //    public string at_destination { get; set; }
    //    public string in_service { get; set; }
    //    public string pt_count { get; set; }
    //    public string dispatch_method { get; set; }
    //    public string phone { get; set; }
    //    public string mileage_begin { get; set; }
    //    public string mileage_end { get; set; }
    //    public string address_id { get; set; }
    //    public string facility_id { get; set; }
    //    public string call_type { get; set; }
    //    public string update_id { get; set; }
    //    public string insert_id { get; set; }
    //    public string utc_update { get; set; }
    //    public string utc_insert { get; set; }
    //    public string CallReceivedTime { get; set; }
    //    public string neighborhood { get; set; }
    //}

}