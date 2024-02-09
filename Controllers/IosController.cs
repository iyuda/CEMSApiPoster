using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using WebApiPoster.Models;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Parse;
using System.Threading.Tasks;
using System.Text;
using System.Drawing;
using System.Web;
using System.IO;
using System.Windows.Forms;
using System.Transactions;
using System.Threading;
using System.Web.SessionState;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http.Cors;

namespace WebApiPoster.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class IosController : ApiController
    {
        [HttpGet]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public object resetPassRequest(string agency, string user)
        {
            HttpContext.Current.Session["LogDirectory"] = "resetPassRequest";
            try
            {
                agency = agency ?? "";
                user = user ?? "";

                string badge;
                string agency_id = Agency.GetAgencyIDByNumber(agency);
                if (String.IsNullOrEmpty(agency_id))
                {
                    return GetErrorJson("invalid agency");
                }
                string user_id = Users.GetIDByNameAndAgency(out badge, user, agency_id);
                if (String.IsNullOrEmpty(user_id))
                {
                    return GetErrorJson("invalid user");
                }
                string e_mail = Users.GetUserEmail(user_id, agency_id);
                if (!String.IsNullOrEmpty(e_mail))
                {
                    using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                    {
                        cn.Open();
                        string SqlString = "INSERT INTO `ios`.`pass_reset` (`token_id`, `created`, `user_id`)";
                        SqlString += "VALUES('{0}','{1}','{2}')";
                        string token = Guid.NewGuid().ToString();
                        SqlString = String.Format(SqlString, token, Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"), user_id);

                        MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                        Logger.LogAction(DbConnect.ProvisionConnectionString + System.Environment.NewLine + SqlString, "PasswordRecovery");
                        int rows = cmd.ExecuteNonQuery();
                        string ReturnJson;
                        if (rows == 0)
                        {
                            return GetErrorJson("no record was inserted");
                        }
                        ReturnJson = SendEmail(e_mail, token);
                        //string url = ConfigTools.GetConfigValue("RecoverPasswordUrl", "https://www.creativeems.com:85/users/RecoverPassword");
                        //url += "&token=" + token;
                        //string content;
                        //HttpResponseMessage response = UrlRequests.UrlRequest(url, out content, "GET", NoAuthorization: true);
                        Logger.LogAction(ReturnJson, "PasswordRecovery");
                        return JsonConvert.DeserializeObject(ReturnJson);
                    }
                }
                else
                {
                    return GetErrorJson("user has no e-mail");
                    //string ReturnJson = "{success: false,";
                    //ReturnJson += " \"reason\" : \"user has no e-mail\"}";
                    //return JsonConvert.DeserializeObject(ReturnJson);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return GetErrorJson("resetPassRequest: " + ex.Message);
            }

        }
        
        public static object GetErrorJson (string message)
        {
            string ReturnJson = "{success: false,";
            ReturnJson += " \"reason\" : \"" + message + "\"}";
            return JsonConvert.DeserializeObject(ReturnJson);
        }
        public static string SendEmail(string email, string token)
        {
            var fromAddress = new MailAddress("Fax@creativeems.com", "From Creative EMS");
            var toAddress = new MailAddress(email, "Password Reset");
            const string fromPassword = "support4732.";

            string subject = "Password Reset";
            string url = ConfigTools.GetConfigValue("RecoverPasswordUrl", "https://www.creativeems.com:85/users/RecoverPassword");
            string body = String.Format(url+"?token={0}", token);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            var message = new MailMessage(fromAddress, toAddress);
            message.Subject = subject;
            message.Body = body;
            try
            {
                smtp.Send(message);
                message.Dispose();
                Console.WriteLine("E-mail successfully sent.");
                string ReturnJson = "{success: true}";
                return ReturnJson;
            }

            catch (Exception ex)
            {
                File.AppendAllText(Directory.GetCurrentDirectory() + "FaxlogError.txt", DateTime.Now.ToString() + " Fax successfully sent. " + HttpContext.Current.Session["pcr_id"] + "\r\n");
                Console.WriteLine("Failure Sending E-mail.");
                string ReturnJson = "{success: false,";
                ReturnJson += " \"reason\" : \"" + ex.Message + "\"}";
                return ReturnJson;
            }

        }
        [HttpGet]
        public object resetPassToken(string token)
        {
            try
            {
                HttpContext.Current.Session["LogDirectory"] = "resetPassToken";
                string expired_tokens_ttl = ConfigTools.GetConfigValue("ExpiredTokensTTL", "120");
                int int_expired_tokens_ttl;
                int.TryParse(expired_tokens_ttl, out int_expired_tokens_ttl);
                
                using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                {
                    cn.Open();
                    string SqlString = "delete from `ios`.`pass_reset` where now() >  ADDDATE(if (ifnull(created, '') = '', '1970-01-01', created), interval {0} minute";
                    SqlString = String.Format(SqlString, int_expired_tokens_ttl);
                    //AND ADDDATE(if (ifnull('2018/12/31', '') = '', '2070-01-01', '2018/12/31'),  INTERVAL 1 DAY) 
                      
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    Logger.LogAction(DbConnect.ProvisionConnectionString + System.Environment.NewLine + SqlString, "PasswordRecovery");
                        
                    cmd.CommandText = "select count(*) from ios.pass_reset where token_id = '" + token + "'";
                    int rc = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
                    Logger.LogAction(DbConnect.ProvisionConnectionString + System.Environment.NewLine + cmd.CommandText, "PasswordRecovery");
                    string ReturnJson = "{success: false}";
                    if (rc == 0)
                    {
                        return JsonConvert.DeserializeObject(ReturnJson); ;
                        //return new HttpResponseMessage(HttpStatusCode.NotFound);
                    }
                    ReturnJson = "{success: true}";
                    return JsonConvert.DeserializeObject(ReturnJson); ;
                    //return  new HttpResponseMessage(HttpStatusCode.OK);
                        
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                string ReturnJson = "{success: false,";
                ReturnJson += " \"reason\" : \"" + ex.Message + "\"}";
                return JsonConvert.DeserializeObject(ReturnJson);
            }
        }
      
        public string DeleteToken(string token_id)
        {
            try
            {

                using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                {
                    cn.Open();
                    string SqlString = "delete from `ios`.`pass_reset` where token_id = '" +  token_id + "'";
                    
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    Logger.LogAction(DbConnect.ProvisionConnectionString + System.Environment.NewLine + SqlString, "PasswordRecovery");
                    int rows = cmd.ExecuteNonQuery();
                    string ReturnJson;
                    if (rows == 0)
                    {
                        ReturnJson = "{success: false}";
                        ReturnJson += " \"reason\" : \"no token was deleted\"}";
                        return ReturnJson;
                    }
                    ReturnJson = "{success: true}";
                    return ReturnJson;

                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                string ReturnJson = "{success: false,";
                ReturnJson += " \"reason\" : \"" + ex.Message + "\"}";
                return ReturnJson;
            }
        }
        [HttpPost]
        //[EnableCors(origins: "*", headers: "*", methods: "*")]
        public object resetPass(object JsonData) { 
            try
            {
                HttpContext.Current.Session["LogDirectory"] = "resetPass";

                string token = JsonMaker.GetIOSJsonExtract("$.token", JsonData);
                string pass = JsonMaker.GetIOSJsonExtract("$.pass", JsonData);
                using (MySqlConnection cn = new MySqlConnection(DbConnect.ProvisionConnectionString))
                {
                    cn.Open();

                    MySqlCommand cmd = new MySqlCommand("select * from ios.pass_reset where token_id = '" + token + "'", cn);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    
                    Logger.LogAction(DbConnect.ProvisionConnectionString + System.Environment.NewLine + cmd.CommandText, "PasswordRecovery");
                    string ReturnJson;
                    if (!dr.Read())
                    {
                        ReturnJson = "{success: false,";
                        ReturnJson += " \"reason\" : \"token not found\"}";
                        return JsonConvert.DeserializeObject(ReturnJson);
                    }
                    else
                    {
                        ReturnJson = Users.SetPassword(dr["user_id"].ToString(), pass);
                        if (ReturnJson.Contains("success: false"))
                        {
                            return JsonConvert.DeserializeObject(ReturnJson);
                        }
                        ReturnJson=DeleteToken(dr["token_id"].ToString());
                        if (ReturnJson.Contains("success: false"))
                        {
                            return JsonConvert.DeserializeObject(ReturnJson);
                        }
                    }
                    return JsonConvert.DeserializeObject("{success: true}");

                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                string ReturnJson = "{success: false,";
                ReturnJson += " \"reason\" : \"" + ex.Message + "\"}";
                return JsonConvert.DeserializeObject(ReturnJson);
            }
        }
    }
}
