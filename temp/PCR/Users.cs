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
    public class Users : PcrBase
        {
        public Users(string TableName, string id)
            : base(TableName, id)
            {

            }
        public Users(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties())
                {
                string OutValue;
                switch (prop.Name)
                    {
                    case "is_admin":
                    case "is_super_admin":
                    case "active":
                        OutValue = !Utilities.IsNumeric(PcrObj[prop.Name]) ? null : PcrObj[prop.Name];
                        break;
                    case "agency_id":
                        OutValue = String.IsNullOrEmpty(PcrObj[prop.Name]) ? null : PcrObj[prop.Name];
                        break;
                    default:
                        OutValue = PcrObj[prop.Name];
                        break;
                    }
                prop.SetValue(this, OutValue);
                }

            }

        public string agency_id { get; set; }
        public string user { get; set; }
        public string pass { get; set; }
        public string badge_number { get; set; }
        public string license { get; set; }
        public string hire_date { get; set; }
        public string termination_date { get; set; }
        public string personnel_status { get; set; }
        public string work_status { get; set; }
        public string salary_type { get; set; }
        public string license_type { get; set; }
        public string is_admin { get; set; }
        public string is_super_admin { get; set; }
        public string active { get; set; }
        public string neighborhood { get; set; }


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
            if (!string.IsNullOrEmpty(agency_id))
                {
                Neighborhood neighborhoodobj = new Neighborhood("neighborhood", neighborhood);
                neighborhoodobj.HandleRecord();
                }

            }

        }
    }