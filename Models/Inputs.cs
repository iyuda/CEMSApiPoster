using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Inputs : BaseClass
        {
        public string value { get; set; }
        public string pcr_id { get; set; }
        public string input_id { get; set; }
        public string _ButtonID { get; set; }
        public string _section_name { get; set; }
        public string _type_name { get; set; }
        public string _json_token_name { get; set; }

        //        public All_Buttons button_object { get; set; }

        public Inputs() { TableName = "pcr_inputs"; }

        static Inputs()
        {
             InitVitals();
        }
        private static Dictionary<string, string> VitalsList = new Dictionary<string, string>();
        private static Dictionary<string, string> VitalsSetConversionList = new Dictionary<string, string>();
        private static Dictionary<string, string> EnteredValues = new Dictionary<string, string>();
        private static string BloodPressure = "";

        public static void ResetBloodPressure()
        {
             BloodPressure = "";
        }
        public static void ResetEnteredGuids()
        {
             EnteredValues.Clear();
        }
        public static int GetEnteredGuidsCount()
        {
             return EnteredValues.Count;
        }
        public static bool IsGuidEntered(string json_token_name)
        {
             return EnteredValues.ContainsKey(json_token_name);
        }
        public static string GetEnteredValue(string json_token_name)
        {
             return EnteredValues[json_token_name];
        }
        public static void InitVitals()
        {
             VitalsList.Clear();
             VitalsList.Add("BloodSugar", "Set6_1");
             VitalsList.Add("BloodPressure1", "Set4_1");
             VitalsList.Add("BloodPressure2", "Set5_1");
             VitalsList.Add("CarbonMonoxide", "Set27_1");
             VitalsList.Add("CarbonDioxide", "Set28_1");
             VitalsList.Add("PulseRate", "Set2_1");
             VitalsList.Add("RespirationRate", "RespRat19_1");
             VitalsList.Add("PulseOxymetry", "Set25_1");
             VitalsList.Add("Temperature", "SetTemp");
             VitalsList.Add("Time1", "Set1_1");
             VitalsList.Add("TakenBy", "Set10_0");

             VitalsSetConversionList.Clear();
             VitalsSetConversionList.Add("Set6_1", "SetGluc");
             VitalsSetConversionList.Add("Set10_1", "Set24_0");


        }
        public static string GetIDByName(string name)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from all_buttons where name = '" + name.Trim().Replace("'", @"\'") + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar() + "";
                  return rv;
             }
        }
        public static string GetIDByButtonID(string ButtonID)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from all_buttons where ButtonID = '" + ButtonID.Trim().Replace("'", @"\'") + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar() + "";
                  return rv;
             }
        }
        public static string GetIDByVitalID(string VitalsID = "", string ButtonIDSuffix = "%")
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from all_buttons";
                 // if (VitalsID + "" != "")
                  SqlString += " where buttonID like 'Vitals" + VitalsID + "%" + ButtonIDSuffix + "'";
                 
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar() + "";
                  return rv;
             }
        }
        public bool LoadVital()
        {
             string ButtonIDSuffix = _ButtonID.Replace("Vitals", "").Remove(0,1);

             string VitalNo = _ButtonID.Replace("Vitals", "").Substring(0, 1);
             if (VitalNo != "1")
             {
                  string NewButtonIDSuffix = VitalsSetConversionList.FirstOrDefault(x => x.Value == ButtonIDSuffix).Key;
                  if (NewButtonIDSuffix != null) ButtonIDSuffix = NewButtonIDSuffix;
             }

             string VitalID = VitalsList.FirstOrDefault(x => x.Value == ButtonIDSuffix).Key;
             
             if (VitalID == null) return false;

             if (VitalID.StartsWith("BloodPressure"))
             {
                  if (String.IsNullOrEmpty(BloodPressure))
                       BloodPressure = this.value;
                  else
                       BloodPressure += "/" + this.value;
                  return JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + ".BloodPressure.choice", BloodPressure);
             }
             else if (VitalID!="TakenBy")
                  return JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID + ".choice", this.value);
             else
                  return JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID + ".selected", this.value);

            
             

        }
        public void SaveVitalByChoice(string Value, string VitalNo, string Category, string pcr_id = "")
        {
             this.pcr_id = pcr_id;
             if (Category.StartsWith("BloodPressure"))
               this._json_token_name = "BloodPressure";
             else
               this._json_token_name = Category;

             if (Category=="TakenBy")
             {
                  if (Value == "EMT" && HttpContext.Current.Session["EMT"] != null)
                       Value = HttpContext.Current.Session["EMT"].ToString();
                  if (Value == "Driver" && HttpContext.Current.Session["Driver"] != null)
                       Value = HttpContext.Current.Session["Driver"].ToString();
                  if (Value.Contains(","))
                       Value = Users.GetBadgeByNameAndAgency(Value, HttpContext.Current.Session["agency_id"].ToString());
             }
             string VitalID = Category.Replace("_TEXT", "").Replace("_TIME", ""); ;
             
             string ButtonIDSuffix = Inputs.VitalsList.ContainsKey(VitalID) ? Inputs.VitalsList[VitalID] : "";
             if (VitalNo != "1")
             {
                  //string SectionID = ButtonIDSuffix.Contains("_") ? ButtonIDSuffix.Split('_')[0] : "";
                  ButtonIDSuffix = Inputs.VitalsSetConversionList.ContainsKey(ButtonIDSuffix) ? Inputs.VitalsSetConversionList[ButtonIDSuffix] : ButtonIDSuffix;
                 // if (SectionID != "" && NewSectionID != "") ButtonIDSuffix = ButtonIDSuffix.Replace(SectionID, NewSectionID);
             }
             this.value = Value + "~";
             if (Value != null) this.input_id = Inputs.GetIDByVitalID(VitalNo, ButtonIDSuffix.ToString());

             if (!String.IsNullOrEmpty(this.input_id))
             {
                  this.HandleRecord();
             }

        }
        public Inputs(string id, string SearchField = "id")
            : base(id, "pcr_inputs", SearchField)
            {
            //  if (!string.IsNullOrEmpty(button_id)) button_object = new All_Buttons(button_id);
            }
        public Inputs(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }
            }

        public void HandleRecord()
            {           
               ValidateFields();
               bool Proceed=true;
               
               if (!String.IsNullOrEmpty(_json_token_name))
               {
                    string check_key;
                    string prev_value;
                    //if ((_json_token_name + "").ToLower().EndsWith("_text"))
                    //{
                    //     check_key = _json_token_name.Replace("_text", "").Replace("_TEXT", "");
                    //     prev_value = Inputs.EnteredValues.ContainsKey(check_key) ? Inputs.EnteredValues[check_key] : "";
                    //}
                    //else
                    //{
                    //     check_key = _json_token_name + "_TEXT";
                    //     prev_value = Inputs.EnteredValues.ContainsKey(check_key) ? Inputs.EnteredValues[check_key] : "";
                    //     if (String.IsNullOrEmpty(prev_value))
                    //     {
                    //          check_key = _json_token_name + "_text";
                    //          prev_value = Inputs.EnteredValues.ContainsKey(check_key) ? Inputs.EnteredValues[check_key] : "";
                    //     }
                    //}

                    if ((_json_token_name + "").EndsWith("_TEXT"))
                         check_key = _json_token_name.Replace("_TEXT", "");
                    else
                         check_key = this.input_id;
                    prev_value = Inputs.EnteredValues.ContainsKey(check_key) ? Inputs.EnteredValues[check_key] : "";

                    if (String.IsNullOrEmpty(this.value) && !String.IsNullOrEmpty(prev_value))
                         Proceed = false;
                    if (!String.IsNullOrEmpty(this.value) && !String.IsNullOrEmpty(prev_value))
                         if ((_json_token_name + "").EndsWith("_TEXT"))
                              Proceed = false;
                         //this.value = prev_value.Replace("~","") + this.value;
               }
               if (Proceed) {
                   // this.Delete(new string[] { "pcr_id", "input_id" }, new string[] { this.pcr_id, this.input_id });
                    bool result = this.InsertUpdateAction(0, new string[] { "pcr_id", "input_id" }, new string[] { this.pcr_id, this.input_id });
                    if (result && !String.IsNullOrEmpty(_json_token_name) && !EnteredValues.ContainsKey(this.input_id)) EnteredValues.Add(this.input_id, this.value);
               }
            }

        public void ValidateFields()
            {
           // Utilities.ValidateField("pcr", pcr_id);
            //Utilities.ValidateField("All_Buttons", input_id );

            }

        public void MapNarrativeIntoIOSJson(string pcr_id, string InputName)
        {
             try
             {

                  this.Retrieve(new string[] { "pcr_id", "input_id" }, new string[] { pcr_id, Buttons.GetIDByButtonID("Notes" + InputName) });
                  JsonMaker.UpdateJsonValue("$.['Narrative'].['UnusualOccurance.UnusualOccurance_" + InputName + "'].['UnusualOccurance_" + InputName + "']", this.value);
 

             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapDispatchIntoIOSJson(string pcr_id, string InputName)
        {
             try
             {

                  this.Retrieve(new string[] { "pcr_id", "input_id" }, new string[] { pcr_id, Buttons.GetIDByButtonID(InputName) });
                  JsonMaker.UpdateJsonValue("$.['Dispatch'].['CallLocation.CallLocation_Room'].['CallLocation_Room']", this.value);


             }
             catch (Exception ex) { Logger.LogException(ex); }
        }  
        public void MapNarrativefromIOSJson(object JsonData, string pcr_id, string InputName)
        {

             string Value = JsonMaker.GetIOSJsonExtract("$.['UnusualOccurance.UnusualOccurance_"+ InputName +"'].['UnusualOccurance_"+ InputName+"']", JsonData);

             this.pcr_id = pcr_id;
             this.input_id = Buttons.GetIDByButtonID("Notes" + InputName);
             this.value = Value;
          //   this.Delete(new string[] { "pcr_id", "input_id" }, new string[] { pcr_id, this.input_id });
             this.HandleRecord();
        }
        public void MapDispatchfromIOSJson(object JsonData, string pcr_id, string InputName)
        {

             string Value = JsonMaker.GetIOSJsonExtract("$.['CallLocation.CallLocation_Room'].['CallLocation_Room']", JsonData);

             this.pcr_id = pcr_id;
             this.input_id = Buttons.GetIDByButtonID(InputName);
             this.value = Value;
             // this.Delete(new string[] { "pcr_id", "input_id" }, new string[] { pcr_id, this.input_id });
             this.HandleRecord();
        }
        }
    }
