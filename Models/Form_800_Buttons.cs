
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class Form_800_Buttons : BaseClass
     {

          public string form_800_id { get; set; }
          public string all_buttons { get; set; }
          public string clicked { get; set; }
          public string _name { get; set; }
          public string _type_name { get; set; }
          public string _xml_ambulance { get; set; }
          public string _xml_sig { get; set; }
          public string _technician_signature { get; set; }

          public Form_800_Buttons()
          {
               this.TableName = "form_800_buttons";
          }
          public Form_800_Buttons(string id)
               : base(id, "form_800_buttons")
          {

          }
          public Form_800_Buttons(string TableName, JsonInputSection PcrObj)
          {
               this.TableName = TableName;
               this.PcrSection = PcrObj;
               foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.")))
               {
                    prop.SetValue(this, PcrObj[prop.Name]);
               }
          }
          public void HandleRecord(string[] WhereColumns = null, string[] WhereValues = null)
          {
               this.InsertUpdateAction(0, WhereColumns, WhereValues);
          }
        
     }
}