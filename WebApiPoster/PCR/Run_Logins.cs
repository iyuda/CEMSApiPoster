using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
{
     public class Run_Logins : PcrBase
     {

          public string ems_run { get; set; }
          public string user_login { get; set; }

          public Run_Logins()
        {
             this.TableName = "run_logins";
        }
          public Run_Logins(string id)
               : base(id, "run_logins")
          {

          }
          public Run_Logins(string TableName, JsonInputSection PcrObj)
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