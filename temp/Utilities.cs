using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Reflection;
namespace WebApiPoster.Models
    {
    public static class Utilities
        {

        public static Boolean Insert(string TableName, string[] Columns, string[] Values)
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

        public static Boolean Insert(string TableName, object WorkObject)
            {
            try
                {
                string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();

                    StringBuilder ColumnList = new StringBuilder();
                    StringBuilder ValueList = new StringBuilder();
                    for (int i = 0; i <= WorkObject.GetType().GetProperties().Length - 1; i++)
                        {
                        ColumnList.Append(WorkObject.GetType().GetProperties()[i].Name);
                        var Value = WorkObject.GetType().GetProperties()[i].GetValue(WorkObject, null);

                        Boolean boolValue;
                        if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                            Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                        DateTime dateValue;
                        if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                            Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

                        ValueList.Append(Value == null ? "null" : "'" + Value + "'");
                        if (i < WorkObject.GetType().GetProperties().Length - 1)
                            {
                            ColumnList.Append(",");
                            ValueList.Append(",");
                            }
                        }
                    string InsertString = "insert into " + TableName + "(" + ColumnList.ToString() + ") values (" + ValueList.ToString() + ")";
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    }

                }
            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }
        public static Boolean Update(string TableName, object WorkObject, string KeyField = "id")
            {
            try
                {
                string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();
                    string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                    StringBuilder Assignments = new StringBuilder();
                    for (int i = 0; i <= WorkObject.GetType().GetProperties().Length - 1; i++)
                        {
                        var prop = WorkObject.GetType().GetProperties()[i];
                        var Value = prop.GetValue(WorkObject, null);

                        Boolean boolValue;
                        if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                            Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                        DateTime dateValue;
                        if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                            Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");


                        string MaybeComma = i < WorkObject.GetType().GetProperties().Length - 1 ? "," : "";
                        if (prop.Name != KeyField) Assignments.Append(prop.Name + " = " + (Value == null ? "null" : "'" + Value + "'") + MaybeComma + System.Environment.NewLine);
                        }
                    Assignments.Append("where id ='" + WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'"); //.GetProperties()["id"].GetValue(WorkObject,null) + "'");
                    UpdateString += Assignments.ToString();
                    MySqlCommand cmd = new MySqlCommand(UpdateString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    }

                }
            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }
        public static Boolean Exists(string TableName, string id) //object WorkObject)
            {
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
        public static Boolean Exists(string TableName, string[] Fields, string[] Values)
            {
            string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
            StringBuilder Comparisons = new StringBuilder();
            using (MySqlConnection cn = new MySqlConnection(strSQL))
                {
                cn.Open();
                for (int i = 0; i <= Fields.Length - 1; i++)
                    {
                    Comparisons.Append(Fields[i] + " = '" + Values[i] + "'" + (i < Fields.Length - 1 ? " and " : ""));
                    }
                string SqlString = "select count(*) from " + TableName + " where " + Comparisons.ToString (); // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                return rc > 0;
                }
            }
        public static void ValidateField(dynamic obj)
            {
            if (!Exists(obj.GetTableName(), obj.id))
                {
                obj.HandleRecord(InsertUpdate: 1);
                }
            }
        public static void ValidateField(string TableName, string id)
            {
            if (!Exists(TableName, id))
                Insert(TableName, new string[] { "id" }, new string[] { "'" + id + "'" });
            }
        public static void ValidateField(string TableName, string[] Fields, string[] Values)
            {
            if (!Exists(TableName, Fields, Values))
                Insert(TableName, Fields, Values);
            }
        public static void InsertUpdateAction(dynamic obj, int InsertUpdate=0) {
            switch (InsertUpdate)
                {
                case 0:
                    if (Exists(obj.TableName, obj.id))
                        Update(obj.TableName, obj);
                    else
                        Insert(obj.TableName, obj);
                    break;
                case 1:
                    Insert(obj.TableName, obj);
                    break;
                case 2:
                    Update(obj.TableName, obj);
                    break;
                }
            }
        public static System.Boolean IsNumeric (System.Object Expression)
        {
            if(Expression == null || Expression is DateTime)
                return false;

            if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;

            try 
            {
                if(Expression is string)
                    Double.Parse(Expression as string);
                else
                    Double.Parse(Expression.ToString());
                    return true;
                } catch {} // just dismiss errors but return false
                return false;
            }
        }
    }