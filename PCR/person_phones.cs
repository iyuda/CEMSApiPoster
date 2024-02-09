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
    public class person_phones:PcrBase 
        {
        public person_phones() { TableName = "person_phones"; }
        public person_phones(string id, string SearchField="id")
            : base(id, "person_phones", SearchField)
            {
            }
        public person_phones(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public person_phones(object JsonData, string PathPrefix, string is_primary = "1")
             : base(Guid.NewGuid().ToString(), "person_phones", "id")    
        {
             phone_id = Guid.NewGuid().ToString();
             phone_number number = new phone_number(phone_id);
             number.number = JsonMaker.GetIOSJsonExtract("$." + PathPrefix , JsonData);
             if (number.number != null)
                  number.HandleRecord();
             else
                  return;
             this.is_primary = is_primary;
           }


        public string person_id { get; set; }
        public string phone_id { get; set; }
        public string is_primary { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
                this.ValidateFields();
                this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
        {

             if (!string.IsNullOrEmpty(person_id))
             {
                  Person person = new Person(person_id);
                  person.HandleRecord();
             }
             if (!string.IsNullOrEmpty(phone_id))
             {
                  phone_number phone = new phone_number(phone_id);
                  phone.HandleRecord();
             }


        }
        }
     
    }