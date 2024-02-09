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
    public class Insurance : PcrBase
        {
        public Insurance() { TableName = "insurance"; }
        public Insurance(string id, string SearchField = "id")
            : base(id, "insurance", SearchField)
            {
            }
        public Insurance(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public void MapIntoIOSJson(string PathPrefix)
        {
             JsonMaker.UpdateJsonValue(PathPrefix + "_Name", name);
             JsonMaker.UpdateJsonValue(PathPrefix + "_PayerID", payer_id);
        }
        public Insurance(object JsonData, string PathPrefix)
        {
             TableName = "Insurance";
             name = JsonMaker.GetIOSJsonExtract("$." + PathPrefix + "_Name", JsonData);
             payer_id = JsonMaker.GetIOSJsonExtract("$." + PathPrefix + "_PayerID", JsonData);
             //name = JsonMaker.GetIOSJsonExtract("$." + PathPrefix + "_Name", JsonData);


        }
        public string name { get; set; }
        public string payer_id { get; set; }
        public string address_id { get; set; }
        public string active { get; set; }
        public string agency_id { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction(InsertUpdate);
            }
        }
    }