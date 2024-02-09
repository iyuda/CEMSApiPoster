using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using WebApiPoster.Models;
namespace WebApiPoster.Models
{


    public class Pcr:PcrBase 
    {
        public Pcr(string TableName, string id)
            : base(TableName, id)
            {
            this.Retrieve();
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
        public void HandleRecord()
            {
            ValidateFields();
            this.InsertUpdateAction();
            //if (this.Exists())
            //    this.Update();
            //else
            //    this.Insert();
            }
        public void ValidateFields()
            {
            Dispatch dispatch = new Dispatch("pcr_dispatch", pcr_dispatch_id);            
            dispatch.ValidateField();
            Demographic demographic = new Demographic("pcr_demographic", pcr_demographic_id);
            demographic.ValidateField();
            Narrative_Notes narrative_notes = new Narrative_Notes("pcr_narrative_notes", pcr_narrative_notes_id);
            narrative_notes.ValidateField();
            Rma rma = new Rma("pcr_rma", pcr_rma_id);
            rma.ValidateField();
            Disposition disposition = new Disposition("pcr_disposition", pcr_disposition_id);
            disposition.ValidateField();
            Narcotic narcotic = new Narcotic("pcr_narcotic", pcr_narcotics_id);
            narcotic.ValidateField();
            Authorization authorization = new Authorization("pcr_authorization", pcr_authorization_id);
            authorization.ValidateField();

            Utilities.ValidateField("pcr_assessment", pcr_assessment_id);
            Utilities.ValidateField("pcr_Apcf", pcr_apcf_id);
            Utilities.ValidateField("ems_run", ems_run);
            Utilities.ValidateField("agency", agency_id);
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
        //    string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //    using (MySqlConnection cn = new MySqlConnection(strSQL))
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
        //    catch (Exception ex) { ErrorLog.LogException(ex); return false; }
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
        //            foreach (var prop in this.GetType().GetProperties())
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
        //    catch (Exception ex) { ErrorLog.LogException(ex); return false; }
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