using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;

namespace WebApiPoster.Models
    {
    public class phone_number:BaseClass 
        {


        public phone_number() { TableName = "phone_number"; }
        public phone_number(string id, string SearchField="id")
              : base(id, "phone_number", SearchField)
            {
            }
        public phone_number(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }



        public string number { get; set; }
        public string phone_type { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
                this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
        {

             if (!string.IsNullOrEmpty(phone_type))
             {
                  Phone_Type pt = new Phone_Type(phone_type);
                  pt.HandleRecord();
             }


        }
        }
     
    }