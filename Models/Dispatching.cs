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

     public class Dispatching : BaseClass
     {
          public Dispatching()
        {
             this.TableName = "dispatching";
        }
         
          public Dispatching(string id)
               : base(id, "dispatching")
          {

          }


          public string pcr_id { get; set; }
          public string agency_id { get; set; }
          public string cad_number { get; set; }
          public string user_id { get; set; }
          public string current_user_id { get; set; }
          public string additional_crew_members { get; set; }
          public string dispatched_started { get; set; }
          public string dispatched_ended { get; set; }
          public string dispatch_switch { get; set; }
          public string update_count { get; set; }
          public string info { get; set; }
          public string neighborhood { get; set; }
          public string is_cancelled { get; set; }
          public string bus_number { get; set; }
          public string dispatch_enum { get; set; }
          public string _user { get; set; }

          public void HandleRecord(int InsertUpdate = 0)
          {
               this.InsertUpdateAction(InsertUpdate);

          }


     }
}