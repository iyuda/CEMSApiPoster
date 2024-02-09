using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class dynamic_button_types : PcrBase
        {

        public string type_name { get; set; }
        public string finding_index { get; set; }

        public dynamic_button_types(string id)
            : base(id, "dynamic_button_types")
            {
            }
        public dynamic_button_types(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.")))
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