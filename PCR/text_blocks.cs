using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class text_blocks:PcrBase 
        {

        
        public string text { get; set; }
        public string language { get; set; }
        public string agency_id { get; set; }
        public Languages  language_object { get; set; }
        public Agency  agency_object { get; set; }
        
        public text_blocks(string id):base(id,"text_blocks")
            {
            if (!string.IsNullOrEmpty(language)) language_object = new Languages(language);
            if (!string.IsNullOrEmpty(agency_id)) agency_object = new Agency(agency_id);
            }
        public text_blocks(string TableName, JsonInputSection PcrObj):base(TableName , PcrObj)
            {
            
            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            if (!string.IsNullOrEmpty(language)) Utilities.ValidateField("languages", language);
            if (!string.IsNullOrEmpty(agency_id))
                {
                Agency agency = new Agency(agency_id);
                agency.HandleRecord();
                }

            this.InsertUpdateAction(InsertUpdate);
            }
        }
    }