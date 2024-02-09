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
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiPoster.Models;

namespace WebApiPoster
{
     public class Provisioning
     {
          public static object DeviceRegistered(string id, bool GetInvitationCode = false) //object WorkObject)
          {
               try
               {
                DataTable dt = new ExtendedDataTable();
                using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                {
                    cn.Open();
                    string Fields = "a.id, agency_number, agency_name, suspended";
                    //                        string Fields = "a.id, agency_number, agency_name, suspended,  (select concat('[',GROUP_CONCAT('{','id : ', '\"',id,'\"', ', ','name : ', '\"',name,'\"', '}'),']') from neighborhood group by agency_id having  agency_id=a.id) as neighborhoods";
                    if (GetInvitationCode)
                        Fields += ", left(d.id,8) as invitation_code";
                    string SqlString = "select " + Fields + " from ios.provisioning_devices d inner join ios.provisioning_agencies a on d.agency_id=a.id left outer join newcems.agency na on a.id=na.id where device_id = '" + id + "' limit 1"; // WorkObject.GetType().GetProperty("id").GetValue(WorkObject, null) + "'";
                    Logger.LogAction(DbConnect.ProvisionConnectionString + System.Environment.NewLine +  SqlString, "Provisioning");
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);

                    dt.Load(cmd.ExecuteReader());
                }
                string ReturnJson = "";
                foreach (DataRow row in dt.Rows)
                         {
                              if (ReturnJson.Length > 2)
                                   ReturnJson += ",";
                              ReturnJson += "{";

                              if (GetInvitationCode)
                              {
                                   ReturnJson += "registered: false,";
                                   ReturnJson += " \"reason\" : \"already registered to '" + row["invitation_code"].ToString() + "'\"";
                              }
                              else
                              {
                                   ReturnJson += "registered: true,";
                                   ReturnJson += "agency_id: \"" + row["id"].ToString() + "\",";
                                   ReturnJson += "agency_number: \"" + row["agency_number"].ToString() + "\",";
                                   ReturnJson += "agency_name: \"" + row["agency_name"].ToString() + "\",";
                                   ReturnJson += "suspended: " + row["suspended"].ToString().ToLower() + ",";
                                   List<string> NeighborhoodsList = getNeighborhoods(row["id"].ToString());
                                   string neighborhoods = "["+String.Join(",", NeighborhoodsList.ToArray()) + "]";   //dr["neighborhoods"].ToString();
                                   if (String.IsNullOrEmpty(neighborhoods)) // || dr["id"].ToString() != "90b16e1c-aa76-11e5-b94a-842b2b4bbc99")
                                        neighborhoods = "[]";
                                   
                                   ReturnJson += "neighborhoods: " + neighborhoods;
                              }
                              ReturnJson += "}";
                         }
                         if (ReturnJson.Length < 3)
                              if (GetInvitationCode)
                                   ReturnJson = "{registered: false}";
                              else
                              {
                                   ReturnJson = "{registered: false,";
                                   ReturnJson += " \"reason\" : \"invitation code not found\"}";
                              }
                         dynamic parsedJson = JsonConvert.DeserializeObject(ReturnJson);
                         return parsedJson;

                         //return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);

