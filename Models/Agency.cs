using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace WebApiPoster.Models
    {
    public class Agency : BaseClass 
        {
  
        public string agency_name { get; set; }
        public string agency_number { get; set; }
        public string agency_type { get; set; }
        public string agency_npi { get; set; }
        public string agency_comments { get; set; }
        public string license_number { get; set; }
        public string license_level { get; set; }
        public string business_id { get; set; }
        public Business business_object { get; set; }

        public Agency(string id):base(id,"agency")
            {
            if (!string.IsNullOrEmpty(business_id)) business_object = new Business(business_id);
            }
        public Agency(string TableName, JsonInputSection PcrObj):base(TableName , PcrObj)
            {
            if (!string.IsNullOrEmpty(business_id)) business_object = new Business(business_id);
            }
        public Agency(object JsonData, string PathPrefix)
             
        {
             this.TableName = "Agency";
          }
        public void HandleRecord(int InsertUpdate = 0)
            {
            if (business_object != null) business_object.HandleRecord();
                //Utilities.ValidateField("business", business_id);
            this.InsertUpdateAction(InsertUpdate);
            }

        public static string GetAgencyIDByNumber(string name)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from agency where agency_number = '" + name.Trim() + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        public static string GetAgencyNumberByID(string agency_id)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select agency_number from agency where id = '" + agency_id.Trim() + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar() + "";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        }

    }