using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class multi_buttons : BaseClass
        {

        public string type_name { get; set; }
        public string finding_index { get; set; }

        public multi_buttons(string id)
            : base(id, "multi_buttons")
            {
            }
        public multi_buttons(string TableName, JsonInputSection PcrObj)
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