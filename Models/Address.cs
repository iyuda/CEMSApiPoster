using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;
using System.Reflection;
namespace WebApiPoster.Models
{
    public class Address :BaseClass 
    {
        public Address()
            {
             TableName = "Address";
            }
        public Address(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public Address(string id, Boolean boolRetrieve=false)
        {
            this.id = id;
            this.TableName = "Address";
            if (boolRetrieve) this.Retrieve();
        }
        public void MapIntoIOSJson(string PathPrefix) {
             JsonMaker.UpdateJsonValue(PathPrefix + "_Address", address);
             JsonMaker.UpdateJsonValue(PathPrefix + "_Apartment", unit);
             JsonMaker.UpdateJsonValue(PathPrefix + "_City", GetCityNameById(city_id)+"");
             JsonMaker.UpdateJsonValue(PathPrefix + "_State", GetStateNameById(state_id) + "");
             JsonMaker.UpdateJsonValue(PathPrefix + (PathPrefix.Contains("CallLocation") ? "_ZipCode" : "_Zip"), GetZipNameById(zip_id) + "");
        }
        public void MapDispatchAddress(object JsonData, string PathPrefix) 
         {

         }
        public void MapPersonAddress(object JsonData, string PathPrefix)
        {

        }
        public Address(object JsonData, string PathPrefix)
             
        {
             this.TableName = "Address";
             address =JsonMaker.GetIOSJsonExtract(PathPrefix+"_Address", JsonData);
             
             unit =JsonMaker.GetIOSJsonExtract(PathPrefix+"_Apartment", JsonData);
        
             string City=JsonMaker.GetIOSJsonExtract(PathPrefix+"_City", JsonData);
             string city_id = GetCityIdByName(City);
             this.city_id = String.IsNullOrEmpty(city_id)?null:city_id;

             string State=JsonMaker.GetIOSJsonExtract(PathPrefix+"_State", JsonData);
             string state_id = GetStateIdByName(State);
             this.state_id = String.IsNullOrEmpty(state_id)?null:state_id;

             string Zip=JsonMaker.GetIOSJsonExtract(PathPrefix + (PathPrefix.Contains("Location")?"_ZipCode":"_Zip"), JsonData);
             if ((Zip + "").Length > 5) Zip = Zip.Substring(0, 5);
             string zip_id = GetZipIdByName(Zip);
             this.zip_id = String.IsNullOrEmpty(zip_id)?null:zip_id;

             string Country ="United States Of America";
             string country_id = GetCountryIdByName(Country);
             this.country_id = String.IsNullOrEmpty(country_id)?null:country_id;

    }
        //public Address(string address, string city, string state, string zip)
        //    {
        //    this.Retrieve(address, city,  state, zip);
        //    }
        public string address { get; set; }
        public string unit { get; set; }
        public string city_id { get; set; }
        public string state_id { get; set; }
        public string zip_id { get; set; }
        public string country_id { get; set; }
        public string address_type_id { get; set; }
        //public string utc_insert { get; set; }
        //public string utc_update { get; set; }
        //public string insert_id { get; set; }
        //public string update_id { get; set; }
        public void MakeSureItExists()
            {
            if (this.id + "" == "") this.id = GetAddressIdByAddress();
            //if (this.id + "" != "") 
            this.InsertUpdateAction();
            }
        public void HandleRecord()
            {
            MakeSureItExists();
            }

         public string GetCityIdByName(string city_name) 
             {
            
          
             using (MySqlConnection cn = new MySqlConnection( DbConnect.ConnectionString))
                 {
                 cn.Open();
                 string SqlString = "select id from city where city_name = '" + city_name + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 string rv = cmd.ExecuteScalar()+"";
                 //rv = rv == null ? "" : rv.ToString();
                 return rv; 
                 }
             }
         public string GetCityNameById(string id)
         {


              using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
              {
                   cn.Open();
                   string SqlString = "select city_name from city where id = '" + id + "'";
                   MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                   string rv = cmd.ExecuteScalar()+"";
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }

         public string GetStateIdByName(string state_name)
             {
            
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                 {
                 cn.Open();
                 string SqlString = "select id from state where state_name = '" + state_name + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 string rv = cmd.ExecuteScalar()+"";
                 //rv = rv == null ? "" : rv.ToString();
                 return rv; 
                 }
             }

         public string GetStateNameById(string id)
         {


              using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
              {
                   cn.Open();
                   string SqlString = "select state_name from state where id = '" + id + "'";
                   MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                   string rv = cmd.ExecuteScalar()+"";
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }
         public string GetZipIdByName(string zip_code)
             {
            
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                 {
                 cn.Open();
                 string SqlString = "select id from zip where zip_code = '" + zip_code + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 string rv = cmd.ExecuteScalar()+"";
                 //rv = rv == null ? "" : rv.ToString();
                 return rv; 
                 }
             }
         public string GetZipNameById(string id)
         {


              using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
              {
                   cn.Open();
                   string SqlString = "select LPAD( zip_code, 5, '0') from zip where id = '" + id + "'";
                   MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                   string rv = cmd.ExecuteScalar()+"";
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }
         public string GetCountryIdByName(string country_name)
             {
            
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                 {
                 cn.Open();
                 string SqlString = "select id from country where country_name = '" + country_name + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 string rv = cmd.ExecuteScalar()+"";
                 //rv = rv == null ? "" : rv.ToString();
                 return rv; 
                 }
             }
         public string GetCountryNameById(string id)
         {


              using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
              {
                   cn.Open();
                   string SqlString = "select country_name from country where id = '" + id + "'";
                   MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                   string rv = cmd.ExecuteScalar()+"";
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }

         public string GetAddressIdByAddress()
             {

             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                 {
                 cn.Open();
                 string SqlString = "select id from address where address = '" + this.address + "' and city_id = '" + this.city_id + "' and state_id = '" + this.state_id + "' and zip_id = '" + this.zip_id + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 string rv = cmd.ExecuteScalar()+"";
                 //rv = rv == null ? "" : rv.ToString();
                 return rv;
                 }
             }
    }
}