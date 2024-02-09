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
     public class BaseClass
        {
        protected JsonInputSection PcrSection;
        protected string TableName;
        public string id { get; set; }
        public string GetTableName()
            {
            return TableName;
            }
        public BaseClass()
            {

            }
        public BaseClass(string id, string TableName, string SearchField="id")
            {
            this.id = id;
            this.TableName = TableName;
            this.Retrieve(SearchField);
        //    this.TableName = TableName;
            }

        public BaseClass(string TableName, JsonInputSection PcrObj)
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
               if (!this.GetType().Name.ToLower().StartsWith("json_"))
               {
                    strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
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
                    if (!this.GetType().Name.ToLower().StartsWith("json_"))
                         Logger.LogAction(InsertString);
                    }

                }
            catch (Exception ex) { Logger.LogException(ex); return false; }
            }
        public Boolean InsertOrUpdate(string[] PrimaryKeys)
        {
             try
             {

                  string strConnect;
                  if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
                  if (!this.GetType().Name.ToLower().StartsWith("json_"))
                  {
                       strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                  }
                  else
                  {
                       MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                       builder.Database = "IOS";
                       strConnect = builder.GetConnectionString(true);
                  }


                  //string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                  //List<string> Assignments = new List<string>();
                  //foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                  //{
                  //     var Value = prop.GetValue(this, null);
                  //     var temp = Value + "";
                  //     if (Value != null) Value = Value.ToString().Replace("~", "");
                  //     Boolean boolValue;
                  //     if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                  //          Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                  //     DateTime dateValue;
                  //     if (!temp.ToString().Contains("~"))
                  //          if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                  //               Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

                  //     // string MaybeComma = i < this.GetType().GetProperties().Length - 1 ? "," : "";
                  //     if (Value != null)
                  //     {
                  //          if (Value.ToString().Contains("'")) Value = Value.ToString().Replace("'", @"\'");
                  //          string Element = prop.Name + " = " + (Value == null ? "null" : "'" + Value + "'");
                  //          if (prop.Name != KeyField) Assignments.Add(Element);
                  //     }
                  //}
                  using (MySqlConnection cn = new MySqlConnection(strConnect))
                  {
                       cn.Open();

                       List<string> ColumnList = new List<string>();
                       List<string> ValueList = new List<string>();
                       List<string> Assignments = new List<string>();
                       foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
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
                            {
                                 if (Value.ToString().Contains("'")) Value = Value.ToString().Replace("'", @"\'");
                                 string Element = prop.Name + " = " + (Value == null ? "null" : "'" + Value + "'");
                                 if (!PrimaryKeys.Contains( prop.Name)) Assignments.Add(Element);
                                 ValueList.Add(Value == null ? "null" : "'" + Value + "'");
                            }
                            else
                                 ColumnList.Remove(prop.Name);
                       }
                       string InsertString = "insert into " + TableName + "(" + string.Join(",", ColumnList.ToArray()) + ")" + System.Environment.NewLine + "values" + System.Environment.NewLine + "(" + string.Join(",", ValueList.ToArray()) + ")";
                       if (Assignments.Count > 0)
                       {
                            InsertString += System.Environment.NewLine + " ON DUPLICATE KEY UPDATE " + System.Environment.NewLine;
                            InsertString += string.Join("," + System.Environment.NewLine, Assignments.ToArray());
                       }
                       MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                       int rows = cmd.ExecuteNonQuery();
                       if (!this.GetType().Name.ToLower().StartsWith("json_")) Logger.LogAction(InsertString);
                       return rows > 0;
                  }

             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        } 
        public  Boolean Insert()
            {
            string ErrorButtonMsg="";
            try
                {
                
                string strConnect;
                if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
                MySqlConnection cn=null;
                if (!this.GetType().Name.ToLower().StartsWith("json_"))
                {
                     strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                     if (HttpContext.Current.Session["Connection"] != null)
                     {
                          cn = (MySqlConnection)HttpContext.Current.Session["Connection"];
                          if (cn.State != ConnectionState.Open) cn.Open();
                     }
                }
                else
                {
                     MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                     builder.Database = "IOS";
                     strConnect = builder.GetConnectionString(true);
                }
               bool CloseConnection=false;
               if (cn == null)
               {
                    cn = new MySqlConnection(strConnect);
                    CloseConnection = true;
               }
                if (cn.State != ConnectionState.Open) cn.Open();
                //using (MySqlConnection cn = new MySqlConnection(strConnect))
                //    {
                    //cn.Open();

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
                        Decimal decValue;
                        bool IsDate=false;
                        if (!temp.ToString().Contains("~"))   //  having "~" in a value indicates that it should not be attemtped date parsing
                             if (!Utilities.IsNumeric(Value) && DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                             { 
                                 Value = "DATE_FORMAT('"+ Convert.ToDateTime(Value).ToString("yyyy-MM-dd HH:mm:ss")+ "', '%Y:%m:%d %H:%i:%s')";
                                 IsDate = true;
                              }

                        if (prop.Name.Contains("signature") && !(Value ??"").ToString().Contains("data:image/png;base64,"))
                             Value = "data:image/png;base64," + Value; 
                        if (Value != null)
                        {

                             if (!IsDate)
                             {
                                  if (Value.ToString().Contains("'")) Value = Value.ToString().Replace("'", @"\'");
                                  ValueList.Add(Value == null ? "null" : "'" + Value + "'");
                             }
                             else
                                  ValueList.Add(Value == null ? "null" : Value.ToString());
                        }
                        else
                             ColumnList.Remove(prop.Name);
                        }
                    string InsertString = "insert into " + TableName + "(" + string.Join(",", ColumnList.ToArray()) + ")" +System.Environment.NewLine + "values" + System.Environment.NewLine + "(" + string.Join(",", ValueList.ToArray()) + ")";
                    
                    if (TableName.ToLower() == "pcr_buttons")
                         ErrorButtonMsg = "Error inserting button_id " + this.GetType().GetProperties().First(prop => prop.Name == "button_id").GetValue(this).ToString() + " into " + TableName + System.Environment.NewLine;
                    if (TableName.ToLower() == "pcr_inputs")
                         ErrorButtonMsg = "Error inserting input_id " + this.GetType().GetProperties().First(prop => prop.Name == "input_id").GetValue(this).ToString() + " into " + TableName + System.Environment.NewLine;
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    if (!this.GetType().Name.ToLower().StartsWith("json_")) Logger.LogAction(InsertString);                    
                    
               //     }
                    if (CloseConnection) cn.Close();
                    return rows > 0;
                }
            catch (Exception ex) { Logger.LogException(ex, ErrorButtonMsg : ErrorButtonMsg); return false; }
            }
        public Boolean Delete(string[] Fields, string[] Values)
        {
             try
             {
               StringBuilder Comparisons = new StringBuilder();
               bool CloseConnection = false;
               MySqlConnection cn = (MySqlConnection)HttpContext.Current.Session["Connection"];
               if (cn == null)
               {
                    cn = new MySqlConnection(DbConnect.ConnectionString);
                    CloseConnection = true;
               }
               if (cn.State != ConnectionState.Open) cn.Open();
               //using (MySqlConnection cn = new MySqlConnection(strConnect))
               //    {
               //cn.Open();
                    for (int i = 0; i <= Fields.Length - 1; i++)
                    {
                         Comparisons.Append(Fields[i] + " = '" + Values[i] + "'" + (i < Fields.Length - 1 ? " and " : ""));
                    }


                    string DeleteString = "delete from " + TableName + " where " + Comparisons.ToString(); // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                    MySqlCommand cmd = new MySqlCommand(DeleteString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    if (CloseConnection) cn.Close();
                    if (!this.GetType().Name.ToLower().StartsWith("json_")) Logger.LogAction(DeleteString);
                    return rows > 0;
              // }

             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public Boolean Update(string[] Columns, string[] WhereColumns)
        {
             try
             {
                  string strConnect;
                  if (!this.GetType().Name.ToLower().StartsWith("json_"))
                  {
                       strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
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
                       string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                       StringBuilder Assignments = new StringBuilder();
                       for (int i = 0; i <= Columns.Length - 1; i++)
                       {
                            var column = Columns[i];
                            var Value = this.GetType().GetProperties().First(prop => prop.Name == column).GetValue(this,null);
                            var temp = Value + "";
                            if (Value != null) Value = Value.ToString().Replace("~", "");
                            Boolean boolValue;
                            if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                                 Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                            DateTime dateValue;
                            if (!Utilities.IsNumeric(Value) && DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                                 Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");


                            string MaybeComma = i < Columns.Length - 1 ? "," : "";
                            if (column.Contains("signature") && !(Value ?? "").ToString().Contains("data:image/png;base64,"))
                                 Value = "data:image/png;base64," + Value; 
                            if (column != "id") Assignments.Append(column + " = " + (Value == null ? "null" : "'" + Value + "'") + MaybeComma + System.Environment.NewLine);
                       }
                       StringBuilder Comparisons = new StringBuilder();
                       for (int i = 0; i <= WhereColumns.Length - 1; i++)
                       {
                            string MaybeAnd = i < WhereColumns.Length - 1 ? " and " : "";
                            Comparisons.Append(WhereColumns[i] + " = '" + this.GetType().GetProperties().First(prop=> prop.Name==WhereColumns[i]).GetValue(this,null) + "'" + MaybeAnd + System.Environment.NewLine);
                       }
                       if (WhereColumns.Length>0)
                            Assignments.Append("where " + Comparisons.ToString()); //.GetProperties()["id"].GetValue(WorkObject,null) + "'");
                       UpdateString += Assignments.ToString();
                       MySqlCommand cmd = new MySqlCommand(UpdateString, cn);
                       int rows = cmd.ExecuteNonQuery();
                       return rows > 0;
                  }

             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public Boolean Update(bool DynamicWhere=false, string[] WhereColumns = null, string[] WhereValues = null)
            {
            try
                {
                 if (WhereColumns==null)  WhereColumns=new string[]{"id"};
                 string strConnect;
                 MySqlConnection cn = null;
                   if (!this.GetType().Name.ToLower().StartsWith("json_"))
                   {
                         strConnect = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
                         if (HttpContext.Current.Session["Connection"] != null)
                         {
                              cn = (MySqlConnection)HttpContext.Current.Session["Connection"];
                              if (cn.State != ConnectionState.Open) cn.Open();
                         }
                   }
                   else
                   {
                         MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                         builder.Database = "IOS";
                         strConnect=builder.GetConnectionString(true);
                   }
                  bool CloseConnection=false;
               if (cn == null)
               {
                    cn = new MySqlConnection(strConnect);
                    CloseConnection = true;
               }
                if (cn.State != ConnectionState.Open) cn.Open();
                        //using (MySqlConnection cn = new MySqlConnection(strConnect))
                        //{
                        //     cn.Open();
                             string UpdateString = "update " + TableName + " set " + System.Environment.NewLine;
                             List<string> Assignments = new List<string>();
                             foreach (var prop in this.GetType().GetProperties().Where(prop => !WhereColumns.ToList().Contains(prop.Name) && prop.Name!="id" && prop.PropertyType.FullName.StartsWith("System.") && !prop.Name.StartsWith("_")))
                             {
                                  var Value = prop.GetValue(this, null);
                                  var temp = Value + "";
                                  if (Value != null) Value = Value.ToString().Replace("~", "");
                                  Boolean boolValue;
                                  if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                                       Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                                  DateTime dateValue;
                                  bool IsDate = false;
                                  if (!temp.ToString().Contains("~")) //  having "~" in a value indicates that it should not be attemtped date parsing
                                       if (!Utilities.IsNumeric(Value) && DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                                       {
                                            Value = "DATE_FORMAT('" + Convert.ToDateTime(Value).ToString("yyyy-MM-dd HH:mm:ss") + "', '%Y:%m:%d %H:%i:%s')";
                                            IsDate = true;
                                       }     

                                  // string MaybeComma = i < this.GetType().GetProperties().Length - 1 ? "," : "";
                                  if (prop.Name.Contains("signature") && !(Value ?? "").ToString().Contains("data:image/png;base64,"))
                                       Value = "data:image/png;base64," + Value; 
                                  if (Value != null)
                                  {
                                       if (!IsDate)
                                       {
                                            if (Value.ToString().Contains("'")) Value = Value.ToString().Replace("'", @"\'");
                                            string Element = prop.Name + " = " + (Value == null ? "null" : "'" + Value + "'");
                                            //if (prop.Name != KeyField)
                                             Assignments.Add(Element);
                                       }
                                       else
                                       {
                                            string Element = prop.Name + " = " + (Value.ToString());
                                            //if (prop.Name != KeyField) 
                                            Assignments.Add(Element);
                                       }
                                  }
                                  
                             }
                             string Id = this.id; //this.GetType().GetProperty("id").GetValue(this, null).ToString ();
                             
                             if (Assignments.Count > 0)
                             {
                                  UpdateString += string.Join("," + System.Environment.NewLine, Assignments.ToArray());
                                  if (WhereColumns != null)
                                  {
                                        StringBuilder Comparisons = new StringBuilder();
                                        for (int i = 0; i <= WhereColumns.Length - 1; i++)
                                        {
                                             string MaybeAnd = i < WhereColumns.Length - 1 ? " and " : "";
                                             if (WhereValues==null)
                                                  Comparisons.Append(WhereColumns[i] + " = '" + this.GetType().GetProperties().First(prop => prop.Name == WhereColumns[i]).GetValue(this, null) + "'" + MaybeAnd + System.Environment.NewLine);
                                             else
                                                  Comparisons.Append(WhereColumns[i] + " = '" + WhereValues[i] + "'" + MaybeAnd + System.Environment.NewLine);
                                        }
                                        if (WhereColumns.Length > 0)
                                             UpdateString += " where " + Comparisons.ToString();
                                        else
                                             UpdateString += " where id ='" + this.id + "'";
                                  }
                                  else
                                        UpdateString += " where id ='" + this.id + "'";
                                  MySqlCommand cmd = new MySqlCommand(UpdateString, cn);
                                  int rows = cmd.ExecuteNonQuery();
                                  if (!this.GetType().Name.ToLower().StartsWith("json_")) Logger.LogAction(UpdateString);
                                  if (CloseConnection) cn.Close();
                                  return rows > 0;
                             }
                             else
                                  return true;
                      //  }

                }
            catch (Exception ex) { Logger.LogException(ex); return false; }
            }

        public Boolean Retrieve(string SearchField="id")
            {
            try
                {
                     string strConnect;
                     if (!this.GetType().Name.ToLower().StartsWith("json_"))
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
        public Boolean Retrieve(string[] SearchFields, string[] SearchValues)
        {
             try
             {
                  string strConnect;
                  StringBuilder Comparisons = new StringBuilder();

                  if (!this.GetType().Name.ToLower().StartsWith("json_"))
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
                       for (int i = 0; i <= SearchFields.Length - 1; i++)
                       {
                            Comparisons.Append(SearchFields[i] + " = '" + SearchValues[i] + "'" + (i < SearchValues.Length - 1 ? " and " : ""));
                       }
                       string SqlString = "select * from " + TableName + " where " + Comparisons.ToString();
                       // MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                       DataTable dt = DbConnect.GetDataTable(cn, SqlString);
                       //dt.Load(cmd.ExecuteReader());

                       if (dt.Rows.Count > 0)
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
            MySqlConnection cn = null;
            if (!this.GetType().Name.ToLower().StartsWith("json_"))
            {
                 strConnect = DbConnect.ConnectionString;
                 if (HttpContext.Current.Session["Connection"] != null)
                 {
                      cn = (MySqlConnection)HttpContext.Current.Session["Connection"];
                      if (cn.State != ConnectionState.Open) cn.Open();
                 }
            }
            else
            {
                 MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                 builder.Database = "IOS";
                 strConnect = builder.GetConnectionString(true);
            }
            bool CloseConnection=false;
               if (cn == null)
               {
                    cn = new MySqlConnection(strConnect);
                    CloseConnection = true;
               }
                if (cn.State != ConnectionState.Open) cn.Open();
                //using (MySqlConnection cn = new MySqlConnection(strConnect))
                //    {
                    //cn.Open();
                string SqlString = "select count(*) from " + TableName + " where id = '" + id + "'"; // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                if (CloseConnection) cn.Close();
                return rc > 0;
              //  }
            }

            catch (Exception ex) { Logger.LogException(ex); return false; }
            }
        public  Boolean Exists(string[] Fields, string[] Values)
            {
            try { 
           
            StringBuilder Comparisons = new StringBuilder();
            string strConnect;
            if (this.id + "" == "") this.id = Guid.NewGuid().ToString();
            if (!this.GetType().Name.ToLower().StartsWith("json_"))
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

        public bool InsertUpdateAction(int InsertUpdate = 0, string[] WhereColumns = null, string[] WhereValues = null)
            {
               bool rv=false;
               switch (InsertUpdate)
                {
                case 0:
                          if (WhereColumns != null && WhereValues != null)
                          {
                               if (Exists(WhereColumns, WhereValues))
                                    rv = Update(true, WhereColumns, WhereValues);
                               else
                                    rv = Insert();
                          }
                          else
                          {
                               if (Exists())
                                    rv = Update();
                               else
                                    rv = Insert();
                          }
                    break;
                case 1:
                    rv = Insert();
                    break;
                case 2:
                    rv = Update();
                    break;
                }
               return rv;
            }
        }
    }