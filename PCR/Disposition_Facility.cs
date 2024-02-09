using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;

namespace WebApiPoster.PCR
    {
    public class Disposition_Facility:PcrBase 
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
            if (!string.IsNullOrEmpty(business_id)) business_object = new Business(business_id);
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