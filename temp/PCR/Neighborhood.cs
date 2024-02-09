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
    public class Neighborhood:PcrBase 
        {
        public Neighborhood(string TableName, string id)
            : base(TableName, id)
            {
            
            }

        public Neighborhood(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties())
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }  
            }
      

        public string name { get; set; }
        public string agency_id { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {

            if (!string.IsNullOrEmpty(agency_id))
                {
                Agency agency = new Agency("agency", agency_id);
                agency.HandleRecord();
                }


            }

        }
    }