using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class Agency : PcrBase 
        {
  
        public string agency_name { get; set; }
        public string agency_number { get; set; }
        public string agency_type { get; set; }
        public string agency_npi { get; set; }
        public string agency_comments { get; set; }
        public string license_number { get; set; }
        public string license_level { get; set; }
        public string business_id { get; set; }
        public Business business_object { get; set; }

        public Agency(string id):base(id,"agency")
            {
            if (!string.IsNullOrEmpty(business_id)) business_object = new Business(business_id);
            }
        public Agency(string TableName, JsonInputSection PcrObj):base(TableName , PcrObj)
            {
            if (!string.IsNullOrEmpty(business_id)) business_object = new Business(business_id);
            }
        public Agency(object JsonData, string PathPrefix)
             
        {
             this.TableName = "Agency";
          }
        public void HandleRecord(int InsertUpdate = 0)
            {
            if (business_object != null) business_object.HandleRecord();
                //Utilities.ValidateField("business", business_id);
            this.InsertUpdateAction(InsertUpdate);
            }
        }
    }