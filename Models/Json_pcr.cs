using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class Json_pcr:BaseClass 
     {
        public string data { get; set; }
        public string agency { get; set; }
        public string device { get; set; }
        public string received { get; set; }
        public string sent { get; set; }
        public string is_upload { get; set; }
        public string ss { get; set; }
        public string last_name { get; set; }
        public string dob { get; set; }
        
        public Json_pcr() { TableName = "json_pcr"; }
        public Json_pcr(string id)
             : base(id, "json_pcr", "id")
            {
            }
        public Json_pcr(string pcr_id, string is_upload)
        {
             this.TableName = "json_pcr"; 
             this.id = pcr_id;
             this.is_upload = is_upload;
        }
        public Json_pcr(string TableName, JsonInputSection PcrObj)
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
                 this.InsertOrUpdate (new string[] {"id", "is_upload"});
            }
     }
}