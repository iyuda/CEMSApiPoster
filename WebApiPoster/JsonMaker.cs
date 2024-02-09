using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WebApiPoster.PCR;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;
namespace WebApiPoster
    {
    public static class JsonMaker
        {
         public static List<T> GetListFromJSON<T>(string jsonString) where T : class, new()
         {
              if (jsonString == null) return null;
              JavaScriptSerializer jss = new JavaScriptSerializer();
              List<T> list = jss.Deserialize<List<T>>(jsonString);
              return list;
         }
        public static string GetJSONFromList<T>(List<T> ListParam, string Prefix = "")
            {
            try
                {
                return Prefix ==""?"": "\"" + Prefix + "\": " + JsonConvert.SerializeObject(ListParam);
                }
            catch (Exception ex)
                {
                return Logger.LogException(ex);
                }
            }
        public static string GetJSON(dynamic obj, Boolean SkipTableName = false, string Prefix = "")
            {
            try
                {

              
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                JsonSerializerSettings Jsettings = new JsonSerializerSettings();
                if (!SkipTableName)
                    {
                    if (Prefix == "") Prefix = obj.GetTableName(); else Prefix += obj.GetTableName();
                    json = "\"" + Prefix + "\": " + json;
                    }
                return json;
                }
            catch (Exception ex)
                {
                return Logger.LogException(ex);
                }
            }

        public static object  GetIOSJsonExtract(string Expression, string Input = "data")
        {
             object value;
             try
             {
                  string JsonString = Input.Replace("\"", "'").Replace(".", "@");
                  if (Expression.Contains("@"))
                  {
                       string[] SplitArray = Expression.Split('@');
                       Expression += "." + SplitArray[SplitArray.Length - 1];
                  }


                  JObject JsonObject = JObject.Parse(@JsonString);
                  IEnumerable<JToken> Selection = (JToken)null;
                  if (!Expression.Contains("*"))
                    Selection = JsonObject.SelectToken(Expression);
                  else
                    Selection = JsonObject.SelectTokens(Expression);
                  value = Selection == null ? null : Selection;
                  //var values = Selection.Select(o => o.First);
                  //foreach (JToken token in Selection)
                  //{
                  //     Console.WriteLine(token.Path + ": " + token);
                  //}
             }
                  
             catch (Exception ex) { Logger.LogException(ex); return null; }
             return value;
             //string value;
             //try
             //{
             //     MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
             //     builder.Database = "IOS";
             //     using (MySqlConnection cn = new MySqlConnection(builder.GetConnectionString(true)))
             //     {
             //          cn.Open();
             //          //string QueryString = "select json_extract(data," + (Expression.Contains("'") ? Expression : "'" + Expression + "'") + ") FROM json_pcr j where id='5db404f9-d2eb-11e6-b953-3052cb650342'";
             //          string QueryString = "select json_extract('" + Input + "', " + (Expression.Contains("'") ? Expression : "'" + Expression + "'") + ") FROM json_pcr j where id='5db404f9-d2eb-11e6-b953-3052cb650342'";
             //          MySqlCommand cmd = new MySqlCommand(QueryString, cn);
             //          value = cmd.ExecuteScalar().ToString();
             //     }
             //}
             //catch (Exception ex) { Logger.LogException(ex); return null; }
             //return value;

        }
        public static bool ModifyArrayItem(string Path, string SearchField, string SearchValue, String ModifyField, string ModifyValue)
        {
             object JsonData = Pcr.OutgoingJson;
             string JsonString = JsonData.ToString().Replace("\"", "'").Replace(".", "@");
             JToken JsonToken = JObject.Parse(@JsonString);
             JArray jarray = (JArray)JsonToken.SelectToken(Path);
             if (jarray == null) return false;
             JToken SearchToken = jarray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString() == SearchValue);
             if (SearchToken == null) return false;
             SearchToken.SelectToken(ModifyField).Replace(ModifyValue);
             Pcr.OutgoingJson = (JObject)JsonToken; 
             return true;
        }
        public static bool UpdateJsonValue(string Path, string Value, bool MustExist=false)
        {
             object JsonData = Pcr.OutgoingJson;
             try
             {
                  string JsonString = JsonData.ToString().Replace("\"", "'"); //.Replace(".", "@");
                  if (Path.Contains("@"))
                  {
                       string[] SplitArray = Path.Split('@');
                       Path += "." + SplitArray[SplitArray.Length - 1];
                       Path = "$~"+ "['" + Path.Replace("$.", "").Replace(".", "']~['").Replace("@", ".") + "']";
                  }


                  //  JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JsonData.ToString());
                  JToken JsonToken = JObject.Parse(@JsonString);
                  JToken ModifyToken = JsonToken;
                  string RemainingPath = Path;
                  foreach (string item in Path.Split('~'))
                  {
                       JToken Selection = (JToken)ModifyToken.SelectToken(item);
                       if (Selection != null)
                            ModifyToken = Selection;
                       else if (item != "$")
                       {
                            if (MustExist) return false;
                            string last_loop_item = "";
                            JToken PrevJsonToken = ModifyToken;
                            foreach (string loopitem in RemainingPath.Split('.'))
                            {
                                 JObject JsonObject = new JObject();
                                 ModifyToken[loopitem] = JsonObject;
                                 PrevJsonToken = ModifyToken;
                                 ModifyToken = JsonObject;
                                 last_loop_item = loopitem;
                            }
                            //if (last_loop_item != "") PrevJsonToken[last_loop_item] = Value;
                            //JsonData = PrevJsonToken.Root;
                            //break;
                         
                            //JsonObject.AddAfterSelf(@"{" + RemainingPath.Replace(".", "}:{") + "="+ Value+ "}");
                       }
                       RemainingPath = string.Join(".",RemainingPath.Split('.').Skip(1).ToArray());
                  }
                  Path = Path.Replace("~", ".");
                  JsonToken.SelectToken(Path).Replace(Value);
                  JsonData = (JObject)JsonToken;
                  Pcr.OutgoingJson = JsonData;
                  return true;
                  //JsonObject[Path] = Value;
                 
                  
             }
             catch (Exception ex) { Logger.LogException(ex); return false; }

        }
       
        public static string GetIOSJsonExtract(string Expression, object JsonData)
        {

             string value;
             try
             {
                  string JsonString = JsonData.ToString().Replace("\"", "'").Replace(".", "@");
                  if (Expression.Contains("@"))
                  {
                       string[] SplitArray = Expression.Split('@');
                       Expression += "." + SplitArray[SplitArray.Length - 1];
                  }


                  //  JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JsonData.ToString());
                  JObject JsonObject = JObject.Parse(@JsonString);
                  JToken Selection = JsonObject.SelectToken(Expression);
                  value = Selection == null || Selection.Type == JTokenType.Null ? null : Selection.ToString();
                  //MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(DbConnect.ConnectionString);
                  //builder.Database = "IOS";
                  //using (MySqlConnection cn = new MySqlConnection(builder.GetConnectionString(true)))
                  //{
                  //     cn.Open();
                       
                  //     string QueryString = "select json_extract(" + ((JsonString=="data" || JsonString.Contains("'")) ?JsonString: "'" + JsonString + "'") + ", " + ((Expression.Contains("'") ? Expression : "'" + Expression + "'")+")") ;
                  //     MySqlCommand cmd = new MySqlCommand(QueryString, cn);
                  //     value = cmd.ExecuteScalar().ToString();
                  //}
             }
             catch (Exception ex) { Logger.LogException(ex); return null; }
             return value;

        }
        public static string GetJSON(string table_name, string Id, string SearchField = "id")
            {
            try
                {
                ExtendedDataTable dt = new ExtendedDataTable();
                using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                    {
                    cn.Open();

                    string SqlString = "select * from " + table_name + " where " + SearchField + " = '" + Id + "'";
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);

                    dt.Load(cmd.ExecuteReader());
                    }


                List<object> myList = dt.toList(); // new List<object>();

                string json = "\"" + table_name + "\": " + JsonConvert.SerializeObject(myList);
                return json;
                }
            catch (Exception ex)
                {
                return Logger.LogException(ex);
                }
            }

    
        }
    }