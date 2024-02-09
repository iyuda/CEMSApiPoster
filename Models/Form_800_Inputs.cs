
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class Form_800_Inputs : BaseClass
     {
  
          public string form_800_id { get; set; }
          public string all_buttons_id { get; set; }
          public string value { get; set; }

          public Form_800_Inputs()
          {
               this.TableName = "form_800_inputs";
          }
          public Form_800_Inputs(string id)
               : base(id, "form_800_inputs")
          {

          }
          public Form_800_Inputs(string TableName, JsonInputSection PcrObj)
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
          public void UpdateUsingUserName(string name, string agency_id)
          {

               this.HandleRecord();
          }
     }
}