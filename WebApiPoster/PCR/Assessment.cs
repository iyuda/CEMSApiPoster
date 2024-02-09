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
    
    public class Assessment:PcrBase 
        {
        public Assessment(string id): base(id,"pcr_Assessment")
            {

            }
        public Assessment(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                string OutValue;
                switch (prop.Name)
                    {
                    case "burn_1st":
                    case "burn_2nd":
                    case "burn_3rd":
                    case "total_burn":
                        if (!Utilities.IsNumeric(PcrObj[prop.Name])) OutValue = null; else OutValue = PcrObj[prop.Name];
                        break;
                    default:
                        OutValue=PcrObj[prop.Name];
                        break;
                    }
                prop.SetValue(this, OutValue);
                }
            //this.Assessment_facility_id = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //this.address_id = objAssessment.cad;
            //this.out_of_area = objAssessment.transported_from;
            //this.out_of_area_exists = null;
           
            }

        public string chief_complaint { get; set; }
        public string medications { get; set; }
        public string burn_1st { get; set; }
        public string burn_2nd { get; set; }
        public string burn_3rd { get; set; }
        public string total_burn { get; set; }
        public string onset { get; set; }
        public string minutes { get; set; }
        public string hours { get; set; }
        public string days { get; set; }
        public string weeks { get; set; }
        public string months { get; set; }
        public string neckScribble { get; set; }
        public string abdomenScribble { get; set; }
        public string chestScribble { get; set; }
        public string rightHandScribble { get; set; }
        public string leftHandScribble { get; set; }
        public string rightFootScribble { get; set; }
        public string leftFootScribble { get; set; }
        public string backScribble { get; set; }
        public string groinScribble { get; set; }
       
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction(InsertUpdate);

            }
      

        }
    }