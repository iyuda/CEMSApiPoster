using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;

namespace WebApiPoster.Models
    {
    public class Users : BaseClass
        {
        public Users() { TableName = "users"; }

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
        public string person_id { get; set; }
        public string is_admin { get; set; }
        public string is_super_admin { get; set; }
        public string active { get; set; }
        public string neighborhood { get; set; }
        public Agency agency_object { get; set; }
        public Neighborhood neighborhoodn_object { get; set; }
        public Person person_object { get; set; }
        
        public Users(string id)
            : base(id,"users")
            {
            if (!string.IsNullOrEmpty(agency_id)) agency_object = new Agency(agency_id);
            if (!string.IsNullOrEmpty(neighborhood)) neighborhoodn_object = new Neighborhood(neighborhood);
            if (!string.IsNullOrEmpty(person_id)) person_object = new Person(person_id);
            }
        public Users(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
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
                    case "person_id":
                        OutValue = String.IsNullOrEmpty(PcrObj[prop.Name]) ? null : PcrObj[prop.Name];
                        break;
                    default:
                        OutValue = PcrObj[prop.Name];
                        break;
                    }
                prop.SetValue(this, OutValue);
                }

            }

      
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {

            if (!string.IsNullOrEmpty(agency_id))
                {
                Agency agency = new Agency(agency_id);
                agency.HandleRecord();
                }
            if (!string.IsNullOrEmpty(neighborhood))
                {
                Neighborhood neighborhoodobj = new Neighborhood(neighborhood);
                neighborhoodobj.HandleRecord();
                }
            if (!string.IsNullOrEmpty(person_id)) Utilities.ValidateField("person", person_id);
            
            }
        public static string GetAgencyIDByNumber(string name)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select agency_id from users where user = '" + name.Trim() + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
       
        public static string GetBadgeByNameAndAgency(string name, string agency_id)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {

                  string SqlString;
                  if (name.Contains(","))
                  {
                       string last_name = name.Split(',')[0];
                       string first_name = name.Split(',')[1];
                       SqlString = "select u.user from users u inner join person p on u.person_id=p.id where last_name = '" + last_name.Trim() + "' and first_name = '" + first_name.Trim() + "' and agency_id = '" + agency_id + "'";
                  }
                  else
                       SqlString = "select badge from users where user = '" + name.Trim() + "' and agency_id = '" + agency_id + "'";

                  cn.Open();

                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string result = cmd.ExecuteScalar() + "";
                  return result.Split('_')[0];
                  //rv = rv == null ? "" : rv.ToString();

             }
        }
        public static string GetIDByNameAndAgency( out string badge,string name, string agency_id)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  
                  string SqlString;
                  if (name.Contains(","))
                  {
                       string last_name = name.Split(',')[0];
                       string first_name = name.Split(',')[1];
                       SqlString = "select concat(u.id,'_', u.user) from users u inner join person p on u.person_id=p.id where last_name = '" + last_name.Trim() + "' and first_name = '" + first_name.Trim() + "' and agency_id = '" + agency_id + "'";
                  }
                  else
                       SqlString = "select id from users where user = '" + name.Trim() + "' and agency_id = '" + agency_id + "'";

                  cn.Open();
                  
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string result = cmd.ExecuteScalar()+"";
                  if (result.Contains("_"))
                  {
                       badge = result.Split('_')[1];
                       return result.Split('_')[0];

                  }
                  else if (!String.IsNullOrEmpty(result))
                  {
                       badge = name;
                       return result;
                  }
                  else
                  {
                       badge = "";
                       return "";
                  }

                  //rv = rv == null ? "" : rv.ToString();
                  
             }
        }
        public static string GetEMTByAgency(string agency_id)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from users where user = '' and agency_id = '" + agency_id + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  if (rv == null)
                  {
                       rv = Guid.NewGuid().ToString();
                       Users user = new Users(rv);
                       user.agency_id = agency_id;
                       user.user = "";
                       user.HandleRecord();
                  }
                  return rv;
             }
        }
        public static string GetUserEmail(string user_id, string agency_id)
        {
            using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
            {
                cn.Open();
                string SqlString = String.Format("select e.email from person_email e inner join person p on e.person_id=p.id inner join users u on p.id=u.person_id where u.id = '{0}' and u.agency_id='{1}'", user_id, agency_id);
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                string rv = cmd.ExecuteScalar() + "";
                //rv = rv == null ? "" : rv.ToString();
                return rv;
            }
        }
        public static string SetPassword(string user_id, string pass)
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                {
                    cn.Open();
                    string SqlString = String.Format("update users set pass='{0}' where id = '{1}'", pass, user_id);
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    Logger.LogAction(DbConnect.ConnectionString + System.Environment.NewLine + SqlString, "Provisioning");
                    int rows = cmd.ExecuteNonQuery();
                    string ReturnJson;
                    if (rows == 0)
                    {
                        ReturnJson = "{success: false}";
                        ReturnJson += " \"reason\" : \"no record was updated for setting password\"}";
                        return ReturnJson;
                    }
                    ReturnJson = "{success: true}";
                    return ReturnJson;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                string ReturnJson = "{success: false,";
                ReturnJson += " \"reason\" : \"" + ex.Message + "\"}";
                return ReturnJson;
            }
        }
    }
    }