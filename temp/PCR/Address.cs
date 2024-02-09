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
    public class Address :PcrBase 
    {
        public Address()
            {
            }
        
        public Address(string id, Boolean boolRetrieve=false)
        {
            this.id = id;
            this.TableName = "Address";
            if (boolRetrieve) this.Retrieve();
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
            if (this.id + "" != "") this.InsertUpdateAction();
            }
        //public Boolean Insert()
        //{
        //    try
        //    {
        //        if (this.id+""=="") this.id = Guid.NewGuid().ToString();
        //        string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //        using (MySqlConnection cn = new MySqlConnection(strConnect))
        //        {
        //            cn.Open();

        //            StringBuilder ColumnList = new StringBuilder();
        //            StringBuilder ValueList = new StringBuilder();
        //            for (int i = 0; i <= this.GetType().GetProperties().Length - 1; i++) //DataColumn col in SourceRow.Table.Columns)
        //            {
        //                ColumnList.Append(this.GetType().GetProperties()[i].Name);

        //                var Value = this.GetType().GetProperties()[i].GetValue(this, null);
        //                                       Boolean boolValue;
        //                if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
        //                    Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();
                        
        //                DateTime dateValue;
        //                if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
        //                    Value = Convert.ToDateTime(Value).ToString ("yyyy-MM-dd hh:mm:ss"); 

        //                ValueList.Append(Value == null ? "null" : "'" + Value + "'");
        //                if (i < this.GetType().GetProperties().Length - 1)
        //                {
        //                    ColumnList.Append(",");
        //                    ValueList.Append(",");
        //                }
        //            }
        //            string InsertString = "insert into Address (" + ColumnList.ToString () + ") values (" + ValueList.ToString() + ")";
        //            MySqlCommand cmd = new MySqlCommand(InsertString, cn);
        //            int rows = cmd.ExecuteNonQuery();
        //            return rows > 0;
        //        }

        //    }
        //    catch (Exception ex) { ErrorLog.LogException (ex); return false; }
        //}
        //public Boolean ExistsByID()
        //    {
        //    if (this.id + "" == "") return false;
        //    string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //    using (MySqlConnection cn = new MySqlConnection(strSQL))
        //        {
        //        cn.Open();
        //        string SqlString = "select count(*) from address where id = '" + this.id + "'";
        //        MySqlCommand cmd = new MySqlCommand(SqlString, cn);
        //        int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //        return rc > 0;
        //        }
        //    }
        // public Boolean ExistsByAddress()
        //    {
        //   string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //   using (MySqlConnection cn = new MySqlConnection(strSQL))
        //       {
        //       cn.Open();
        //       string SqlString = "select count(*) from address where address = '" + this.address + "' and city_id = '" + this.city_id + "' and state_id = '" + this.state_id + "' and zip_id = '" + this.zip_id + "'";
        //       MySqlCommand cmd = new MySqlCommand(SqlString, cn);
        //       int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //       return rc > 0;
        //       }
        //    }
         public string GetCityIdByName(string city_name)
             {
             string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
             using (MySqlConnection cn = new MySqlConnection(strSQL))
                 {
                 cn.Open();
                 string SqlString = "select id from city where city_name = '" + city_name + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 var rv = cmd.ExecuteScalar();
                 rv = rv == null ? "" : rv.ToString();
                 return rv.ToString();
                 }
             }
         public string GetStateIdByName(string state_name)
             {
             string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
             using (MySqlConnection cn = new MySqlConnection(strSQL))
                 {
                 cn.Open();
                 string SqlString = "select id from state where state_name = '" + state_name + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 var rv = cmd.ExecuteScalar();
                 rv = rv == null ? "" : rv.ToString();
                 return rv.ToString();
                 }
             }
         public string GetZipIdByName(string zip_code)
             {
             string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
             using (MySqlConnection cn = new MySqlConnection(strSQL))
                 {
                 cn.Open();
                 string SqlString = "select id from zip where zip_code = '" + zip_code + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 var rv = cmd.ExecuteScalar();
                 rv = rv == null ? "" : rv.ToString();
                 return rv.ToString ();
                 }
             }
         public string GetCountryIdByName(string country_name)
             {
             string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
             using (MySqlConnection cn = new MySqlConnection(strSQL))
                 {
                 cn.Open();
                 string SqlString = "select id from country where country_name = '" + country_name + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 var rv = cmd.ExecuteScalar();
                 rv = rv == null ? "" : rv.ToString();
                 return rv.ToString ();
                 }
             }

 
         public string GetAddressIdByAddress()
             {
             string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
             using (MySqlConnection cn = new MySqlConnection(strSQL))
                 {
                 cn.Open();
                 string SqlString = "select id from address where address = '" + this.address + "' and city_id = '" + this.city_id + "' and state_id = '" + this.state_id + "' and zip_id = '" + this.zip_id + "'";
                 MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                 var rv = cmd.ExecuteScalar();
                 rv = rv == null ? "" : rv.ToString ();
                 return rv.ToString();
                 }
             }
    }
}