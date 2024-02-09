using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;

namespace WebApiPoster.Models
    {
    public class Disposition_Facility:BaseClass 
        {
        public Disposition_Facility()
            {

            }
        public Disposition_Facility(string id):base(id, "Disposition_Facility")
            {
            InitObjects();
            }
        public Disposition_Facility(string TableName, JsonInputSection PcrObj)
            : base(TableName, PcrObj)
            {
            InitObjects();
            }
        public Disposition_Facility(object JsonData, string PathPrefix)
        {
             this.TableName = "disposition_facility";

             Business business = new Business(JsonData, PathPrefix);
             if (!string.IsNullOrEmpty(business.name))
             {
                  business.HandleRecord();
                  this.business_id = business.id;
             }
             string responded_from = JsonMaker.GetIOSJsonExtract("$.Location@Location_RespondedFrom", JsonData);
             if (!String.IsNullOrEmpty(responded_from))
                  this.disposition_type_id = GetDispositionTypeIdByName(responded_from.Split(',')[0]);
             //if (!String.IsNullOrEmpty(disposition_type_id))
                  HandleRecord();


        }
        public string GetDispositionTypeIdByName(string name)
        {


             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from disposition_type where type_name = '" + name + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        
        public string business_id { get; set; }
        public string disposition_code { get; set; }
        public string city_code { get; set; }
        public string npi { get; set; }
        public string disposition_type_id { get; set; }
        public string active { get; set; }
        public string agency_id { get; set; }
        public Business business_object { get; set; }
        public Disposition_Type disposition_type_object { get; set; }

        private void InitObjects()
            {
            if (!string.IsNullOrEmpty(business_id)) business_object = new Business(business_id,true);
            if (!string.IsNullOrEmpty(disposition_type_id)) disposition_type_object = new Disposition_Type(disposition_type_id);
            }


        public void HandleRecord(int InsertUpdate = 0)
            {
            if (business_object != null) business_object.HandleRecord();
            if (disposition_type_object != null) disposition_type_object.HandleRecord();
            //if (!string.IsNullOrEmpty(business_id)) Utilities.ValidateField("business", business_id);
            //if (!string.IsNullOrEmpty(disposition_type_id)) Utilities.ValidateField("disposition_type", disposition_type_id);
            this.InsertUpdateAction(InsertUpdate);
            }
        
        }
    }