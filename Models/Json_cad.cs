using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class Json_cad:BaseClass 
     {
        public string data { get; set; }
        public string agency { get; set; }
        public string received { get; set; }
        public string sent { get; set; }
        
        public Json_cad() { TableName = "json_cad"; }
        public Json_cad(string id)
             : base(id, "json_cad", "id")
            {
            }
        public Json_cad(string pcr_id, string is_upload)
        {
             this.TableName = "json_cad"; 
             this.id = pcr_id;
        }
        public Json_cad(string TableName, JsonInputSection PcrObj)
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
                 this.InsertUpdateAction ();
            }
     }
}