using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class ems_run:PcrBase 
        {

  
        public string status { get; set; }
        public string explanation { get; set; }
        public string state_report { get; set; }
        public string billing_file { get; set; }


        public ems_run(string TableName, string id) :base(TableName ,id)
            {
            
            }
        public ems_run(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties())
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }
            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction( InsertUpdate);
            }
        }
    }