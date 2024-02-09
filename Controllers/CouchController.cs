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

namespace WebApiPoster.Controllers
{
   
    public class CouchController : ApiController
    {
         [HttpGet]
         public string EmptyAgencyConfig(string agency_id)
         {
              new Thread(() =>
              {
                   Thread.CurrentThread.IsBackground = true;
              }).Start();

              return "{ok:true}";

         }
         [HttpGet]
         public IHttpActionResult UpdateAgencyConfig_Async(string agency_id, string neighborhood_id = "")
         {

                   //List<string> AgencyConfig_ParameterIDs = (List<string>)HttpContext.Current.Application["Agency IDs"];
                   //if (AgencyConfig_ParameterIDs == null)
                   //     AgencyConfig_ParameterIDs = new List<string>();
                   HttpContext.Current.Session["LogDirectory"] = "UpdateAgencyConfig";
                   Utilities.LogsDirectory = "UpdateAgencyConfig";
                   UrlRequests.AgencyConfig_ParameterIDs.Add(agency_id + "@" + neighborhood_id);
                  // HttpContext.Current.Application["Agency IDs"] = AgencyConfig_ParameterIDs;
                   //HttpContext ctx = HttpContext.Current;

                   //new Thread(() =>
                   //{
                   //     Thread.CurrentThread.IsBackground = true;
                   //     HttpContext.Current = ctx;
                   //     UrlRequests.UpdateAgencyConfig();
                   //     //Console.WriteLine("Hello, world");
                   //}).Start();
                   return Ok(); //"{\"ok\":true}";

         }

        [HttpGet]
        public HttpResponseMessage UpdateAgencyConfig(string agency_id, string neighborhood_id = "", string fax = "")
        {

            HttpContext.Current.Session["LogDirectory"] = "UpdateAgencyConfig";
            Utilities.LogsDirectory = "UpdateAgencyConfig";
            try
            {
                Guid.Parse(agency_id);
            }
            catch (FormatException)
            {
                agency_id = Agency.GetAgencyIDByNumber(agency_id);
            }
            if (!String.IsNullOrEmpty(agency_id))
                return UrlRequests.UpdateAgencyConfig(agency_id, neighborhood_id, fax);
            else
                return new HttpResponseMessage(HttpStatusCode.NotFound);
        }
        
        [HttpPost]
        public HttpResponseMessage UpdateAgencyConfig([FromBody] string IniFileContent, string agency_id, string neighborhood_id = "", string fax = "")
         {

            HttpContext.Current.Session["LogDirectory"] = "UpdateAgencyConfig";
            Utilities.LogsDirectory = "UpdateAgencyConfig";
            HttpContext.Current.Session["IniFileContent"] = IniFileContent;
            try
            {
                Guid.Parse(agency_id);
            }
            catch (FormatException)
            {
                agency_id = Agency.GetAgencyIDByNumber(agency_id);
            }
            if (!String.IsNullOrEmpty(agency_id))
                return UrlRequests.UpdateAgencyConfig(agency_id, neighborhood_id, fax);
            else
                return new HttpResponseMessage(HttpStatusCode.NotFound);
        }  
         //[HttpGet]
         //public HttpResponseMessage PCR2COUCH(string pcr_id)
         //{
         //     try
         //     {

         //          HttpContext.Current.Session["LogDirectory"] = "PCR2COUCH";
         //          Utilities.LogsDirectory = "PCR2COUCH";
         //          return UrlRequests.PCR2COUCH(pcr_id);
         //          //return "{\"ok\":true}";

