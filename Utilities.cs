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
using System.Drawing;
using System.IO;
namespace WebApiPoster.PCR
    {
    public static class Utilities
        {
         public static string BinaryToString(string data)
         {
              List<Byte> byteList = new List<Byte>();

              for (int i = 0; i < data.Length; i += 8)
              {
                   byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
              }
              return Encoding.ASCII.GetString(byteList.ToArray());
         }

         public static string GetIOSJson(string PcrID)
         {
              MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
              builder.Database = "IOS";
              using (MySqlConnection cn = new MySqlConnection(builder.GetConnectionString(true)))
              {
                   cn.Open();
                   StringBuilder SqlString = new StringBuilder();
                   SqlString.Append("Select data from json_pcr where id = '" + PcrID + "'");
                   MySqlCommand cmd = new MySqlCommand(SqlString.ToString(), cn);
                   string rv = (string)cmd.ExecuteScalar();
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }

         public static string OutputIOSJson(string JsonString, string PcrID)
         {
              MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
              builder.Database = "IOS";
              using (MySqlConnection cn = new MySqlConnection(builder.GetConnectionString(true)))
              {
                   cn.Open();
                   StringBuilder SqlString = new StringBuilder();
                   SqlString.Append("insert into  json_pcr (id, data) values ('" + PcrID + "', '" + JsonString + "')");
                   MySqlCommand cmd = new MySqlCommand(SqlString.ToString(), cn);
                   string rv = (string)cmd.ExecuteScalar();
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }

         public static string GetLSButtonID(string LS_Type, string name, string form_800_id, string agency_id)
         {


              using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
              {
                   cn.Open();
                   StringBuilder SqlString = new StringBuilder();
                   //SqlString.Append("Select ab.id from (select form_800_id, all_buttons from form_800_buttons union select form_800_id, all_buttons_id from form_800_inputs) fb");
                   //SqlString.Append(" inner join form_800 f on fb.form_800_id=f.id");
                   //SqlString.Append(" inner join all_buttons ab on fb.all_buttons=ab.id");
                   //SqlString.Append(" inner join dynamic_button_types d on ab.dynamic_button_id=d.id");
                   //SqlString.Append(" inner join user_login ul on ul.form800_id=f.id");
                   //SqlString.Append(" inner join run_logins rl on rl.user_login=ul.id");
                   //SqlString.Append(" inner join ems_run er on er.id=rl.ems_run");
                   //SqlString.Append(" inner join pcr on pcr.ems_run =er.id");
                   //SqlString.Append(" where ab.name = '" + name + "' and d.type_name ='800 " + LS_Type + "'");
                   SqlString.Append("Select ab.id, ");
                   SqlString.Append(" case when ab.agency_id ='" + agency_id + "' then 1 else 2 end as SortField");
                   SqlString.Append(" from all_buttons ab ");
                   SqlString.Append(" inner join dynamic_button_types d on ab.dynamic_button_id=d.id");
                   SqlString.Append(" inner join user_login ul on ul.form800_id='" + form_800_id + "'");
                   SqlString.Append(" inner join run_logins rl on rl.user_login=ul.id");
                   SqlString.Append(" inner join ems_run er on er.id=rl.ems_run");
                   SqlString.Append(" inner join pcr on pcr.ems_run =er.id");
                   SqlString.Append(" where ab.name = '" + name + "' and d.type_name ='800 " + LS_Type + "' and ab.agency_id in ('" + agency_id + "', '802fa7c4-9abc-11e1-bbb3-842b2b4bbc99')");
                   //SqlString.Insert(0, "select * from (");
                   SqlString.Append(" order by SortField");
                   SqlString.Append(" limit 1");
                   MySqlCommand cmd = new MySqlCommand(SqlString.ToString(), cn);
                   string rv = (string)cmd.ExecuteScalar();
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }
//         System.IO.MemoryStream ms = New System.IO.MemoryStream();
//Bitmap signatureImage  = New Bitmap(800, 800);

//signatureImage  = SignObj.SigJsonToImage(signatureJson);
//signatureImage .Save(ms, Imaging.ImageFormat.Bmp);
//signatureImage .Save("FilePath/" + "image.png");
         //public static Bitmap SigJsonToImage(string json, Size size)
         //{
         //     var signatureImage = GetBlankCanvas();
         //     if (!string.IsNullOrWhiteSpace(json))
         //     {
         //          using (var signatureGraphic = Graphics.FromImage(signatureImage))
         //          {
         //               signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
         //               var pen = new Pen(PenColor, PenWidth);
         //               var serializer = new JavaScriptSerializer();
         //               // Next line may throw System.ArgumentException if the string
         //               // is an invalid json primitive for the SignatureLine structure
         //               var lines = serializer.Deserialize<List<SignatureLine>>(json);
         //               foreach (var line in lines)
         //               {
         //                    signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
         //               }
         //          }
         //     }
         //     return (Bitmap)((size.Width == CanvasWidth && size.Height == CanvasHeight) ? signatureImage : ResizeImage(signatureImage, size));
         //}
         //public static Bitmap SigJsonToImage(string json, Size size)
         //{
         //     var signatureImage = GetBlankCanvas();
         //     if (!string.IsNullOrWhiteSpace(json))
         //     {
         //          using (var signatureGraphic = Graphics.FromImage(signatureImage))
         //          {
         //               signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
         //               var pen = new Pen(PenColor, PenWidth);
         //               var serializer = new JavaScriptSerializer();
         //               // Next line may throw System.ArgumentException if the string
         //               // is an invalid json primitive for the SignatureLine structure
         //               var lines = serializer.Deserialize<List<SignatureLine>>(json);
         //               foreach (var line in lines)
         //               {
         //                    signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
         //               }
         //          }
         //     }
         //     return (Bitmap)((size.Width == CanvasWidth && size.Height == CanvasHeight) ? signatureImage : ResizeImage(signatureImage, size));
         //}
 //        private String getStringFromBitmap(Bitmap bitmapPicture) {
 ///*
 //* This functions converts Bitmap picture to a string which can be
 //* JSONified.
 //* */
 //final int COMPRESSION_QUALITY = 100;
 //String encodedImage;
 //ByteArrayOutputStream byteArrayBitmapStream = new ByteArrayOutputStream();
 //bitmapPicture.compress(Bitmap.CompressFormat.PNG, COMPRESSION_QUALITY,
 //byteArrayBitmapStream);
 //byte[] b = byteArrayBitmapStream.toByteArray();
 //encodedImage = Base64.encodeToString(b, Base64.DEFAULT);
 //return encodedImage;
 //}
         public static string DecodeBase64(this System.Text.Encoding encoding, string encodedText)
         {
              if (encodedText == null)
              {
                   return null;
              }

              byte[] textAsBytes = System.Convert.FromBase64String(encodedText);
              return encoding.GetString(textAsBytes);
         }
         public static string ImageToBase64(Image image,  System.Drawing.Imaging.ImageFormat format)
         {
              using (MemoryStream ms = new MemoryStream())
              {
                   // Convert Image to byte[]
                   image.Save(ms, format);
                   byte[] imageBytes = ms.ToArray();

                   // Convert byte[] to Base64 String
                   string base64String = Convert.ToBase64String(imageBytes);
                   return base64String;
              }
         }
         public static Image Base64ToImage(string base64String)
         {
              // Convert Base64 String to byte[]
              byte[] imageBytes = Convert.FromBase64String(base64String);
              MemoryStream ms = new MemoryStream(imageBytes, 0,
                imageBytes.Length);

              // Convert byte[] to Image
              ms.Write(imageBytes, 0, imageBytes.Length);
              Image image = Image.FromStream(ms, true);
              return image;
         }
         public static string GetForm800ButtonID(string LS_Type, string name)
         {


              using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
              {
                   cn.Open();
                   StringBuilder SqlString = new StringBuilder();
                   SqlString.Append("Select distinct f.id from (select form_800_id, all_buttons from form_800_buttons union select form_800_id, all_buttons_id from form_800_inputs) fb");
                   SqlString.Append(" inner join form_800 f on fb.form_800_id=f.id");
                   SqlString.Append(" inner join all_buttons ab on fb.all_buttons=ab.id");
                   SqlString.Append(" inner join dynamic_button_types d on ab.dynamic_button_id=d.id");
                   SqlString.Append(" inner join user_login ul on ul.form800_id=f.id");
                   SqlString.Append(" inner join run_logins rl on rl.user_login=ul.id");
                   SqlString.Append(" inner join ems_run er on er.id=rl.ems_run");
                   SqlString.Append(" inner join pcr on pcr.ems_run =er.id");
                   SqlString.Append(" where ab.name = '" + name + "' and d.type_name ='800 " + LS_Type + "'");
                   SqlString.Append(" limit 1");

                   MySqlCommand cmd = new MySqlCommand(SqlString.ToString(), cn);
                   string rv = (string)cmd.ExecuteScalar();
                   //rv = rv == null ? "" : rv.ToString();
                   return rv;
              }
         }
        public static Boolean Insert(string TableName, string[] Columns, string[] Values)
            {
            try
                {
                string strConnect = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
                using (MySqlConnection cn = new MySqlConnection(strConnect))
                    {
                    cn.Open();

                    string InsertString = "insert into " + TableName + "(" + string.Join(",", Columns).ToString() + ") values ('" + string.Join("','", Values).ToString() + "')";
                    MySqlCommand cmd = new MySqlCommand(InsertString, cn);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                    }

                }
            catch (Exception ex) { Logger.LogException(ex); return false; }
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
            catch (Exception ex) { Logger.LogException(ex); return false; }
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
            catch (Exception ex) { Logger.LogException(ex); return false; }
            }
         public static string  ValidateTimeField(string value) {
              
              string rv;
              if (IsNumeric(value) && value.Length>=4)
              {
                   value = value.Insert(2, ":");
              }
              DateTime dateValue;
              if (!DateTime.TryParse(value == null ? "" : value.ToString(), out dateValue))  rv = null;  else rv = value;
              return rv;
         }

        public static List<T> GetClassList<T>(string TableName, string Id="", string SearchField = "") where T : class, new()
            {
            try
                {
                DataTable table=GetDataTable(TableName, Id, SearchField);
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                    {
                    T obj = (T) Activator.CreateInstance(typeof(T));

                    foreach (var prop in obj.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.")))
                        {
                        try
                            {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name.StartsWith("_") ? prop.Name.Remove(0, 1) : prop.Name], propertyInfo.PropertyType), null);
                            }
                        catch
                            {
                            continue;
                            }
                        }

                    list.Add(obj);
                    }

                return list;
                }
            catch (Exception ex)
                {
                Logger.LogException(ex);
                return null;
                }
            }
        public static Boolean UseRequiredFields { get; set; }
        public static DataTable GetDataTable(string TableName, string Id, string SearchField = "")
            {
            try
                {

                using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                    {
                    cn.Open();

                    string SqlString = "select * from " + TableName + " t";
                    if (SearchField != "") SqlString += " where " + SearchField + " = '" + Id + "'";
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    DataTable dt = new ExtendedDataTable();
                    dt.Load(cmd.ExecuteReader());
                    return dt;
                    }
                }
            catch (Exception ex) { Logger.LogException(ex); return null; }
            }
        public static Boolean Exists(string TableName, string id) //object WorkObject)
            {
           
            using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
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
           
            StringBuilder Comparisons = new StringBuilder();
            using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
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
        public static string ConnectionString {get; set;}
        public static void ValidateField(string TableName, string id)
            {
            if (!Exists(TableName, id))
                Insert(TableName, new string[] { "id" }, new string[] {  id });
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