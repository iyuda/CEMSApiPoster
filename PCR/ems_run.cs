using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class Ems_run:PcrBase 
        {

  
        public string status { get; set; }
        public string explanation { get; set; }
        public string state_report { get; set; }
        public string billing_file { get; set; }
        public string _form_800_id { get; set; }  
        public Ems_run()
        {
             this.TableName = "ems_run";
        }
        public Ems_run(string id) :base(id,"ems_run")
            {

            }
        public Ems_run(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }
            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction( InsertUpdate);
            }

        public void HandleWithUserName(string UserName, string agency_id)
        {
             this.HandleRecord();
             User_Login user_login = new User_Login();
             user_login.UpdateUsingUserName(UserName, agency_id);
             _form_800_id = user_login.form800_id;
             Run_Logins run_login = new Run_Logins();
             run_login.user_login = user_login.id;
             run_login.ems_run = this.id;
             run_login.HandleRecord();
             
        }
        }
    
    }