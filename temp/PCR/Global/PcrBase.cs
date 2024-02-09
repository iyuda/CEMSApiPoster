using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Data;
using WebApiPoster.Models;

namespace WebApiPoster.Models
    {
    public class PcrBase
        {
        protected JsonInputSection PcrSection;
        protected string TableName;
        public string id { get; set; }
        public string GetTableName()
            {
            return TableName;
            }
        public PcrBase()
            {

            }
        public PcrBase(string TableName, string id)
            {
            this.id = id;
            this.TableName = TableName;
            }
       

        public  Boolean Insert(string[] Columns, string[] Values)
            {
            try
                {
                string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();

                    string InsertString = "insert into " + TableName + "(" + string.Join(",", Columns).ToString() + ") values (" + string.Join(",", Values).ToString() + ")";
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    }

                }
            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }

        public  Boolean Insert()
            {
            try
                {
                if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
                string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();

                    List<string> ColumnList = new List<string>();
                    List<string> ValueList = new List<string>();
                    //for (int i = 0; i <= this.GetType().GetProperties().Length - 1; i++)
                    //    {
                    foreach (var prop in this.GetType().GetProperties())
                        {
                        var Value = prop.GetValue(this, null);
                        ColumnList.Add(prop.Name);

                        Boolean boolValue;
                        if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                            Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                        DateTime dateValue;
                        if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                            Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

                        ValueList.Add(Value == null ? "null" : "'" + Value + "'");
                        }
                    string InsertString = "insert into " + TableName + "(" + string.Join(",", ColumnList.ToArray()) + ") values (" + string.Join(",", ValueList.ToArray()) + ")";
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    }

                }
            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }
        public Boolean Update(string KeyField = "id")
            {
            try
                {
                string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();
                    string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                    List<string> Assignments = new List<string>();
                    foreach (var prop in this.GetType().GetProperties()) {
                        var Value = prop.GetValue(this, null);

                        Boolean boolValue;
                        if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                            Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                        DateTime dateValue;
                        if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                            Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

                       // string MaybeComma = i < this.GetType().GetProperties().Length - 1 ? "," : "";
                        string Element =prop.Name + " = " + (Value == null ? "null" : "'" + Value + "'");

                        if (prop.Name != KeyField) Assignments.Add(Element);
                        }
                    UpdateString += string.Join ("," +System.Environment.NewLine , Assignments.ToArray ())+" where id ='" + this.GetType().GetProperty("id").GetValue(this, null) + "'";
                    MySqlCommand cmd = new MySqlCommand(UpdateString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    }

                }
            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }

        public Boolean Retrieve()
            {
            try
                {
                string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strSQL))
                    {
                    cn.Open();
                    
                    string SqlString = "select * from " + TableName + " where ID = '" + this.id + "'";
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                   
                    if (dt.Rows.Count>0)
                        {
                        foreach (var prop in this.GetType().GetProperties())
                        {
                        prop.SetValue(this, dt.Rows[0][prop.Name].ToString());
                        }
        //                string Value = prop.GetValue(this, null) == null ? "null" : "'" + prop.GetValue(this, null) + "'";
        //                if (prop.Name != KeyField) Assignments.Append(prop.Name + " = " + Value + System.Environment.NewLine);
        //                }
                        }
                   // this.id = id == null ? null : id.ToString();
                    }
                return true;
                }

            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }

        public  Boolean Exists(string id="") //object WorkObject)
            {
            try {
            if (id=="") id =this.id;
            string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
            using (MySqlConnection cn = new MySqlConnection(strSQL))
                {
                cn.Open();
                string SqlString = "select count(*) from " + TableName + " where id = '" + id + "'"; // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                return rc > 0;
                }
            }

            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }
        public  Boolean Exists(string[] Fields, string[] Values)
            {
            try { 
            string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
            StringBuilder Comparisons = new StringBuilder();
            using (MySqlConnection cn = new MySqlConnection(strSQL))
                {
                cn.Open();
                for (int i = 0; i <= Fields.Length - 1; i++)
                    {
                    Comparisons.Append(Fields[i] + " = '" + Values[i] + "'" + (i < Fields.Length - 1 ? " and " : ""));
                    }
                string SqlString = "select count(*) from " + TableName + " where " + Comparisons.ToString(); // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                return rc > 0;
                }
            }

            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }
     
        public void ValidateField()
            {
            if (!Exists())
                Insert();
            }
        public void ValidateField(string[] Fields, string[] Values)
            {
            if (!Exists(Fields, Values))
                Insert(Fields, Values);
            }

        public void InsertUpdateAction(int InsertUpdate = 0)
            {
            switch (InsertUpdate)
                {
                case 0:
                    if (Exists())
                        Update();
                    else
                        Insert();
                    break;
                case 1:
                    Insert();
                    break;
                case 2:
                    Update();
                    break;
                }
            }
        }
    }