         //     }
         //     catch (Exception ex) { Logger.LogException(ex); throw ex; }
         //}
         [HttpGet]
         public HttpResponseMessage getMessages(string agency_id, string user_id)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "getMessages";
                   Utilities.LogsDirectory = "getMessages";
                   return UrlRequests.GetMessages(agency_id, user_id);
                   //return "{\"ok\":true}";

              }
              catch (Exception ex) { Logger.LogException(ex); throw ex; }
         }
         [HttpGet]
         public HttpResponseMessage clearOutbox(string user_id, string agency_id)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "clearOutbox";
                   Utilities.LogsDirectory = "clearOutbox";
                   return UrlRequests.ClearOutbox(agency_id, user_id);
                   //return "{\"ok\":true}";

              }
              catch (Exception ex) { Logger.LogException(ex); throw ex; }
         }
         [HttpGet]
         public string PCR2COUCH_Async(string pcr_id)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "PCR2COUCH";
                   Utilities.LogsDirectory = "PCR2COUCH";
                   UrlRequests.Pcr_ParameterIDs.Add(pcr_id);
                   return "{\"ok\":true}";

              }
              catch (Exception ex) { Logger.LogException(ex); return "false"; }
         }
        
        
         [HttpPost]
         public HttpResponseMessage ExpectedPcr2Couch([FromBody] object JsonData)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "ExpectedPcr2Couch";
                   Utilities.LogsDirectory = "ExpectedPcr2Couch";
                   return UrlRequests.ExpectedPcr2Couch(JsonData);

              }
              catch (Exception ex) { Logger.LogException(ex); throw ex; }
         }
         
         //         DEPRICATED
         [HttpGet]
         public HttpResponseMessage ImportPcr(string id)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "ImportPcr";
                   Utilities.LogsDirectory = "ImportPcr";
                   return UrlRequests.ImportPcr(id);

              }
              catch (Exception ex) { Logger.LogException(ex); throw ex; }
         }
        
         [HttpGet]
         public object IsRegistered(string UUID)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "IsRegistered";
                   Utilities.LogsDirectory = "IsRegistered";
                   return Provisioning.DeviceRegistered(UUID);

              }
              catch (Exception ex) { Logger.LogException(ex); return ex.Message; }
         }
         [HttpGet]
         public object IsRegistered2(string UUID)
         {
              try
              {
                   if (string.IsNullOrEmpty(UUID))
                   {
                        //HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        //message.Content = new StringContent("UUID is empty.");
                        //throw new HttpResponseException(message);
                        HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        Response.ReasonPhrase = HttpStatusCode.BadRequest + ":  UUID is empty";
                        throw new HttpResponseException(Response);
                       //throw new HttpResponseException(HttpStatusCode.BadRequest);
                   }
                   HttpContext.Current.Session["LogDirectory"] = "IsRegistered";
                   Utilities.LogsDirectory = "IsRegistered";
                   return Provisioning.DeviceRegistered(UUID);

              }
              catch (HttpResponseException ex) { Logger.LogException(ex); return ex.Response.ToString(); }
              catch (Exception ex) { Logger.LogException(ex); return ex.Message; }
         }
         [HttpGet]
         public object RegisterDevice(string invitationCode, string UUID)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "RegisterDevice";
                   Utilities.LogsDirectory = "RegisterDevice";
                  // invitationCode.Split('.')[
                   return Provisioning.RegisterDevice(invitationCode.Remove(0, invitationCode.IndexOf(".")+1), UUID);

              }
              catch (Exception ex) { Logger.LogException(ex); return "false"; }
         }
        [HttpGet]
        public object autoRegister(string agency, string UUID)
        {
            try
            {

                HttpContext.Current.Session["LogDirectory"] = "autoRegister";
                Utilities.LogsDirectory = "RegisterDevice";
                // invitationCode.Split('.')[
                return Provisioning.autoRegister(agency, UUID);

            }
            catch (Exception ex) { Logger.LogException(ex); return "false"; }
        }
        [HttpGet]
         public object SetupForms(string agency_id)
           {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "SetupForms";
                   Utilities.LogsDirectory = "SetupForms";
                   return UrlRequests.SetupForms(agency_id);

              }
              catch (Exception ex) { Logger.LogException(ex); return "false"; }
         }
         [HttpGet]
         public object SetupAllAgencies()
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "SetupAllAgencies";
                   Utilities.LogsDirectory = "SetupAllAgencies";
                   return UrlRequests.SetupAllAgencies();

              }
              catch (Exception ex) { Logger.LogException(ex); return "false"; }
         }
       
        [HttpGet]
        public object SetupAllConfigs()
        {
            try
            {

                HttpContext.Current.Session["LogDirectory"] = "SetupAllConfigs";
                Utilities.LogsDirectory = "SetupAllConfigs";
                return UrlRequests.SetupAllConfigs();

            }
            catch (Exception ex) { Logger.LogException(ex); return "false"; }
        }
        [HttpPut]
         public object SetupAgencies([FromBody] object JsonData)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "SetupAgencies";
                   Utilities.LogsDirectory = "SetupAgencies";
                   return UrlRequests.SetupAgencies(JsonData);

              }
              catch (Exception ex) { Logger.LogException(ex); return "false"; }
         }
         [HttpPut]
         public IHttpActionResult SetupAgencies_Async([FromBody] object JsonData)
         {
               HttpContext.Current.Session["LogDirectory"] = "SetupAgencies";
               Utilities.LogsDirectory = "SetupAgencies";
               UrlRequests.SetupAgencies_ParameterIDs.Add(JsonData);
               return Ok();
         }

         [HttpGet]
         public object UnregisterDevice(string UUID)
         {
              try
              {

                   HttpContext.Current.Session["LogDirectory"] = "RegisterDevice";
                   Utilities.LogsDirectory = "RegisterDevice";
                   return Provisioning.UnregisterDevice(UUID);

              }
              catch (Exception ex) { Logger.LogException(ex); return "false"; }
         }
         [HttpGet]
         public object GetAgencyConfig(string Agency)
         {
              try
              {
                   string ConfigFile = ConfigurationManager.ConnectionStrings["AgencyConfigPath"].ToString() + "\\" + Agency + "\\agencysettings.ini";
                   if (!File.Exists(ConfigFile))
                   {
                        HttpResponseMessage Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
                        Response.ReasonPhrase = HttpStatusCode.BadRequest + ":  File " + ConfigFile + " does not exist";
                        throw new HttpResponseException(Response);

                   }

                   //string config = File.ReadAllText(ConfigFile);
                   HttpContext.Current.Session["LogDirectory"] = "GetAgencyConfig";
                   Utilities.LogsDirectory = "GetAgencyConfig";
                   string agency_id = WebApiPoster.Models.Agency.GetAgencyIDByNumber(Agency);
                   IniParser iniParser = new IniParser(ConfigFile, agency_id);
                   //object a = JsonMaker.GetIOSJsonExtract("$", (object)"{  	\"agency_id\": \"355bfcc5-baae-11e3-b9ed-842b2b4bbc99\",  	\"section_ids\":[   		\"7b7d960d-03d9-11e0-8458-b228b30b1b68\",  		\"7b98b4b5-03d9-11e0-8458-b228b30b1b68\"  	]   }");
                   return iniParser.ConvertIniToJson();

              }
              catch (HttpResponseException ex) { Logger.LogException(ex); return ex.Response.ToString(); }
              catch (Exception ex) { Logger.LogException(ex); return "false"; }
         }
         [HttpPost]
         //public object GetFormDataForAgency([FromBody] object JsonData)
         //{
         //     try
         //     {
         //          if (JsonData == null)
         //               throw new HttpResponseException(HttpStatusCode.BadRequest) ;
         //               //return (new WebRequest).CreateResponse(HttpStatusCode.BadRequest, jsonResult.Data);  //"HTTP Error 400 - Bad Request";
                    

         //          HttpContext.Current.Session["LogDirectory"] = "GetFormDataForAgency";
         //          Utilities.LogsDirectory = "GetFormDataForAgency";
         //          //UrlRequests.AgencyFormdata_ParameterIDs.Add(JsonData.ToString());
         //          string agency_id = JsonMaker.GetIOSJsonExtract("$.agency_id", JsonData).ToString();

         //           string url = Configuratanager.ConnectionStrings["ConfigCouchUrl"].ToString() + "dynamicforms_generaration_data";
         //           string dynamicforms_genation_data;
         //           HttpResponseMessage rese = UrlRequests.UrlRequest(url, out dynamicforms_generaration_data, "GET");
         //           if (response.StatusCodeHttpStatusCode.OK)
         //           {
         //           JToken jtoken = JToken.e(dynamicforms_generaration_ddynamicforms_generaration_data JToken section_ids = (Jn)JsonMaker.GetIOSJsonExtract("section_ids", jtoken);
         //               JToken agencies_section(JToken)JsonMaker.GetIOSJsonExtract("agencies_sections", jtoken);
         //               return UrlRequests.GetFataByIdsection_ids, agencies_sections);
         //           }
                    
         //           return response;


         //   //JToken section_ids = (JTon)JsonMaker.GetIOSJsonExtract("$.section_ids", dynamicforms_generaration_ddynamicforms_generaration_dataoken agencies_sectio = (JToken)JsonMaker.GetIOSJsonExtract("agencies_sections", dynamicforms_generaration_ddynamicforms_generaration_dataturn UrlRequests.GetrmDataById(agency_id, section_ids, agencies_sections);

         //       //HttpContext ctx = HttpContext.Current;
         //       //new Thread(() =>
         //       //{
         //       //     Thread.CurrentThread.IsBackground = true;
         //       //     HttpContext.Current = ctx;
         //       //     UrlRequests.GetFormDataForAgency();
         //       //}).Start();
         //       return "{\"ok\":true}";

         //     }
         //     catch (HttpResponseException ex) { Logger.LogException(ex); return ex.Response.ToString(); }
         //     catch (JsonException ex) { Logger.LogException(ex); return "false"; }
         //     catch (Exception ex) { Logger.LogException(ex); return ex.Message ; }
              
         //}
         //public object GetFormDataForAgency([FromBody] object JsonData)
         //{
         //     try
         //     {
         //          if (JsonData == null)
         //               throw new HttpResponseException(HttpStatusCode.BadRequest) ;
         //               //return (new WebRequest).CreateResponse(HttpStatusCode.BadRequest, jsonResult.Data);  //"HTTP Error 400 - Bad Request";
                    

         //          HttpContext.Current.Session["LogDirectory"] = "GetFormDataForAgency";
         //          Utilities.LogsDirectory = "GetFormDataForAgency";
         //          //UrlRequests.AgencyFormdata_ParameterIDs.Add(JsonData.ToString());
         //          string agency_id = JsonMaker.GetIOSJsonExtract("$.agency_id", JsonData).ToString();

         //       string url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + "app_generaration_data";  //"dynamicforms_generaration_data";
         //           string dynamicforms_generaration_data;
         //           HttpResponseMessage response = UrlRequests.UrlRequest(url, out dynamicforms_generaration_data, "GET");
         //           if (response.StatusCode == HttpStatusCode.OK)
         //           {
         //               JToken jtoken = JToken.Parse(dynamicforms_generaration_data);
         //               JToken section_ids = (JToken)JsonMaker.GetIOSJsonExtract("dynamicForms.section_ids", jtoken);
         //               JToken agencies_sections = (JToken)JsonMaker.GetIOSJsonExtract("dynamicForms.agencies_sections", jtoken);
         //               return UrlRequests.GetFormDataById(agency_id, section_ids, agencies_sections);
         //           }
         //           else
         //               return response;


         //       //JToken section_ids = (JToken)JsonMaker.GetIOSJsonExtract("$.section_ids", dynamicforms_generaration_data);
         //       //   JToken agencies_sections = (JToken)JsonMaker.GetIOSJsonExtract("agencies_sections", dynamicforms_generaration_data);
         //       //   return UrlRequests.GetFormDataById(agency_id, section_ids, agencies_sections);

         //       //HttpContext ctx = HttpContext.Current;
         //       //new Thread(() =>
         //       //{
         //       //     Thread.CurrentThread.IsBackground = true;
         //       //     HttpContext.Current = ctx;
         //       //     UrlRequests.GetFormDataForAgency();
         //       //}).Start();
         //       return "{\"ok\":true}";

         //     }
         //     catch (HttpResponseException ex) { Logger.LogException(ex); return ex.Response.ToString(); }
         //     catch (JsonException ex) { Logger.LogException(ex); return "false"; }
         //     catch (Exception ex) { Logger.LogException(ex); return ex.Message ; }
              
         //}
         public Boolean UpdateAgencyConfig_old(string agency_id)
         {
              try
              {
                   
                   string url = ConfigurationManager.ConnectionStrings["GetAgencyUrl"].ToString()+ "&agency_id=" + agency_id;
                   HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                   webRequest.Method = "POST";
                    //webRequest.Headers.Add("Accept-Language", "en-us\r\n");
                    //webRequest.Headers.Add("UA-CPU", "x86 \r\n");
                    //webRequest.Headers.Add("Cache-Control", "no-cache\r\n");
                   // webRequest.ContentType = "text/xml";
                    //webRequest.KeepAlive = true;
                    //webRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                    Stream requestStream = webRequest.GetRequestStream();

                    string postData = "agency_id=" + agency_id;
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    // Set the ContentType property of the WebRequest.
                   // webRequest.ContentType = "application/x-www-form-urlencoded";
                    // Set the ContentLength property of the WebRequest.
                    //webRequest.ContentLength = byteArray.Length;
                    // Get the request stream.
                    using (Stream dataStream = webRequest.GetRequestStream())
                    {
                         dataStream.Write(byteArray, 0, byteArray.Length);
                    }

                   // WebResponse response = webRequest.GetResponse();
                    //UrlRequests UrlRequest = new UrlRequests();
                    //UrlRequest.StartWebRequest(webRequest);
                    string PHP_Json = "";
                    //UrlRequest.GetResponseAsync(webRequest, (response_async) =>
                    //{
                    //     PHP_Json = new StreamReader(response_async.GetResponseStream()).ReadToEnd();
                    //     HttpContext.Current.Session["json_out"] = PHP_Json;
                    //});
                    
                   // UrlRequest.DoWithResponse(webRequest,(response_async) => {
                   //      PHP_Json = new StreamReader(response_async.GetResponseStream()).ReadToEnd();
                   //     HttpContext.Current.Session["json_out"] = PHP_Json;
                   //});

                   // return ws.ResponseUri.ToString(); 
                    //StreamReader reader = new StreamReader(response.GetResponseStream());
                 //   string PHP_Json = reader.ReadToEnd();
                    

                    url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + agency_id;
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Method = "GET";
                    WebResponse response = webRequest.GetResponse();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string Couch_Json = reader.ReadToEnd();
                    string rev = JsonMaker.GetIOSJsonExtract("$._rev", (object) Couch_Json);
                    
                    if (!String.IsNullOrEmpty(rev))
                    {
                         JsonMaker.UpdateJsonValue("$._rev", rev);
                         PHP_Json=HttpContext.Current.Session["json_out"].ToString();
                    }
                    Logger.LogAction(url, "CouchStatus");
                    Logger.LogAction(PHP_Json, "CouchStatus");
                    webRequest = (HttpWebRequest)WebRequest.Create(url);
                    webRequest.Method = "PUT";
                   // webRequest2.ContentType = "text";
                    byteArray = Encoding.UTF8.GetBytes(PHP_Json);
                    using (Stream dataStream = webRequest.GetRequestStream())
                    {
                         dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                    response = webRequest.GetResponse();
                    bool status = false;
                   
                    using (reader = new StreamReader(response.GetResponseStream()))
                    {
                         Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                         Logger.LogAction(((HttpWebResponse)response).StatusDescription, "CouchStatus");
                         string responseFromServer = reader.ReadToEnd();
                         // Display the content.
                         Console.WriteLine(responseFromServer);
                         status = ((HttpWebResponse)response).StatusDescription.ToLower().Contains("ok") || ((HttpWebResponse)response).StatusDescription.ToLower().Contains("created");
                    }
                    return status;

             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        [HttpGet]
        public string GetPcrReport(string pcr_id, string FaxNumber, string Agency)
        {
            try
            {


                HttpContext.Current.Session["pcr_id"] = pcr_id;
                HttpContext.Current.Session["LogDirectory"] = "GetPcrReport";
                Logger.LogAction("Breakpoint1", "Breakpoints");
                Utilities.LogsDirectory = "GetPcrReport";

                
                string OnlinePath = String.Format("http://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/Report&pcrid={0}&autoload=true&btnfax=1", pcr_id);
                string ReportUrl = OnlinePath; // string.Format(OnlinePath + "&btnfax=1");
                Logger.LogAction("Breakpoint2", "Breakpoints");
                

                string FileName = "PcrReport_" + pcr_id + ".pdf";

                CreateFax CreateFax = new CreateFax();
                Logger.LogAction("Breakpoint3", "Breakpoints");
                string DestFile = CreateFax.CreateFile(pcr_id, ReportUrl);
                Logger.LogAction("Breakpoint5", "Breakpoints");

                string status = "";
                if (!String.IsNullOrEmpty(DestFile))
                {
                    Logger.LogAction("Breakpoint5a", "Breakpoints");
                    status = CreateFax.sendEmailFax(DestFile, FaxNumber, Agency);
                    Logger.LogAction("Breakpoint5b", "Breakpoints");
                    Logger.LogAction(ReportUrl + "fax status: " + status, "Activity");
                }
                else
                    status = " Error Creating PDF File";
                Logger.LogAction("Breakpoint5c", "Breakpoints");
                Logger.LogAction(ReportUrl + " status: " + status, "Activity");
                return status;
            }

            catch (Exception ex) { Logger.LogException(ex); return ex.Message; }

        }

    [HttpGet]
         public string GetPcrReport(string pcr_id, string agency_id, string FaxNumber, string agency_name)
         {
              try
              {

                   
                HttpContext.Current.Session["pcr_id"] = pcr_id;
                HttpContext.Current.Session["LogDirectory"] = "GetPcrReport";
                Logger.LogAction("Breakpoint1", "Breakpoints");
                Utilities.LogsDirectory = "GetPcrReport";
                Logger.LogAction("Breakpoint2", "Breakpoints");
                //string OnlinePath = ConfigurationManager.ConnectionStrings["GetReportUrl"].ToString() + "&agency_id=" + agency_id + "&neighborhood_id=" + neighborhood_id;
                string OnlinePath = ConfigTools.GetConfigValue("GetReportUrl", "https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/Report"); 
                Logger.LogAction("Breakpoint3", "Breakpoints");
                if (String.IsNullOrEmpty(OnlinePath))
                    OnlinePath= "https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/Report";
                Logger.LogAction("Breakpoint4", "Breakpoints");
                OnlinePath = String.Format(OnlinePath+ "&pcrid={0}&agency_id={1}&autoload=true&btnfax=1", pcr_id, agency_id);

                Logger.LogAction("Breakpoint5", "Breakpoints");

                string ReportUrl = OnlinePath; // string.Format(OnlinePath + "&btnfax=1");
                   Logger.LogAction("Breakpoint6", "Breakpoints");
                   //WebClient client = new WebClient();
                   //client.DownloadString(OnlinePath);

                   //WebBrowser wb = new WebBrowser();
                   //wb.ScrollBarsEnabled = false;
                   //wb.ScriptErrorsSuppressed = true;
                   //wb.Navigate(ReportUrl);
                   //while (wb.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }
                   //wb.Document.DomDocument.ToString();


                   //HttpWebRequest request = WebRequest.Create(OnlinePath) as HttpWebRequest;
                   //WebResponse response = request.GetResponse();

                   string FileName = "PcrReport_" + pcr_id + ".pdf";

                   CreateFax CreateFax = new CreateFax();
                   Logger.LogAction("Breakpoint7", "Breakpoints");
                   string DestFile = CreateFax.CreateFile(pcr_id, ReportUrl);
                   Logger.LogAction("Breakpoint5", "Breakpoints");
                    
                   string status = "";
                   if (!String.IsNullOrEmpty(DestFile)) 
                        status = CreateFax.sendEmailFax(DestFile, FaxNumber, agency_name);
                   else
                        status = " Error Creating PDF File";
                   Logger.LogAction(ReportUrl + " status: " + status, "Activity");
                   return status;
              }
             
              catch (Exception ex) { Logger.LogException(ex); return ex.Message; }

         }
     }

}
