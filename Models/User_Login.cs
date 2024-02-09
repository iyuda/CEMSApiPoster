
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class User_Login : BaseClass
     {

     public string driver_user_id { get; set; }
     public string emt_user_id { get; set; }
     public string form800_id { get; set; }
     public string bus_id { get; set; }

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
               if (this.form800_id !=null)
                    Utilities.Insert("Form_800", new string[] { "id" }, new string[] { this.form800_id });
               this.InsertUpdateAction(InsertUpdate);
          }
          public void UpdateUsingUserName(string bus_number, string driver_name, string emt_name, string agency_id)
          {
               string driver_badge;
               string emt_badge;
               driver_user_id = Users.GetIDByNameAndAgency(out driver_badge, driver_name, agency_id);
               HttpContext.Current.Session["Driver"] = driver_badge;
               emt_user_id = Users.GetIDByNameAndAgency(out emt_badge, emt_name, agency_id);
               HttpContext.Current.Session["EMT"] = emt_badge;

               bus_id = Bus.GetBusIDByAgency(agency_id, bus_number);
               this.Retrieve(new string[] { "driver_user_id", "emt_user_id", "bus_id", "DATE_FORMAT(login_time, '%m-%d-%Y')" }, new string[] { driver_user_id, emt_user_id, bus_id, DateTime.Now.ToString("MM-dd-yyyy") });
               if (form800_id == null)
               {
                    form800_id = Guid.NewGuid().ToString();
                    this.HandleRecord();
               }
          }
          public void UpdateUsingUserName(string bus_number, Members driver_member, Members emt_member, string agency_id)
          {
               driver_user_id = driver_member.user_id; // Users.GetIDByNameAndAgency(out driver_badge, driver_name, agency_id);
               HttpContext.Current.Session["Driver"] = driver_member._user;
               emt_user_id = emt_member.user_id; // Users.GetIDByNameAndAgency(out emt_badge, emt_name, agency_id);
               HttpContext.Current.Session["EMT"] = emt_member._user;

               bus_id = Bus.GetBusIDByAgency(agency_id, bus_number);
               this.Retrieve(new string[] { "driver_user_id", "emt_user_id", "bus_id", "DATE_FORMAT(login_time, '%m-%d-%Y')" }, new string[] { driver_user_id, emt_user_id, bus_id, DateTime.Now.ToString("MM-dd-yyyy") });
               if (form800_id == null)
               {
                    form800_id = Guid.NewGuid().ToString();
                    this.HandleRecord();
               }
          }
     }
}