                         //JToken JsonObject = JObject.Parse(@ReturnJson);
                         //return JsonConvert.SerializeObject(JsonObject.ToString(Formatting.Indented));
                    
               }
               catch (Exception ex) { Logger.LogException(ex); return "{registered: \"false\"}"; }
          }
        public static List<string> getNeighborhoods(string agency_id) //object WorkObject)
        {
            string SqlString = "select CONCAT('{', 'id : ', '\"', id, '\"', ', ', 'name : ', '\"', name, '\"', '}') as entry from newcems.neighborhood where agency_id = '" + agency_id + "'";
            List<string> WorkList = new List<string>();
            using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
            {
                cn.Open();
                MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                MySqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    WorkList.Add(dr["entry"].ToString());
                }
                return WorkList;
            }
        }
        public static string getInvitationCode(string UUID) //object WorkObject)
          {
               try
               {

                    using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                    {
                         cn.Open();
                         string SqlString = "select left(id,8) as invitation_code from  ios.provisioning_devices where device_id = '" + UUID +"'";
                         MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                         int rows = cmd.ExecuteNonQuery();
                         return cmd.ExecuteScalar() + "";

                    }
               }
               catch (Exception ex) { Logger.LogException(ex); return "{registered: \"false\"}"; }
          }
          public static string getDeviceByInvitationCode(string InvitationCode) //object WorkObject)
          {
               try
               {

                    using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                    {
                         cn.Open();
                         string SqlString = "select device_id from  ios.provisioning_devices where left(id,8) = '" + InvitationCode + "'";
                         MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                         int rows = cmd.ExecuteNonQuery();
                         return cmd.ExecuteScalar() + "";

                    }
               }
               catch (Exception ex) { Logger.LogException(ex); return "{registered: false}"; }
          }

          public static object RegisterDevice(string invitationCode, string UUID) //object WorkObject)
          {
               try
               {
                    string Used_Device = getDeviceByInvitationCode(invitationCode);
                    if (!String.IsNullOrEmpty(Used_Device))
                    {
                         dynamic parsedJson = JsonConvert.DeserializeObject("{\"registered\": false, \"reason\" : \"this invitation code is already assigned to device '" + Used_Device + "'\"}");
                         return parsedJson;
                    }

                    object IsAlreadyRegistered = DeviceRegistered(UUID, GetInvitationCode: true);
                    if (IsAlreadyRegistered.ToString().Contains("already registered"))
                         return (IsAlreadyRegistered);
                    
                   
                    using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                    {
                         cn.Open();
                         string SqlString = "update  ios.provisioning_devices set device_id= '" + UUID + "', last_verification = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "' where left(id,8)= '" + invitationCode + "' and ifnull(device_id, '') =''";
                         MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                         int rows = cmd.ExecuteNonQuery();
                         return DeviceRegistered(UUID);

                    }
               }
               catch (Exception ex) { Logger.LogException(ex); return "{registered: false}"; }
          }
        public static object autoRegister(string agency, string UUID)
        {
            try
            {
                string agency_id = Agency.GetAgencyIDByNumber(agency);
                //string Used_Device = getDeviceByInvitationCode(invitationCode);
                //if (!String.IsNullOrEmpty(Used_Device))
                //{
                //    dynamic parsedJson = JsonConvert.DeserializeObject("{\"registered\": false, \"reason\" : \"this invitation code is already assigned to device '" + Used_Device + "'\"}");
                //    return parsedJson;
                //}

                object IsAlreadyRegistered = DeviceRegistered(UUID, GetInvitationCode: true);
                if (IsAlreadyRegistered.ToString().Contains("already registered"))
                    return (IsAlreadyRegistered);


                using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                {
                    cn.Open();
                    string SqlString = "update  ios.provisioning_devices set device_id= '" + UUID + "', last_verification = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "' where ifnull(device_id, '') ='' and agency_id = '" + agency_id + "' LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    Logger.LogAction(DbConnect.ProvisionConnectionString + System.Environment.NewLine +  SqlString, "Provisioning");
                    int rows = cmd.ExecuteNonQuery();
                    if (rows==0)
                    {
                        string ReturnJson = "{registered: false,";
                        ReturnJson += " \"reason\" : \"no invitation code for this agency\"}";
                        return JsonConvert.DeserializeObject(ReturnJson);
                    }
                    return DeviceRegistered(UUID);
                }
            }
            catch (Exception ex) {
                Logger.LogException(ex);
                string ReturnJson = "{registered: false,";
                ReturnJson += " \"reason\" : \"" + ex.Message + "\"}";
                return JsonConvert.DeserializeObject(ReturnJson);
            }
        }
        public static object UnregisterDevice(string UUID)
          {
               try
               {
                    string InvitationCode = getInvitationCode(UUID);
                    using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                    {
                         cn.Open();
                         string SqlString = "update ios.provisioning_devices set device_id = null, last_verification = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss") + "' where device_id ='" + UUID + "'";
                         MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                         int rows = cmd.ExecuteNonQuery();
                         string ReturnJson = DeviceRegistered(UUID).ToString();

                         if (!ReturnJson.Contains("agency_id}"))
                              ReturnJson = "{registered: \"false\", \"invitationCodeAvailable\" : \"" + InvitationCode + "\"}";
                         return JsonConvert.DeserializeObject(ReturnJson);

                    }
               }
               catch (Exception ex) { Logger.LogException(ex); return "{registered: \"false\"}"; }
          }
     }
}