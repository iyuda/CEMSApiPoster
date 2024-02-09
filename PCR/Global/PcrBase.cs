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
using WebApiPoster.PCR;

namespace WebApiPoster.PCR
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
        public PcrBase(string id, string TableName, string SearchField="id")
            {
            this.id = id;
            this.TableName = TableName;
            this.Retrieve(SearchField);
        //    this.TableName = TableName;
            }

        public PcrBase(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public  Boolean Insert(string[] Columns, string[] Values)
            {
            try
                {
               string strConnect;
               if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
               if (this.GetType().Name != "Json_pcr")
               {
                    strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
               }
               else
               {
                    MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                    builder.Database = "IOS";
                    strConnect = builder.GetConnectionString(true);
               }
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();

                    string InsertString = "insert into " + TableName + "(" + string.Join(",", Columns).ToString() + ") values (" + string.Join(",", Values).ToString() + ")";
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    Logger.LogAction(InsertString);
                    }

                }
            catch (Exception ex) { Logger.LogException(ex); return false; }
            }

        public  Boolean Insert()
            {
            try
                {
                
                string strConnect;
                if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
                if (this.GetType().Name != "Json_pcr")
                {
                     strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                }
                else
                {
                     MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                     builder.Database = "IOS";
                     strConnect = builder.GetConnectionString(true);
                }
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();

                    List<string> ColumnList = new List<string>();
                    List<string> ValueList = new List<string>();

                    foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                        {
                        var Value = prop.GetValue(this, null);
                        var temp = Value + "";
                        if (Value != null) Value = Value.ToString().Replace("~", "");
                        ColumnList.Add(prop.Name);

                        Boolean boolValue;
                        if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                            Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                        DateTime dateValue;
                        if (!temp.ToString().Contains("~"))
                             if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                                 Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd HH:mm:ss");

                        if (Value != null)
                             ValueList.Add(Value == null ? "null" : "'" + Value + "'");
                        else
                             ColumnList.Remove(prop.Name);
                        }
                    string InsertString = "insert into " + TableName + "(" + string.Join(",", ColumnList.ToArray()) + ")" +System.Environment.NewLine + "values" + System.Environment.NewLine + "(" + string.Join(",", ValueList.ToArray()) + ")";
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    Logger.LogAction(InsertString);                    
                    return rows > 0;
                    }

                }
            catch (Exception ex) { Logger.LogException(ex); return false; }
            }
        public Boolean Delete(string[] Fields, string[] Values)
        {
             try
             {
               StringBuilder Comparisons = new StringBuilder();
               using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
               {
                    cn.Open();
                    for (int i = 0; i <= Fields.Length - 1; i++)
                    {
                         Comparisons.Append(Fields[i] + " REGEXP '" + Values[i] + "'" + (i < Fields.Length - 1 ? " and " : ""));
                    }


                    string DeleteString = "delete from " + TableName + " where " + Comparisons.ToString(); // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                    MySqlCommand cmd = new MySqlCommand(DeleteString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    Logger.LogAction(DeleteString);
               }

             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public Boolean Update(string KeyField = "id")
            {
            try
                {
                 
                
                 string strConnect;
                   if (this.GetType().Name != "Json_pcr")
                   {
                         strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                   }
                   else
                   {
                         MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                         builder.Database = "IOS";
                         strConnect=builder.GetConnectionString(true);
                   }
                        using (MySqlConnection cn = new MySqlConnection(strConnect))
                        {
                             cn.Open();
                             string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                             List<string> Assignments = new List<string>();
                             foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                             {
                                  var Value = prop.GetValue(this, null);
                                  var temp = Value + "";
                                  if (Value != null) Value = Value.ToString().Replace("~", "");
                                  Boolean boolValue;
                                  if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                                       Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                                  DateTime dateValue;
                                  if (!temp.ToString().Contains("~"))
                                       if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                                            Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

                                  // string MaybeComma = i < this.GetType().GetProperties().Length - 1 ? "," : "";
                                  if (Value != null)
                                  {
                                       string Element = prop.Name + " = " + (Value == null ? "null" : "'" + Value + "'");
                                       if (prop.Name != KeyField) Assignments.Add(Element);
                                  }
                             }
                             string Id = this.id; //this.GetType().GetProperty("id").GetValue(this, null).ToString ();
                             if (Assignments.Count > 0)
                             {
                                  UpdateString += string.Join("," + System.Environment.NewLine, Assignments.ToArray()) + " where id ='" + this.id + "'";
                                  MySqlCommand cmd = new MySqlCommand(UpdateString, cn);
                                  int rows = cmd.ExecuteNonQuery();
                                  Logger.LogAction(UpdateString);
                                  return rows > 0;
                             }
                             else
                                  return true;
                        }

                }
            catch (Exception ex) { Logger.LogException(ex); return false; }
            }

        public Boolean Retrieve(string SearchField="id")
            {
            try
                {
                     string strConnect;
                     if (this.GetType().Name != "Json_pcr")
                     {
                          strConnect = DbConnect.ConnectionString;
                     }
                     else
                     {
                          MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                          builder.Database = "IOS";
                          strConnect = builder.GetConnectionString(true);
                     }
                     using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();
                    
                    string SqlString = "select * from " + TableName + " where "+ SearchField + " = '" + this.id.ToString () + "'";
                   // MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    DataTable dt = DbConnect.GetDataTable(cn, SqlString);
                    //dt.Load(cmd.ExecuteReader());
                   
                    if (dt.Rows.Count>0)
                        {
                             foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                        {
                        if (prop.PropertyType.FullName.StartsWith("System."))
                            {
                            //string PropName = prop.Name;
                            //if (PropName.StartsWith("_")) PropName = PropName.Remove(0, 1);
                            //if (!Utilities.UseRequiredFields || (Utilities.UseRequiredFields && !prop.Name.StartsWith("_")))
                            //    {
                            //    string Value = dt.Rows[0][PropName].ToString();
                            //    prop.SetValue(this, Value == "" ? null : Value);
                            //    }
                            string Value = dt.Rows[0][prop.Name].ToString();
                            prop.SetValue(this, Value == "" ? null : Value);
                            }
                        }
        //                string Value = prop.GetValue(this, null) == null ? "null" : "'" + prop.GetValue(this, null) + "'";
        //                if (prop.Name != KeyField) Assignments.Append(prop.Name + " = " + Value + System.Environment.NewLine);
        //                }
                        }
                   // this.id = id == null ? null : id.ToString();
                    }
                return true;
                }

            catch (Exception ex) { Logger.LogException(ex); return false; }
            }

        public  Boolean Exists(string id="") //object WorkObject)
            {
            try {

            string strConnect;
            if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
            if (id == "") id = this.id;
            if (this.GetType().Name != "Json_pcr")
            {
                 strConnect = DbConnect.ConnectionString;
            }
            else
            {
                 MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                 builder.Database = "IOS";
                 strConnect = builder.GetConnectionString(true);
            }
            using (MySqlConnection cn = new MySqlConnection(strConnect))
                {
                cn.Open();
                string SqlString = "select count(*) from " + TableName + " where id = '" + id + "'"; // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                return rc > 0;
                }
            }

            catch (Exception ex) { Logger.LogException(ex); return false; }
            }
        public  Boolean Exists(string[] Fields, string[] Values)
            {
            try { 
           
            StringBuilder Comparisons = new StringBuilder();
            string strConnect;
            if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
            if (this.GetType().Name != "Json_pcr")
            {
                 strConnect = DbConnect.ConnectionString;
            }
            else
            {
                 MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                 builder.Database = "IOS";
                 strConnect = builder.GetConnectionString(true);
            }
            using (MySqlConnection cn = new MySqlConnection(strConnect))
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

            catch (Exception ex) { Logger.LogException(ex); return false; }
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