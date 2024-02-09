using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
namespace WebApiPoster.PCR
    {
    
    public class Apcf:PcrBase 
        {
        public Apcf(string id) : base(id,"pcr_Apcf")
            {
            }
        public Apcf(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                string OutValue;
                switch (prop.Name)
                    {
                    case "patient_bed_confined":
                    case "patient_bed_confined_after_transport":
                        if (!Utilities.IsNumeric(PcrObj[prop.Name])) OutValue = null; else OutValue = PcrObj[prop.Name];
                        break;
                    default:
                        OutValue = PcrObj[prop.Name];
                        break;
                    }
                prop.SetValue(this, OutValue);
                }
            //this.Assessment_facility_id = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //this.address_id = objAssessment.cad;
            //this.out_of_area = objAssessment.transported_from;
            //this.out_of_area_exists = null;

            }
      
 
        public string reason_appointment { get; set; }
        public string reason_ambulance { get; set; }
        public string reason_medical { get; set; }
        public string physician_name { get; set; }
        public string date_appointment { get; set; }
        public string xml_physician_signature { get; set; }
        public string ems_emt_name { get; set; }
        public string patient_bed_confined { get; set; }
        public string title { get; set; }
        public string patient_bed_confined_after_transport { get; set; }

        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction( InsertUpdate);

            }

        }
    }