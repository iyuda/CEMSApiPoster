using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;

namespace WebApiPoster.PCR
    {
    public class Business:PcrBase 
        {
        public Business()
            {

            }
        public Business(string id, Boolean boolRetrieve = false)
        {
            this.id = id;
            this.TableName = "business";
            if (boolRetrieve) this.Retrieve();
        }
        public Business(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public void MapIntoIOSJson(string PathPrefix)
        {
             JsonMaker.UpdateJsonValue(PathPrefix + "_Name", name);
        }
        public Business(object JsonData, string PathPrefix, string is_primary = "1")
             
        {
             this.TableName = "business";
             name =JsonMaker.GetIOSJsonExtract(PathPrefix+"_Name", JsonData);
            
    }
        //public Facility(string facility_name)
        //    {
        //    this.Retrieve(facility_name);
        //    }
        public string name { get; set; }

        public void MakeSureItExists()
            {
            if (this.id + "" == "") this.id = GetFacilityIdByName();
            //if (this.id + "" != "")
                 this.InsertUpdateAction(); 
            }
        public void HandleRecord()
            {
            MakeSureItExists();
            }
        public string GetFacilityIdByName()
            {
           
            using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                {
                cn.Open();
                string SqlString = "select id from business where name = '" + this.name + "'";
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                string rv = (string) cmd.ExecuteScalar();
                //rv = rv == null ? "" : rv.ToString();
                return rv; 
                }
            }
        //public Boolean Insert()
        //    {
        //    try
        //        {
        //        this.id = Guid.NewGuid().ToString();
        //        string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //        using (MySqlConnection cn = new MySqlConnection(strConnect))
        //            {
        //            cn.Open();

        //            StringBuilder ColumnList = new StringBuilder();
        //            StringBuilder ValueList = new StringBuilder();
        //            for (int i = 0; i <= this.GetType().GetProperties().Length - 1; i++) //DataColumn col in SourceRow.Table.Columns)
        //                {
        //                ColumnList.Append(this.GetType().GetProperties()[i].Name);

        //                var Value = this.GetType().GetProperties()[i].GetValue(this, null);
        //                Boolean boolValue;
        //                if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
        //                    Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

        //                DateTime dateValue;
        //                if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
        //                    Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

        //                ValueList.Append(Value == null ? "null" : "'" + Value + "'");
        //                if (i < this.GetType().GetProperties().Length - 1)
        //                    {
        //                    ColumnList.Append(",");
        //                    ValueList.Append(",");
        //                    }
        //                }
        //            string InsertString = "insert into business (" + ColumnList.ToString() + ") values (" + ValueList.ToString() + ")";
        //            MySqlCommand cmd = new MySqlCommand(InsertString, cn);
        //            int rows = cmd.ExecuteNonQuery();
        //            return rows > 0;
        //            }

        //        }
        //    catch (Exception ex) { Logger.LogException(ex); return false; }
        //    }

        //public Boolean Exists()
        //    {
        //    return this.id != null;
        //    //string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //    //using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
        //    //    {
        //    //    cn.Open();
        //    //    string SqlString = "select count(*) from business where name = '" + this.name + "'";
        //    //    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
        //    //    int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //    //    return rc > 0;
        //    //    }
        //    }

        //public Boolean Retrieve(string facility_name)
        //    {
        //    try
        //        {
        //       
        //        using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
        //            {
        //            cn.Open();
        //            this.name = facility_name;
        //            string SqlString = "select id from business where name = '" + facility_name + "'";
        //            MySqlCommand cmd = new MySqlCommand(SqlString, cn);
        //            var id = cmd.ExecuteScalar();
        //            this.id = id == null ? null : id.ToString();
        //            }
        //        return true;
        //        }
        //    catch (Exception ex) { Logger.LogException(ex); return false; }
        //    }
        
        }
    }