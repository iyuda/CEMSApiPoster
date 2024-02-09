using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;

namespace WebApiPoster.Models
    {
    public class Disposition_Facility
        {
        public Disposition_Facility()
            {

            }
        public Disposition_Facility(string id)
            {
            this.id = id;
            }
        public string id { get; set; }
        public string business_id { get; set; }
        public string disposition_code { get; set; }
        public string city_code { get; set; }
        public string npi { get; set; }
        public string disposition_type_id { get; set; }
        public string active { get; set; }
        public string agency_id { get; set; }



        public void MakeSureItExists()
            {
            if (!Exists())
                Insert();
            }
        public Boolean Insert()
            {
            try
                {
                this.id = Guid.NewGuid().ToString();
                this.active = "0";
                string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();

                    StringBuilder ColumnList = new StringBuilder();
                    StringBuilder ValueList = new StringBuilder();
                    for (int i = 0; i <= this.GetType().GetProperties().Length - 1; i++) //DataColumn col in SourceRow.Table.Columns)
                        {
                        ColumnList.Append(this.GetType().GetProperties()[i].Name);

                        var Value = this.GetType().GetProperties()[i].GetValue(this, null);
                        Boolean boolValue;
                        if (Boolean.TryParse(Value == null ? "" : Value.ToString(), out boolValue))
                            Value = Convert.ToInt16(Convert.ToBoolean(Value)).ToString();

                        DateTime dateValue;
                        if (DateTime.TryParse(Value == null ? "" : Value.ToString(), out dateValue))
                            Value = Convert.ToDateTime(Value).ToString("yyyy-MM-dd hh:mm:ss");

                        ValueList.Append(Value == null ? "null" : "'" + Value + "'");
                        if (i < this.GetType().GetProperties().Length - 1)
                            {
                            ColumnList.Append(",");
                            ValueList.Append(",");
                            }
                        }
                    string InsertString = "insert into disposition_facility (" + ColumnList.ToString() + ") values (" + ValueList.ToString() + ")";
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    }

                }
            catch (Exception ex) { ErrorLog.LogException(ex); return false; }
            }
        public Boolean Exists()
            {
            return this.id != null;
            //string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
            //using (MySqlConnection cn = new MySqlConnection(strSQL))
            //    {
            //    cn.Open();
            //    string SqlString = "select count(*) from business where name = '" + this.name + "'";
            //    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
            //    int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
            //    return rc > 0;
            //    }
            }

        
        }
    }