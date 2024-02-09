
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
{
     public class Form_800_Buttons : PcrBase
     {

          public string form_800_id { get; set; }
          public string all_buttons { get; set; }
          public string clicked { get; set; }
          public string _name { get; set; }
          public string _type_name { get; set; }

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
          public void HandleRecord(int InsertUpdate = 0)
          {
               this.InsertUpdateAction(InsertUpdate);
          }
          public void UpdateUsingUserName(string name, string agency_id)
          {
               //driver_user_id = Users.GetDriverIDByName(name, agency_id);
               //emt_user_id = Users.GetEMTByAgency(agency_id);
               //form800_id = Guid.NewGuid().ToString();
               this.HandleRecord();
          }
     }
}