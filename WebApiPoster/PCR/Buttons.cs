using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
namespace WebApiPoster.PCR
    {
    public class Buttons : PcrBase
        {
          public string pcr_id { get; set; }
          public string button_id { get; set; }
          public string Clicked { get; set; }
          public string _ButtonID { get; set; }
          public string _multi_id { get; set; }
          public string _name { get; set; }
          public string _type_name { get; set; }

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



        public void HandleRecord(int InsertUpdate = 0)
            {
            //if (ValidateFields())
            //    this.InsertUpdateAction(InsertUpdate);
            //else
            //    Logger.LogError("Buttons Table Insert/Update Violation for id # " + this.id);
            ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            
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
             InitVitals();
        }
        private static Dictionary<string, string> VitalsList = new Dictionary<string, string>();
        private static Dictionary<string, string> VitalsSetConversionList = new Dictionary<string, string>();
      
        public static void InitVitals()
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
             VitalsList.Add("Status.Potentially Unstable", "PtCond3");
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
             VitalsList.Add("TempScale_Centegrade", "TempC");

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

             // used to convert from vital 1 to other vitals
             VitalsSetConversionList.Clear();
             VitalsSetConversionList.Add("23", "11");
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
        public bool LoadButton(string Path)
        {
             return JsonMaker.ModifyArrayItem("$.['" + Path + "'].['" + (_multi_id.Length > 2 ?_multi_id:button_id) + "'].['model'].['buttons']", "id", this.button_id, "choice", "on");
        }
        public void LoadVital()
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

             if (!JsonMaker.ModifyArrayItem("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.buttons", "id", VitalID.Split('.')[1], "choice", "on"))
                  if (!JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".selected", VitalID.Split('.')[1], MustExist: true))
                       Logger.LogError("Could not load from button " + this._ButtonID + " of pcr " + this.pcr_id);
             //JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.butttons.img", "check-circle-o");
             //JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.butttons.str", VitalID.Split(',')[1]);
             //JsonMaker.UpdateJsonValue("$.Vitals_" + VitalNo.ToString() + '.' + VitalID.Split('.')[0] + ".model.butttons.choice", "on");
             
        }
        public void SaveButton(object JsonData, string pcr_id = "", string button_id="")
        {
             try
             {

                  this.pcr_id = pcr_id;

                  string str = JsonMaker.GetIOSJsonExtract("$.str", JsonData);
                  if (str=="")
                       str = JsonMaker.GetIOSJsonExtract("$.caption", JsonData)+"";
                  string choice = JsonMaker.GetIOSJsonExtract("$.choice", JsonData);

                  if (str != null && button_id=="") this.button_id = Buttons.GetIDByName(str);
                  this.Clicked = "1";
                  if (this.button_id != null)
                       if (choice + "" == "on")
                            this.HandleRecord();
                       else
                            this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });

             }
             catch (Exception ex) { Logger.LogException(ex); }
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
                       if (choice + "" == "on")
                            this.HandleRecord();
                       else
                            this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id });

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
             if (this.button_id != null)
             {
                  string DeletePattern = "Vitals" + VitalNo + "Set" + ButtonIDSuffix.ToString().Split('_')[0] + "_[" + Range + "]$";
                  DeletePrevSelections(pcr_id, DeletePattern);
                  // this.Delete(new string[] { "pcr_id", "button_id" }, new string[] { pcr_id, this.button_id.Split('_')[0]});
                  this.HandleRecord();
             }

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
                  string rv = (string)cmd.ExecuteScalar();
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
                       SqlString += " where name = '" + name.Trim() + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = (string)cmd.ExecuteScalar();
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        }

    }