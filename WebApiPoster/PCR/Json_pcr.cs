using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
{
     public class Json_pcr:PcrBase 
     {

        public string data { get; set; }
        public string received { get; set; }
        public string sent { get; set; }
        
        public Json_pcr() { TableName = "json_pcr"; }
        public Json_pcr(string id, string SearchField="id")
             : base(id, "json_pcr", SearchField)
            {
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
                this.InsertUpdateAction(InsertUpdate);
            }
     }
}