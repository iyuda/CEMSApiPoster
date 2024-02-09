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
    public class personal_addresses:BaseClass 
        {
        public personal_addresses() { TableName = "person_phones"; }
        public personal_addresses(string id, string SearchField="id")
            : base(id, "person_phones", SearchField)
            {
            }
        public personal_addresses(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public personal_addresses(object JsonData, string PathPrefix, string is_primary = "1")
        {
             this.TableName = "personal_addresses";
             Address address_object = new Address(JsonData, PathPrefix);
             address_object.HandleRecord();
             this.address =address_object.id;             
             this.is_primary = is_primary;
           }


        public string address { get; set; }
        public string person { get; set; }
        public string is_primary { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
                //this.ValidateFields();
                this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
        {

             if (!string.IsNullOrEmpty(person))
             {
                  Person person_object = new Person(person);
                  person_object.HandleRecord();
             }
            


        }
        }
     
    }