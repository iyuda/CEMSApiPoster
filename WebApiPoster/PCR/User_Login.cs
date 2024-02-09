
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
{
     public class User_Login : PcrBase
     {

     public string driver_user_id { get; set; }
     public string emt_user_id { get; set; }
     public string form800_id { get; set; }


     public User_Login()
        {
             this.TableName = "user_login";
        }
          public User_Login(string id)
               : base(id, "user_login")
          {

          }
          public User_Login(string TableName, JsonInputSection PcrObj)
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
               Utilities.Insert("Form_800", new string[] { "id" }, new string[] { this.form800_id });
               this.InsertUpdateAction(InsertUpdate);
          }
          public void UpdateUsingUserName(string name, string agency_id)
          {
               driver_user_id = Users.GetDriverIDByName(name, agency_id); 
               emt_user_id = Users.GetEMTByAgency( agency_id);
               form800_id = Guid.NewGuid().ToString();
               this.HandleRecord();
          }
     }
}