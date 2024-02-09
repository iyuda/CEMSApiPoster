using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json.Linq;
namespace WebApiPoster.Models
    {
    public class Buttons : BaseClass
        {
          public string pcr_id { get; set; }
          public string button_id { get; set; }
          public string Clicked { get; set; }
          public string _ButtonID { get; set; }
          public string _multi_id { get; set; }
          public string _name { get; set; }
          public string _type_name { get; set; }
          public string _section_name { get; set; }
          public string _id { get; set; }
          public string _img { get; set; }
          public string _str { get; set; }
          public string _caption { get; set; }
          public string _button_type { get; set; }
         
//        public All_Buttons button_object { get; set; }

        public Buttons() { TableName = "pcr_buttons"; }
        public Buttons(string id, string SearchField = "id")
            : base(id, "Buttons", SearchField)
            {
          //  if (!string.IsNullOrEmpty(button_id)) button_object = new All_Buttons(button_id);
            }
        public Buttons(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }

            }



        public bool HandleRecord(string[] WhereColumns = null, string[] WhereValues = null)
            {
            //if (ValidateFields())
            //    this.InsertUpdateAction(InsertUpdate);
            //else
            //    Logger.LogError("Buttons Table Insert/Update Violation for id # " + this.id);
            ValidateFields();
            return this.InsertUpdateAction(0, WhereColumns, WhereValues);
            
            }


        public void ValidateFields()
            {
            Utilities.ValidateField("pcr", pcr_id);
          //  Utilities.ValidateField("All_Buttons", button_id);
            //Boolean rv = true;
            //if (!string.IsNullOrEmpty(pcr_id))
            //    {
            //    rv = Utilities.Exists("PCR", pcr_id);
            //    }
            //else if (!string.IsNullOrEmpty(button_id))
            //    {
            //    rv = Utilities.Exists("All_Buttons", button_id);
            //    }
            //return rv;
            }


        static Buttons()
        {
             InitVitals_Ariel();
        }
        private static Dictionary<string, string> VitalsList = new Dictionary<string, string>();
        private static Dictionary<string, string> VitalsSetConversionList = new Dictionary<string, string>();
      
        public static void InitVitals_Ariel()
        {
             VitalsList.Clear();
             VitalsList.Add("BP.UTO", "UTO"); //new List<object> { false, "4_1", "5_1" });
             VitalsList.Add("BP.PALP", "PALP");
             VitalsList.Add("SP02.ORA", "25_2");
             VitalsList.Add("SP02.W02", "25_3");
             VitalsList.Add("Pulse.UTO", "2_1"); //new List<object> { false, "2_1" });
             VitalsList.Add("Pulse.Regular", "3_1");
             VitalsList.Add("Pulse.Irregular", "3_2");
             VitalsList.Add("Status.Critical", "PtCond1");
             VitalsList.Add("Status.Unstable", "PtCond2");
             VitalsList.Add("Status.Partially Unstable", "PtCond3");
             VitalsList.Add("Status.Potentially Unstable", "PtCond3");
             VitalsList.Add("Status.PotentiallyUnstable", "PtCond3");
             VitalsList.Add("Status.PartiallyUnstable", "PtCond3");
             VitalsList.Add("Status.Stable", "PtCond4");

             VitalsList.Add("LeftPupil.Normal", "7_1");
             VitalsList.Add("LeftPupil.Dilated", "7_2");
             VitalsList.Add("LeftPupil.Constrict", "7_3");
             VitalsList.Add("LeftPupil.Sluggish", "7_4");
             VitalsList.Add("LeftPupil.NoReaction", "7_5");
             VitalsList.Add("RightPupil.Normal", "8_1");
             VitalsList.Add("RightPupil.Dilated", "8_2");
             VitalsList.Add("RightPupil.Constrict", "8_3");
             VitalsList.Add("RightPupil.Sluggish", "8_4");
             VitalsList.Add("RightPupil.NoReaction", "8_5");

             VitalsList.Add("TempScale.Ferenheit", "TempF");
             VitalsList.Add("TempScale.Centegrade", "TempC");

             VitalsList.Add("Respiration.Regular", "14_1");
             VitalsList.Add("Respiration.Shallow", "14_2");
             VitalsList.Add("Respiration.Labored", "14_3");

             VitalsList.Add("MentalStatus.Alert", "01_1");
             VitalsList.Add("MentalStatus.Voice", "01_2");
             VitalsList.Add("MentalStatus.Pain", "01_3");
             VitalsList.Add("MentalStatus.Unresponsive", "01_4");

             VitalsList.Add("Color.Normal", "23_1");
             VitalsList.Add("Color.Pale", "23_2");
             VitalsList.Add("Color.Cyanotic", "23_3");
             VitalsList.Add("Color.Ashen", "23_4");
             VitalsList.Add("Color.Flushed", "23_5");
             VitalsList.Add("Color.Jaundiced", "23_6");

             VitalsList.Add("LeftLungIns.Clear", "21_1");
             VitalsList.Add("LeftLungIns.Wheezing", "21_2");
             VitalsList.Add("LeftLungIns.Rales", "21_3");
             VitalsList.Add("LeftLungIns.Rhonchi", "21_4");
             VitalsList.Add("LeftLungIns.Diminished", "21_5");
             VitalsList.Add("LeftLungIns.Absent", "21_6");

             VitalsList.Add("LeftLungExp.Clear", "22_1");
             VitalsList.Add("LeftLungExp.Wheezing", "22_2");
             VitalsList.Add("LeftLungExp.Rales", "22_3");
             VitalsList.Add("LeftLungExp.Rhonchi", "22_4");
             VitalsList.Add("LeftLungExp.Diminished", "22_5");
             VitalsList.Add("LeftLungExp.Absent", "22_6");

             VitalsList.Add("RightLungIns.Clear", "19_1");
             VitalsList.Add("RightLungIns.Wheezing", "19_2");
             VitalsList.Add("RightLungIns.Rales", "19_3");
             VitalsList.Add("RightLungIns.Rhonchi", "19_4");
             VitalsList.Add("RightLungIns.Diminished", "19_5");
             VitalsList.Add("RightLungIns.Absent", "19_6");

             VitalsList.Add("RightLungExp.Clear", "20_1");
             VitalsList.Add("RightLungExp.Wheezing", "20_2");
             VitalsList.Add("RightLungExp.Rales", "20_3");
             VitalsList.Add("RightLungExp.Rhonchi", "20_4");
             VitalsList.Add("RightLungExp.Diminished", "20_5");
             VitalsList.Add("RightLungExp.Absent", "20_6");

             VitalsList.Add("Temperature.Normal", "24_1");
             VitalsList.Add("Temperature.Warm", "24_2");
             VitalsList.Add("Temperature.Cool", "24_3");
             VitalsList.Add("Temperature.Cold", "24_4");
             VitalsList.Add("Temperature.Hot", "24_5");

             VitalsList.Add("MotorResponse.Obeys Commands", "08_1");
             VitalsList.Add("MotorResponse.Obeys Command", "08_1");
             VitalsList.Add("MotorResponse.Localizes pain", "08_2");
             VitalsList.Add("MotorResponse.Withdraws To Pain", "08_3");
             VitalsList.Add("MotorResponse.Abnormal Flexion", "08_4");
             VitalsList.Add("MotorResponse.Abnormal Extension", "08_5");
             VitalsList.Add("MotorResponse.None", "08_6");

             VitalsList.Add("VerbalResponse.Oriented", "06_1");
             VitalsList.Add("VerbalResponse.Confused", "06_2");
             VitalsList.Add("VerbalResponse.Inappr. Words", "06_3");
             VitalsList.Add("VerbalResponse.Inappr. Sounds", "06_4");
             VitalsList.Add("VerbalResponse.None", "06_5");

             VitalsList.Add("Condition.Normal", "26_1");
             VitalsList.Add("Condition.Moist", "26_2");
             VitalsList.Add("Condition.Diaphoretic", "26_3");
             VitalsList.Add("Condition.Dry", "26_4");

             VitalsList.Add("EyeOpening.Spontaneous", "05_1");
             VitalsList.Add("EyeOpening.To Speech", "05_2");
             VitalsList.Add("EyeOpening.To Pain", "05_3");
             VitalsList.Add("EyeOpening.None", "05_4");

             VitalsList.Add("TakenBy.EMT", "10_1");
             VitalsList.Add("TakenBy.Driver", "10_2");

             //VitalsList.Add("CarbonMonoxide", "27_1");
             //VitalsList.Add("CarbonDioxide", "28_1");

             // used to convert from vital 1 to other vitals
             VitalsSetConversionList.Clear();
             VitalsSetConversionList.Add("7", "8");
             VitalsSetConversionList.Add("8", "9");
             VitalsSetConversionList.Add("23", "11");
             VitalsSetConversionList.Add("25", "27");
             VitalsSetConversionList.Add("21", "17");
             VitalsSetConversionList.Add("22", "18");
             VitalsSetConversionList.Add("19", "15");
             VitalsSetConversionList.Add("20", "16");
             VitalsSetConversionList.Add("24", "12");
             VitalsSetConversionList.Add("08", "22");
             VitalsSetConversionList.Add("06", "20");
             VitalsSetConversionList.Add("26", "13");
             VitalsSetConversionList.Add("05", "19");
             VitalsSetConversionList.Add("10", "24");
             VitalsSetConversionList.Add("01", "10");
        }
       
        private void DeletePrevSelections(string pcr_id, string ButtonIDPattern)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "delete p from pcr_Buttons p inner join all_buttons a on p.button_id=a.id where pcr_id = '" + pcr_id + "' and ButtonID regexp '" + ButtonIDPattern + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  cmd.ExecuteNonQuery();
             }
        }
        public bool ModifyArrayItem(string Path, string SearchField, string SearchValue, String ModifyField, string ModifyValue,  bool MustExist = false)
        {
             try
             {
                  object JsonData = HttpContext.Current.Session["json_out"];
                  string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
                  JToken JsonToken = JObject.Parse(@JsonString);
                  JArray jarray = (JArray)JsonToken.SelectToken(Path);

                  JToken SearchToken = null;
                  if (jarray == null && MustExist) return false;
                  if (jarray == null)
                  {
                       //JsonMaker.UpdateJsonValue(Path.Replace("buttons", "shmuttons"), "[]");
                       JsonMaker.UpdateJsonValue(Path, "[]");
                       JsonData = HttpContext.Current.Session["json_out"]; ;
                       JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'");
                       JsonToken = JObject.Parse(@JsonString);
                       //jarray = (JArray)JsonToken.SelectToken(Path.Replace("buttons", "shmuttons"));
                       jarray = (JArray)JsonToken.SelectToken(Path);
                  }
                  //return false;
                  else
                  {
                       SearchToken = jarray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString() == SearchValue);
                  }
                  if (SearchToken == null && MustExist) return false;

                  if (SearchToken == null)
                  {

                       JObject ArrayItem = new JObject();
                       ArrayItem[SearchField] = SearchValue;
                       ArrayItem[ModifyField] = ModifyValue;
                       if (this._id != null) ArrayItem["id"] = this._id;
                       if (this._img != null) ArrayItem["img"] = this._img;
                       if (this._str != null) ArrayItem["str"] = this._str;
                       if (this._caption != null) ArrayItem["caption"] = this._caption;

                      

                       //
                       jarray.Add(ArrayItem);

                       //    return false;
                  }
                  else
                       SearchToken.SelectToken(ModifyField).Replace(ModifyValue);
                  HttpContext.Current.Session["json_out"] = (JObject)JsonToken;
                  StringBuilder message = new StringBuilder();
                  message.Append("Button.ModifyArrayItem: " + System.Environment.NewLine);
                  message.Append("SearchField: " + SearchField + System.Environment.NewLine);
                  message.Append("SearchValue: " + SearchValue + System.Environment.NewLine);
                  message.Append("ModifyField: " + ModifyField + System.Environment.NewLine);
                  message.Append("ModifyValue: " + ModifyValue + System.Environment.NewLine);
                  Logger.LogJsonUpdates(message.ToString());
                  return true;
             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public bool LoadButton(string Path, string SubPath, string SearchField="id", string SearchValue="")
        {
             //if (button._type_name.ToLower().Contains("disposition"))
             //     result = button.LoadButton("DispositionWide", "moved", "str", button._name);
             //else if (button._type_name.ToLower().Contains("dispatch info"))
             //{
             //     string strToSplit = button._name.Replace("(", "~").Replace("(", "~");
             //     if (strToSplit.Split('~').Length > 2)
             //          result = button.LoadButton("Dispatch", "CALLTYPE." + strToSplit[1], "caption", button._name);
             //     else
             //          result = button.LoadButton("Dispatch", "CALLTYPE." + button._name.Replace(" ", ""), "caption", button._name);
             //}
             //else if (button._ButtonID.StartsWith("dispatchLightsAndSirensUsed"))
             //     result = button.LoadButton("Dispatch", "LightsSirens", "id", button._name == "10-82" ? "LSToDestination" : "LSToScene");
             //else if (button._ButtonID.ToLower().StartsWith("rma"))
             //     result = button.LoadButton("RMA", "RMAReason", "id", button._ButtonID.Replace("Type", ""));
             //else if (button._ButtonID.ToLower().StartsWith("Apcf"))
             //     result = button.LoadButton("Necessity", "SignerCred", "id", button._ButtonID.Replace("Apcf", ""));
             //else if (button._ButtonID.ToLower().StartsWith("Authorization"))
             //{
             //     result = button.LoadButton("HIPAA", "SignerCred", "str", button._name);
             //     if (!result) result = button.LoadButton("HIPAA", "SecondaryDocs", "str", button._name);
             //}
             _id = null;
             _img = null;
             _str = null;
             _caption = null;
             switch (Path)
             {
                  case "Disposition":
                  case "DispositionWide":
                       _id = _name;
                       _str = _name;
                       _img = "check-circle-o";
                       break;
                  case "Dispatch":
                       if (SubPath.Contains("CALLTYPE"))
                       {
                            _id = SubPath;
                            _caption = _name;
                            _str = "";
                       }
                       else if (SubPath.Contains("LightsSirens"))
                       {
                            _id = SearchValue;
                            _img = "check-square-o";
                            _str = SearchValue == "LSToScene" ? "To Scene" : "To Destination";
                       }
                       break;
                  case "RMA":
                       _id=_ButtonID.Replace("Type", "");
                       _img = "check-circle-o";
                       _str = _name.Replace("RMA ", "");
                       break;
                  case "Necessity":
                       _id=_ButtonID.Replace("Apcf", "");
                       _img = "check-circle-o";
                       _str = SearchValue;
                       break;
                  case "HIPAA":
                       switch (SearchValue)
                       {
                            case "Patient's Legal Guardian":
                                 _id = "REP1"; break;
                            case "Relative or other person who receives social security or other governmental benefits on behalf of the patient":
                                 _id = "REP2"; break;
                            case "Relative or other person who arranges for the patient's treatment or exercises other responsibility for the patient's affair":
                                 _id = "REP3"; break;
                            case "Representative of an agency or institution that did not furnish the services for which payment is claimed (i.e. ambulance services) but furnished other care, services, or assistance to the patient":
                                 _id = "REP4"; break;
                            case "Patient Care Report (signed by representative of facility)":
                                 _id = "SECDOC1"; break;
                            case "Facility Face Sheet/Admissions Record":
                                 _id = "SECDOC2"; break;
                            case "Patient Medical Record":
                                 _id = "SECDOC3"; break;
                            case "Hospital Log or Other Similar Facility Record":
                                 _id = "SECDOC4"; break;
                       }
                       _str = SearchValue;
                       _img = "check-circle-o";
                       break;


             }
             if (SearchValue == "") SearchValue = this.button_id;
             return this.ModifyArrayItem("$.['" + Path + "'].['" + SubPath + "'].['model'].['buttons']", SearchField, SearchValue, "choice", "on");
        }
        public bool LoadGenButton(string Path)
        {
             this._id = this.id;
             _img = null;
             _str = null;
             _caption = null;
             if (this._multi_id.Length > 2)
             {
                  this._img = "check-circle-o";
                  this._str = this._name;
             }
             else
             {
                  this._str = "";
                  this._caption = this._name;
             }

             return this.ModifyArrayItem("$.['" + Path + "'].['" + (_multi_id.Length > 2 ?_multi_id:button_id) + "'].['model'].['buttons']", "id", this.button_id, "choice", "on");
        }
        public bool LoadVital()
        {
             string ButtonIDSuffix="";
             int SetPos=this._ButtonID.LastIndexOf("Set")+3;
             if (SetPos > 3)
                  ButtonIDSuffix = _ButtonID.Substring(SetPos, _ButtonID.Length - SetPos);
          
             string VitalNo = _ButtonID.Replace("Vitals", "").Substring(0, 1);
             if (VitalNo != "1")
             {
                  string SectionID = ButtonIDSuffix.Contains("_") ? ButtonIDSuffix.Split('_')[0] : "";
                  string NewSectionID = VitalsSetConversionList.FirstOrDefault(x => x.Value == SectionID).Key;
                  if (SectionID != "" && NewSectionID != null) ButtonIDSuffix = ButtonIDSuffix.Replace(SectionID, NewSectionID);
             }
             string VitalID = VitalsList.FirstOrDefault(x => x.Value == ButtonIDSuffix).Key;
             if (VitalID == null) return false;
             this._str = VitalID.Split('.')[1]; 
             this._id = _str.Replace(" ", "");
             this._img = "check-circle-o";
             switch (VitalID.Split('.')[0])
             {
                  case "Color":
                  case "LeftLungIns":
                  case "LeftLungExp":
                  case "RightLungIns":
                  case "RightLungExp":
                  case "Temperature":
                  case "MotorResponse":
                  case "VerbalResponse":
                  case "Condition":
                  case "EyeOpening":
                  case "TakenBy":
                       return JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".selected", VitalID.Split('.')[1]);
                       break;
                  default:
                       return this.ModifyArrayItem("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.buttons", "str", VitalID.Split('.')[1], "choice", "on");
                       break;
             }
             //if (!this.ModifyArrayItem("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.buttons", "str", VitalID.Split('.')[1], "choice", "on"))
             //     if (!JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".selected", VitalID.Split('.')[1], MustExist: true))
             //          Logger.LogError("Could not load from button " + this._ButtonID + " of pcr " + this.pcr_id);
             //JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.butttons.img", "check-circle-o");
             //JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.butttons.str", VitalID.Split(',')[1]);
             //JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.butttons.choice", "on");
             
        }
       
        public bool SaveButton(object JsonData, string pcr_id = "", string SectionName="", string DynamicType="")
        {
             try
             {

                  this.pcr_id = pcr_id;

                  string str = JsonMaker.GetIOSJsonExtract("$.str", JsonData);
                  string id = JsonMaker.GetIOSJsonExtract("$.id", JsonData);
                  switch (id)
                  {
                       case "LSToScene": str = "10-63"; break;
                       case "LSToDestination": str = "10-82"; break;
                  }
                  if (str=="")
                       str = JsonMaker.GetIOSJsonExtract("$.caption", JsonData)+"";
                  string choice = JsonMaker.GetIOSJsonExtract("$.choice", JsonData);

                  if (str != null && String.IsNullOrEmpty(this.button_id))
                  {
                       JToken JSonToken = (JToken)JsonData;
                       if (SectionName == "$.RMA")
                       {
                            this.button_id = Buttons.GetIDByButtonID(id.Replace("RMA", "RMAType"));
                       }
                       else if (DynamicType == "")
                            this.button_id = Buttons.GetIDByName(str);
                       else
                            this.button_id = Buttons.GetIDByNameAndDynamicType(str, DynamicType);
                  }

                  this.Clicked = "1";
                  bool status = false;
                  if (!string.IsNullOrEmpty(this.button_id))
                  {
                       //this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });
                      
                       if (choice + "" == "on")
                            status = this.HandleRecord(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });
                       else
                       {
                            status = true;
                            this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });
                       }
                  }
                  if (!status && choice + "" == "on")
                       Logger.LogError("Could not find the following button for pcr id " + pcr_id +
                                        System.Environment.NewLine+
                                        "id: " + this.button_id + 
                                        System.Environment.NewLine+
                                        "name: " + str + 
                                        System.Environment.NewLine);
                                        
                  return status;
             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public void SaveVitalByButton(object JsonData, string VitalNo, string Path, string pcr_id = "")
        {
             try
             {

                  this.pcr_id = pcr_id;
                  //JToken JsonToken = (JToken)JsonData;
                  //if (JsonToken.Path.Contains("Vitals"))
                  //{
                  //     InitVitals();
                  //}
                  string VitalName = Path.Contains(".") ? Path.Split('.')[1] : "";

                  string id = JsonMaker.GetIOSJsonExtract("$.id", JsonData);
                  string VitalID = VitalName + "." + id;
                  string ButtonIDSuffix = Buttons.VitalsList.ContainsKey(VitalID) ? Buttons.VitalsList[VitalID] : "";
                  if (VitalNo != "1")
                  {
                       string SectionID = ButtonIDSuffix.Contains("_") ? ButtonIDSuffix.Split('_')[0] : "";
                       string NewSectionID = Buttons.VitalsSetConversionList.ContainsKey(SectionID) ? Buttons.VitalsSetConversionList[SectionID] : "";
                       if (SectionID != "" && NewSectionID != "") ButtonIDSuffix = ButtonIDSuffix.Replace(SectionID, NewSectionID);
                  }
                  string choice = JsonMaker.GetIOSJsonExtract("$.choice", JsonData);

                  //bool IsVital = JsonToken.Path.StartsWith("Vitals_");

                  if (id != null) this.button_id = Buttons.GetIDByName(id, VitalNo, ButtonIDSuffix.ToString());
                  this.Clicked = "1";
                  if (this.button_id != null)
                  {
                      // this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });
                       if (choice + "" == "on")
                            this.HandleRecord(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });
                       else
                            this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });
                  }
                  //if (this.button_id != null)
                  //     if (choice + "" == "on")
                  //          this.HandleRecord();
                  //     else
                  //          this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });

             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void SaveVitalBySelection(object JsonData, string VitalNo, string Category, string Range, string pcr_id = "")
        {
             this.pcr_id = pcr_id;
             string Value = JsonMaker.GetIOSJsonExtract("$.selected", JsonData);
             string VitalID = Category + "." + Value;
             string ButtonIDSuffix = Buttons.VitalsList.ContainsKey(VitalID) ? Buttons.VitalsList[VitalID] : "";
             if (VitalNo != "1")
             {
                  string SectionID = ButtonIDSuffix.Contains("_") ? ButtonIDSuffix.Split('_')[0] : "";
                  string NewSectionID = Buttons.VitalsSetConversionList.ContainsKey(SectionID) ? Buttons.VitalsSetConversionList[SectionID] : "";
                  if (SectionID != "" && NewSectionID != "") ButtonIDSuffix = ButtonIDSuffix.Replace(SectionID, NewSectionID);
             }

             if (Value != null) this.button_id = Buttons.GetIDByName(Value, VitalNo, ButtonIDSuffix.ToString());
             this.Clicked = "1";
             if (!String.IsNullOrEmpty(this.button_id))
             {
                  string DeletePattern = "Vitals" + VitalNo + "Set" + ButtonIDSuffix.ToString().Split('_')[0] + "_[" + Range + "]$";
                  DeletePrevSelections(pcr_id, DeletePattern);
                  // this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id.Split('_')[0]});
                  this.HandleRecord();
             }

        }
        public void SaveVitalByChoice(object JsonData, string VitalNo, string Category, string Range, string pcr_id = "")
        {
             //this.pcr_id = pcr_id;
             //string Value = JsonMaker.GetIOSJsonExtract("$.choice", JsonData);
             //string VitalID = Category + "." + Value;
             //string ButtonIDSuffix = Buttons.VitalsList.ContainsKey(VitalID) ? Buttons.VitalsList[VitalID] : "";
             //if (VitalNo != "1")
             //{
             //     string SectionID = ButtonIDSuffix.Contains("_") ? ButtonIDSuffix.Split('_')[0] : "";
             //     string NewSectionID = Buttons.VitalsSetConversionList.ContainsKey(SectionID) ? Buttons.VitalsSetConversionList[SectionID] : "";
             //     if (SectionID != "" && NewSectionID != "") ButtonIDSuffix = ButtonIDSuffix.Replace(SectionID, NewSectionID);
             //}

             //if (Value != null) this.button_id = Buttons.GetIDByName(Value, VitalNo, ButtonIDSuffix.ToString());
             //this.Clicked = "1";
             //if (!String.IsNullOrEmpty(this.button_id))
             //{
             //     string DeletePattern = "Vitals" + VitalNo + "Set" + ButtonIDSuffix.ToString().Split('_')[0] + "_[" + Range + "]$";
             //     DeletePrevSelections(pcr_id, DeletePattern);
             //     // this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id.Split('_')[0]});
             //     this.HandleRecord();
             //}

        }
        public static void ClearButtonArray(string Path, string PatternPath="")
        {
         try {
              object JsonData = HttpContext.Current.Session["json_out"]; ;
              string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
              JToken JsonToken = JObject.Parse(@JsonString);
             if (PatternPath != "")
             {
                  foreach (JToken token in JsonToken.Where(t => PatternPath != "*" ? t.Path.Contains(PatternPath) : 1 == 1))
                  {
                       JArray ButtonArray = (JArray)token.SelectToken("$.model.buttons");
                       if (ButtonArray != null)
                            foreach (JToken button in ButtonArray)
                            {
                                 button["choice"] = "off";
                            }

                  }
             }
             else
             {
                  JArray ButtonArray = (JArray)JsonToken.SelectToken("$.model.buttons");
                  if (ButtonArray != null)
                       foreach (JToken button in ButtonArray)
                       {
                            button["choice"] = "off";
                       }
             }
             }
             catch (Exception ex) { Logger.LogException(ex);  }
        }
        public static string GetNameByID(string id)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select name from all_buttons where id = '" + id + "'";
                  //if (VitalsID + "" != "")
                  //     SqlString += " and buttonID like 'Vitals" + VitalsID + "%Set" + ButtonIDSuffix + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        public static string GetIDByButtonID(string ButtonID)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from all_buttons where ButtonID = '" + ButtonID + "'";
                  //if (VitalsID + "" != "")
                  //     SqlString += " and buttonID like 'Vitals" + VitalsID + "%Set" + ButtonIDSuffix + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        public static string GetIDByName(string name, string VitalsID = "", string ButtonIDSuffix = "%")
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from all_buttons";
                  if (VitalsID + "" != "")
                       SqlString += " where buttonID like 'Vitals" + VitalsID + "%Set" + ButtonIDSuffix + "'";
                  else
                       SqlString += " where name = '" + name.Trim().Replace("'", @"\'") + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        public static string GetIDByNameAndDynamicType(string name, string DynamicType)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select a.id from all_buttons a inner join dynamic_button_types d on a.dynamic_button_id=d.id";
                  SqlString += " where name = '" + name.Trim().Replace("'", @"\'") + "' and type_name = '" + DynamicType + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        }

    }