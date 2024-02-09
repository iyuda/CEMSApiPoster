using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;

namespace WebApiPoster.Models
    {
    public class Facility:PcrBase 
        {
        public Facility()
            {

            }
        public Facility(string id, Boolean boolRetrieve = false)
        {
            this.id = id;
            this.TableName = "business";
            if (boolRetrieve) this.Retrieve();
        }
        //public Facility(string facility_name)
        //    {
        //    this.Retrieve(facility_name);
        //    }
        public string name { get; set; }

        public void MakeSureItExists()
            {
            if (this.id + "" == "") this.id = GetFacilityIdByName();
            if (this.id + "" != "") this.InsertUpdateAction(); 
            }
        public string GetFacilityIdByName()
            {
            string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
            using (MySqlConnection cn = new MySqlConnection(strSQL))
                {
                cn.Open();
                string SqlString = "select id from business where name = '" + this.name + "'";
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                var rv = cmd.ExecuteScalar();
                rv = rv == null ? "" : rv.ToString();
                return rv.ToString();
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
        //    catch (Exception ex) { ErrorLog.LogException(ex); return false; }
        //    }

        //public Boolean Exists()
        //    {
        //    return this.id != null;
        //    //string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //    //using (MySqlConnection cn = new MySqlConnection(strSQL))
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
        //        string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //        using (MySqlConnection cn = new MySqlConnection(strSQL))
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
        //    catch (Exception ex) { ErrorLog.LogException(ex); return false; }
        //    }
        
        }
    }