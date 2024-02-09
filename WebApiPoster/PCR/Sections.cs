using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class Sections : PcrBase
        {

        public string section_name { get; set; }
        public string section_label { get; set; }
        public string section_id { get; set; }
        public string numeric_order { get; set; }

        public Sections(string id)
            : base(id, "Sections")
            {
            }
        public Sections(string TableName, JsonInputSection PcrObj)
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