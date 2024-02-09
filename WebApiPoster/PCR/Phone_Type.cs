using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace WebApiPoster.PCR
    {
    public class Phone_Type:PcrBase 
        {
        public Phone_Type() { TableName = "phone_type"; }
        public Phone_Type(string id, string SearchField="id")
              : base(id, "phone_type", SearchField)
            {
            }
        public Phone_Type(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }

  
        public string type { get; set; }
        public string type_info { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
                this.InsertUpdateAction(InsertUpdate);
            }
       
        }
     
    }