using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using WebApiPoster;
using WebApiPoster.Models;
using System.Linq;
using System.Web.Http;
using System.Threading;
using System.Windows.Forms;

public static class UrlRequests
{
     public static string forms_document_name = "app_generaration_data";
     public static List<string> AgencyConfig_ParameterIDs = new List<string>();
     public static List<string> AgencyFormdata_ParameterIDs = new List<string>();
     public static List<string> Pcr_ParameterIDs = new List<string>();
     public static List<object> SetupAgencies_ParameterIDs = new List<object>();
  //   private HttpWebRequest webRequest;
    // private static CookieContainer cookieContainer = new CookieContainer();
     public static HttpResponseMessage UrlRequest(string url, out string content,  string method = "POST", string content_type = "application/json", string data = null, WebHeaderCollection WebHeaders = null, bool NoAuthorization=false)
     {
        try
        {

            //            HttpWebRequest webRequest = (ParamRequest==null) ? (HttpWebRequest)WebRequest.Create(url) : ParamRequest;

            Logger.LogAction("UrlRequest for: " + url + " method: " + method, "Activity");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = method;
            if (WebHeaders != null) webRequest.Headers = WebHeaders;
            if (!NoAuthorization)
                AuthorizeRequest(url, ref webRequest);
            webRequest.ContentType = content_type;
            // Stream requestStream = webRequest.GetRequestStream();
            if (data != null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(data);
                using (Stream dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
            }
            System.Diagnostics.Stopwatch timer = new Stopwatch();
            timer.Start();
            WebResponse response = webRequest.GetResponse();
            timer.Stop();
            Logger.LogAction(url + ":" + System.Environment.NewLine + method + " elapsed time: " + timer.Elapsed + System.Environment.NewLine + response + ":" + ((HttpWebResponse)response).StatusDescription, "CouchStatus");
            //webRequest.CookieContainer = cookieContainer;
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string ResponseString = reader.ReadToEnd();
            HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.OK);
            response_msg.Content = new StringContent(ResponseString, Encoding.UTF8, "application/json");
            content = ResponseString;
            Logger.LogAction(content, "Activity");
            return response_msg;

        }
        catch (WebException ex)
        {
            content = ex.Message;
            Logger.LogException(ex);
            if (ex.Response != null) { 
                var resp = (HttpWebResponse)ex.Response;
                HttpResponseMessage response_msg = new HttpResponseMessage(resp.StatusCode);
                return response_msg;
            }
            else
            {
                var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "text/plain");
                return response_msg;
            }

        }
        catch (HttpResponseException ex)
        {
            Logger.LogException(ex);
            content = ex.Message;
            return ex.Response;
        }
        catch (Exception ex)
        {
            Logger.LogException(ex);
            var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
            response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "text/plain");
            content = ex.Message;
            return response_msg;
        }
        
    }

     public static HttpResponseMessage GetMessages(string agency_id, string user_id)
     {
          try
          {
               HttpResponseMessage response;
               string url;
               url =String.Format("https://www.creativeems.com/MessageService/AUBEMessage?a={0}&u={1}",agency_id,user_id);
               string content;
               response = UrlRequest(url, out content, "GET");

               JArray msgs = null;
               try
               {
                    msgs = (JArray ) JToken.Parse(content);
               }
               catch { }


               if (msgs!=null) {
                    foreach (JToken msg in msgs)
                    {
                         JToken work_msg = msg;
                         JsonMaker.UpdateJsonValue("$.read_status", 0, ref work_msg);
                         message_inbox inbox = new message_inbox();
                         inbox.MapFromJson(work_msg); 
                    }
                    url = ConfigurationManager.ConnectionStrings["MessagesCouchUrl"].ToString() + "/inbox-" + user_id;
                    response = UrlRequest(url, out content, "GET");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                         JToken get_content = JToken.Parse(content);
                         string rev = JsonMaker.GetIOSJsonExtract("$._rev", get_content);
                         response = UrlRequest(url + "?rev=" + rev, out content, "DELETE");
                    }
                    string data=msgs.ToString(); 
                    data = data.StartsWith("[") ? String.Format("{{\"messages\":\"{0}\"}}", data) : data;
                    //else
                    //     data = data.StartsWith("[") ? String.Format("{{\"rev\":\"{0}\", \"messages\":\"{1}\"}}", rev, data) : data;
                    response = UrlRequest(url, out content, "PUT", data: data);
               }
               return response;

          }
          catch (Exception ex)
          {
               var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
               response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "text/plain");
               return response_msg;
          }

     }
     public static HttpResponseMessage ClearOutbox(string agency_id, string user_id)
     {
          try
          {
               HttpResponseMessage response;
               string url;
               
               string content;

               url = ConfigurationManager.ConnectionStrings["MessagesCouchUrl"].ToString() + "/outbox-" + user_id;
               response = UrlRequest(url, out content, "GET");

               JArray msgs =  (JArray ) JToken.Parse(JsonMaker.GetIOSJsonExtract("$.messages", (JToken) content));               

               if (msgs != null)
               {
                    foreach (JToken msg in msgs)
                    {
                         message_inbox inbox = new message_inbox();
                         inbox.MapFromJson(msg);
                    }

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                         JToken get_content = JToken.Parse(content);
                         string rev = JsonMaker.GetIOSJsonExtract("$._rev", get_content);
                         response = UrlRequest(url + "?rev=" + rev, out content, "DELETE");
                    }
               }
               return response;

          }
          catch (Exception ex)
          {
               var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
               response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "text/plain");
               return response_msg;
          }

     }


     public static HttpResponseMessage ExpectedPcr2Couch(object JsonData)
     {
          try
          {


               string url = ConfigurationManager.ConnectionStrings["PcrCouchUrl"].ToString();
               HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
               AuthorizeRequest(url, ref webRequest);
               webRequest.Method = "PUT";
               // webRequest2.ContentType = "text";
               byte[] byteArray = Encoding.UTF8.GetBytes((string)JsonData);
               using (Stream dataStream = webRequest.GetRequestStream())
               {
                    dataStream.Write(byteArray, 0, byteArray.Length);
               }
               System.Diagnostics.Stopwatch timer = new Stopwatch();
               timer.Start();
               WebResponse response = webRequest.GetResponse();
               timer.Stop();

               StreamReader reader = new StreamReader(response.GetResponseStream());
               string ResponseString = reader.ReadToEnd();
               HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.OK);
               response_msg.Content = new StringContent(ResponseString, Encoding.UTF8, "application/json");
               Logger.LogAction(url + ":" + System.Environment.NewLine + "PUT elapsed time: " + timer.Elapsed + System.Environment.NewLine + response + ":" + ((HttpWebResponse)response).StatusDescription, "CouchStatus");

               return response_msg;

          }
          catch (Exception ex)
          {
               //Logger.LogException(ex); HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.NotFound);
               //response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
               //return response_msg;
               var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
               response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
               return response_msg;
          }

     }


    private static  void  AuthorizeRequest(string url, ref HttpWebRequest webRequest)
    {       

            if (url.Contains("@") && url.Contains(":"))
            {
                int pos1 = url.IndexOf("://");
                if (pos1 == -1) pos1 = -3;
                int pos2 = url.IndexOf("@");
                string credentials = url.Substring(pos1 + 3, pos2 - pos1 - 3);
                string user_name = credentials.Split(':')[0];
                string password = credentials.Split(':')[1];
                webRequest.UseDefaultCredentials = true;
                webRequest.PreAuthenticate = true;
                webRequest.Credentials = new System.Net.NetworkCredential(user_name, password);
            }
        string Authorization = ConfigTools.GetConfigValue("Authorization", "Basic Q2Vtc1NlcnZpY2VBZG1pbjpkZjM1QCNZMkBDSWc =");
        if (!webRequest.Headers.AllKeys.Contains("Authorization") && !String.IsNullOrEmpty(Authorization))
            webRequest.Headers.Add("Authorization", Authorization);

    }
            //         DEPRECATED
            public static HttpResponseMessage ImportPcr(string id)
     {
          try
          {

               string url = ConfigurationManager.ConnectionStrings["PcrCouchUrl"].ToString() + id;
               HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
               AuthorizeRequest(url, ref webRequest);
               webRequest.Method = "GET";

               System.Diagnostics.Stopwatch timer = new Stopwatch();
               timer.Start();
               WebResponse response = webRequest.GetResponse();
               timer.Stop();
               Logger.LogAction(url + ":" + System.Environment.NewLine + "GET elapsed time: " + timer.Elapsed + System.Environment.NewLine + response + ":" + ((HttpWebResponse)response).StatusDescription, "CouchStatus");
               StreamReader reader = new StreamReader(response.GetResponseStream());
               string Couch_Json = reader.ReadToEnd();


               //Logger.LogAction(PHP_Json, "CouchStatus");
               url = ConfigurationManager.ConnectionStrings["PostPcrFromCouchUrl"].ToString();
               webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Method = "POST";
               // webRequest2.ContentType = "text";
               byte[] byteArray = Encoding.UTF8.GetBytes(Couch_Json);
               using (Stream dataStream = webRequest.GetRequestStream())
               {
                    dataStream.Write(byteArray, 0, byteArray.Length);
               }
               timer.Start();
               response = webRequest.GetResponse();
               timer.Stop();
               
               bool status = false;

               Console.WriteLine(((HttpWebResponse)response).StatusDescription);
               status = ((HttpWebResponse)response).StatusDescription.ToLower().Contains("ok") || ((HttpWebResponse)response).StatusDescription.ToLower().Contains("created");
               Logger.LogAction(url + ":" + System.Environment.NewLine + "POST elapsed time: " + timer.Elapsed + System.Environment.NewLine + response + ":" + ((HttpWebResponse)response).StatusDescription, "CouchStatus");

               reader = new StreamReader(response.GetResponseStream());
               string ResponseString = reader.ReadToEnd();
               string pcr_id = JsonMaker.GetIOSJsonExtract("$.pcr_id", (object)ResponseString);

               if (!String.IsNullOrEmpty(pcr_id))
                    JsonMaker.UpdateJsonValue("$.pcr_id", pcr_id, ref Couch_Json);

               url = ConfigurationManager.ConnectionStrings["PcrCouchUrl"].ToString() + id; 
               webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Method = "PUT";
               // webRequest2.ContentType = "text";
               byteArray = Encoding.UTF8.GetBytes(Couch_Json);
               using (Stream dataStream = webRequest.GetRequestStream())
               {
                    dataStream.Write(byteArray, 0, byteArray.Length);
               }
               timer.Start();
               response = webRequest.GetResponse();
               timer.Stop();

               reader = new StreamReader(response.GetResponseStream());
               ResponseString = reader.ReadToEnd();
               HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.OK);
               response_msg.Content = new StringContent(ResponseString, Encoding.UTF8, "application/json");
               Logger.LogAction(url + ":" + System.Environment.NewLine + "PUT elapsed time: " + timer.Elapsed + System.Environment.NewLine + response + ":" + ((HttpWebResponse)response).StatusDescription, "CouchStatus");

               return response_msg;

          }
          catch (Exception ex)
          {
               //Logger.LogException(ex); HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.NotFound);
               //response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
               //return response_msg;
               var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
               response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "application/json");
               return response_msg;
          }
          //catch (WebException ex) { Logger.LogException(ex); return ex.Response.ResponseUri.ToString()+ ":  " + ex.Message; }
          //catch (HttpResponseException ex) { Logger.LogException(ex); return ex.Response.ToString(); ; }
          //catch (Exception ex) { Logger.LogException(ex); return ex.Message; }
     }
     private static Boolean UpdateAgencyConfig_old(string parameter_id)
     {
          try
          {
//  Get PHP JSON
               string agency_id = parameter_id.Split('@')[0];
               string neighborhood_id = "";
               if (parameter_id.Contains("@"))
                    neighborhood_id = parameter_id.Split('@')[1];
               string url = ConfigurationManager.ConnectionStrings["GetAgencyUrl"].ToString() + "&agency_id=" + agency_id + "&neighborhood_id=" + neighborhood_id;
               HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Method = "POST";

               Stream requestStream = webRequest.GetRequestStream();

               //string postData = "agency_id=" + agency_id;
               //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
               //// Set the ContentType property of the WebRequest.
               //// webRequest.ContentType = "application/x-www-form-urlencoded";
               //// Set the ContentLength property of the WebRequest.
               ////webRequest.ContentLength = byteArray.Length;
               //// Get the request stream.
               //using (Stream dataStream = webRequest.GetRequestStream())
               //{
               //     dataStream.Write(byteArray, 0, byteArray.Length);
               //}
               //string LogsDirectory = HttpContext.Current.Session["LogDirectory"].ToString();
               //HttpContext ctx = HttpContext.Current;

               //webRequest.KeepAlive = false;
               //webRequest.CookieContainer = cookieContainer;
               System.Diagnostics.Stopwatch timer = new Stopwatch();
               timer.Start();
               WebResponse response = webRequest.GetResponse();
               timer.Stop();
               Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + " POST elapsed time: " + timer.Elapsed, "CouchStatus");
               //webRequest.CookieContainer = cookieContainer;
               StreamReader reader = new StreamReader(response.GetResponseStream());
               string PHP_Json = reader.ReadToEnd();

//  Get Couch JSON
               string use_id = (!String.IsNullOrEmpty(neighborhood_id) ? neighborhood_id : agency_id);

               url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + "config-" + use_id;
               webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Method = "GET";

               try
               {
                    timer.Start();
                    response = webRequest.GetResponse();
                    timer.Stop();
                    Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + " GET elapsed time: " + timer.Elapsed, "CouchStatus");
                    reader = new StreamReader(response.GetResponseStream());
                    string Couch_Json = reader.ReadToEnd();
                    string rev = JsonMaker.GetIOSJsonExtract("$._rev", (object)Couch_Json);

                    if (!String.IsNullOrEmpty(rev))
                    {
                         JsonMaker.UpdateJsonValue("$._rev", rev, ref PHP_Json);
                         //PHP_Json = HttpContext.Current.Session["json_out"].ToString();
                    }
                    Logger.LogAction(url, "CouchStatus");
               }
               catch (Exception ex)
               {

               }
//  Update PHP JSON with Couch data
               JsonMaker.UpdateJsonValue("$._id", "config-" + use_id, ref PHP_Json);
               JsonMaker.UpdateJsonValue("$.type", "config", ref PHP_Json);

               //  Get agency settings from the local =system and add to the new JSON
               string agency_name = Agency.GetAgencyNumberByID(agency_id);
               string ConfigFile = ConfigurationManager.ConnectionStrings["AgencyConfigPath"].ToString() + "\\" + agency_name + "\\agencysettings.ini";
               WebApiPoster.IniParser iniParser = new WebApiPoster.IniParser(ConfigFile,agency_id);
               //JToken iniJson = iniParser.ConvertIniToJson();
               //JToken PHP_Json_JToken = (JToken)PHP_Json;
               //JsonMaker.UpdateJsonValue("$.Config", iniJson, ref PHP_Json_JToken);
               //Logger.LogAction(PHP_Json, "CouchStatus");

//  Get global config settings JSON
               url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + "global-config";
               webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Method = "GET";
               timer.Start();
               response = webRequest.GetResponse();
               timer.Stop();
               Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + " GET elapsed time: " + timer.Elapsed, "CouchStatus");
               reader = new StreamReader(response.GetResponseStream());
               string strConfig = reader.ReadToEnd();
               JToken Config_Json = (JToken)JsonConvert.DeserializeObject(strConfig);
               Config_Json = iniParser.UpdateJsonFromIni(Config_Json);
               JToken PHP_Json_JToken = (JToken)PHP_Json;
               JsonMaker.UpdateJsonValue("$.Config", Config_Json, ref PHP_Json_JToken);

               Logger.LogAction(url, "CouchStatus");


               //  Put the updated PHP JSON on Couch
               webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Method = "PUT";
               // webRequest2.ContentType = "text";
               byte[] byteArray = Encoding.UTF8.GetBytes(PHP_Json);
               using (Stream dataStream = webRequest.GetRequestStream())
               {
                    dataStream.Write(byteArray, 0, byteArray.Length);
               }
               timer.Start();
               response = webRequest.GetResponse();
               timer.Stop();
               Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + "PUT elapsed time: " + timer.Elapsed, "CouchStatus");
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
    public static HttpResponseMessage UpdateAgencyConfig(string agency_id, string neighborhood_id, string fax)
    { 
        string out_response_string;

        return UpdateAgencyConfig(agency_id, neighborhood_id, fax, out out_response_string);
    }
        public static HttpResponseMessage UpdateAgencyConfig(string agency_id, string neighborhood_id, string fax, out string out_response_string)
     {
        string url="";
          try
          {
//  Get PHP JSON
               
               url = ConfigurationManager.ConnectionStrings["GetAgencyUrl"].ToString() + "&agency_id=" + agency_id + "&neighborhood_id=" + neighborhood_id+ "&fax=" + fax;
               HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
               webRequest.Method = "GET";
               //Stream requestStream = webRequest.GetRequestStream();

               //string postData = "agency_id=" + agency_id;
               //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
               //// Set the ContentType property of the WebRequest.
               //// webRequest.ContentType = "application/x-www-form-urlencoded";
               //// Set the ContentLength property of the WebRequest.
               ////webRequest.ContentLength = byteArray.Length;
               //// Get the request stream.
               //using (Stream dataStream = webRequest.GetRequestStream())
               //{
               //     dataStream.Write(byteArray, 0, byteArray.Length);
               //}
               //string LogsDirectory = HttpContext.Current.Session["LogDirectory"].ToString();
               //HttpContext ctx = HttpContext.Current;

               //webRequest.KeepAlive = false;
               //webRequest.CookieContainer = cookieContainer;
               System.Diagnostics.Stopwatch timer = new Stopwatch();
               timer.Start();
               WebResponse response = webRequest.GetResponse();
               timer.Stop();
               Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + " POST elapsed time: " + timer.Elapsed, "CouchStatus");
               //webRequest.CookieContainer = cookieContainer;
               StreamReader reader = new StreamReader(response.GetResponseStream());
               string PHP_Json = reader.ReadToEnd();


//  Get Couch JSON
                string use_id = (!String.IsNullOrEmpty(neighborhood_id) ? neighborhood_id : agency_id);
                url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + "config-" + use_id;
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                AuthorizeRequest(url, ref webRequest);
                webRequest.Method = "GET";

               try
               {
                    timer.Start();
                    response = webRequest.GetResponse();
                    timer.Stop();
                    Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + " GET elapsed time: " + timer.Elapsed, "CouchStatus");
                    reader = new StreamReader(response.GetResponseStream());
                    string Couch_Json = reader.ReadToEnd();
                    string rev = JsonMaker.GetIOSJsonExtract("$._rev", (object)Couch_Json);

                    if (!String.IsNullOrEmpty(rev))
                    {
                         JsonMaker.UpdateJsonValue("$._rev", rev, ref PHP_Json);
                         //PHP_Json = HttpContext.Current.Session["json_out"].ToString();
                    }
                    Logger.LogAction(url, "CouchStatus");
               }
               catch (Exception ex)
               {

               }
//  Update PHP JSON with Couch data
              JsonMaker.UpdateJsonValue("$._id", "config-" + use_id, ref PHP_Json);
               JsonMaker.UpdateJsonValue("$.type", "config", ref PHP_Json);

               JToken PHP_Json_JToken = (JToken)PHP_Json;

            //  Get agency settings from the local system and add to the new JSON
            //WebApiPoster.IniParser iniParser = new WebApiPoster.IniParser(ConfigFile);
            //JToken iniJson = iniParser.ConvertIniToJson();
            //JToken PHP_Json_JToken = (JToken)PHP_Json;
            //JsonMaker.UpdateJsonValue("$.Config", iniJson, ref PHP_Json_JToken);
            //Logger.LogAction(PHP_Json, "CouchStatus");

               JToken dynamic_data;
               JToken agency_settings = JsonMaker.GetIOSJsonExtract("$.agencySettingFile", (object)PHP_Json_JToken);
               HttpContext.Current.Session["IniFileContent"] = agency_settings; 
               JToken Config_Json = GetGlobalConfig(agency_id, neighborhood_id, out dynamic_data);
               JsonMaker.UpdateJsonValue("$.staticPages", Config_Json, ref PHP_Json_JToken);
               JsonMaker.UpdateJsonValue("$.dynamicPages", dynamic_data, ref PHP_Json_JToken);
               JsonMaker.UpdateJsonValue("$.Updated", DateTime.Now.ToString(), ref PHP_Json_JToken);

            //  Put the updated PHP JSON on Couch
            url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + "config-" + use_id; 
               webRequest = (HttpWebRequest)WebRequest.Create(url);
               AuthorizeRequest(url, ref webRequest);
               webRequest.Method = "PUT";
               // webRequest2.ContentType = "text";
               byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PHP_Json_JToken));
               using (Stream dataStream = webRequest.GetRequestStream())
               {
                    dataStream.Write(byteArray, 0, byteArray.Length);
               }
               timer.Start();

               response = webRequest.GetResponse();
               timer.Stop();
               Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + "PUT elapsed time: " + timer.Elapsed, "CouchStatus");
               bool status = false;
               string responseFromServer;
               HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.OK);
               using (reader = new StreamReader(response.GetResponseStream()))
               {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    Logger.LogAction(((HttpWebResponse)response).StatusDescription, "CouchStatus");
                    responseFromServer = reader.ReadToEnd();
                    Logger.LogAction(JsonConvert.DeserializeObject(responseFromServer).ToString(), "CouchStatus");
                    response_msg.Content = new StringContent(responseFromServer, Encoding.UTF8, "application/json");
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                    out_response_string = responseFromServer;
                    status = ((HttpWebResponse)response).StatusDescription.ToLower().Contains("ok") || ((HttpWebResponse)response).StatusDescription.ToLower().Contains("created");
               }
               return response_msg;

          }
        catch (WebException ex)
        {
            Logger.LogException(ex, url: url); 

            if (ex.Response != null)
            {
                var resp = (HttpWebResponse)ex.Response;
                HttpResponseMessage response_msg = new HttpResponseMessage(resp.StatusCode);
                out_response_string = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                response_msg.Content = new StringContent(out_response_string, Encoding.UTF8, "application/json");
                return response_msg;
            }
            else
            {
                var response_msg = new HttpResponseMessage(HttpStatusCode.BadRequest);
                response_msg.Content = new StringContent(ex.Message, Encoding.UTF8, "text/plain");
                out_response_string = "";
                return response_msg;
            }

        }
        catch (HttpResponseException ex)
        {
            Logger.LogException(ex, url: url); throw ex;
            //Logger.LogException(ex);
            //content = ex.Message;
            //return ex.Response;
        }
        catch (Exception ex) { Logger.LogException(ex, url:url); throw ex; }
     }
    private static JToken GetGlobalConfig(string agency_id, string neighborhood_id, out JToken dynamic_data, string doc_name = "app_generaration_data")
    {
        string agency_name = Agency.GetAgencyNumberByID(agency_id);
        string ConfigFile = ConfigurationManager.ConnectionStrings["AgencyConfigPath"].ToString() + "\\" + agency_name + "\\agencysettings.ini";

        string url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + doc_name; // "agency_config_template";
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        AuthorizeRequest(url, ref webRequest);
        webRequest.Method = "GET";

        System.Diagnostics.Stopwatch timer = new Stopwatch();
        timer.Start();
        WebResponse response = webRequest.GetResponse();
        timer.Stop();
        Logger.LogAction("Agency " + agency_id + ", Neightborhood " + neighborhood_id + ":" + System.Environment.NewLine + " GET elapsed time: " + timer.Elapsed, "CouchStatus");
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string strConfig = reader.ReadToEnd();
        JToken Config_Json = (JToken)JsonConvert.DeserializeObject(strConfig);

        JToken static_data = JsonMaker.GetIOSJsonExtract("$.staticPages", Config_Json);
        WebApiPoster.IniParser iniParser = new WebApiPoster.IniParser(ConfigFile, agency_id);
        static_data = iniParser.UpdateJsonFromIni(static_data);

        dynamic_data = JsonMaker.GetIOSJsonExtract("$.dynamicPages", Config_Json);
        dynamic_data = (JObject)JsonConvert.DeserializeObject(dynamic_data.ToString());
        dynamic_data = iniParser.UpdateJsonFromIni(dynamic_data, is_dynamic:true);
        Logger.LogAction(url, "CouchStatus");
        return static_data;
               
}
    public static JToken SetupAllAgencies()
    {
        return SetupAgencies();
    }
    public static JToken SetupAllConfigs()
    {
        return SetupAgencies(skip_get_form:true);
    }
    public static JToken SetupAgencies(object Agencies_Json=null, bool skip_get_form=false)
     {
          try
          {
            string url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + forms_document_name; // "dynamicforms_generaration_data";
               string content;
               HttpResponseMessage response = UrlRequest(url, out content, "GET");
               JToken JsonData;
               if (response.StatusCode == HttpStatusCode.OK)
               {
                    JsonData = JToken.Parse(content);
               }
               else
                    return JToken.Parse("{ok : false, error : \""+content+"\"}");
               HttpResponseMessage ResponseMessage1;
               HttpResponseMessage ResponseMessage2;
               List<object> ResponseMessages = new List<object>();
               JToken agencies;
               if(Agencies_Json==null)
                    agencies = (JToken)JsonMaker.GetIOSJsonExtract("$.dynamicForms.agencies", JsonData);
               else
                    agencies = (JToken)JsonMaker.GetIOSJsonExtract("$.agencies", Agencies_Json);

               JArray AgenciesArray = (JArray)JsonConvert.DeserializeObject((agencies.ToString()));
               JToken section_ids = (JToken)JsonMaker.GetIOSJsonExtract("$.dynamicForms.section_ids", JsonData);
               JToken agencies_sections = (JToken)JsonMaker.GetIOSJsonExtract("$.dynamicForms.agencies_sections", JsonData);

               StringBuilder ResponseMessageBuilder = new StringBuilder();
               JToken ResponsesJson = new JObject();
               foreach (JToken agency_token in AgenciesArray)
               {
                    string agency_id = JsonMaker.GetIOSJsonExtract("$.agency_id", agency_token) ?? "";
                    string neighborhood_id = JsonMaker.GetIOSJsonExtract("$.neighborhood_id", agency_token) ?? "";
                    string agency_name = Agency.GetAgencyNumberByID(agency_id);
                //string server_address = "";
                //url = String.Format("https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=UserSettings/UpdateAgencyConfig&agency={0}&server_address={1}", agency_name, server_address);                    
                //ResponseMessage1 = UrlRequest(url, out response_string1, "GET");
                    string response_string1;
                    ResponseMessage1 = UpdateAgencyConfig(agency_id, neighborhood_id, "1", out response_string1);
                    string response_string2="";
                    if (!skip_get_form)
                    {
                        
                        ResponseMessage2 = GetFormDataById(agency_id, section_ids, agencies_sections, out response_string2);
                        //string ResponseMessageString = "{\"agency\": \"" + agency_id + "\", \"UpdateAgencyConfig Response\": " + JsonConvert.SerializeObject(ResponseMessage) + "}";
                        JsonMaker.ModifyArrayItem(ref ResponsesJson, "$.responses", "agency", agency_id, "agency", agency_id);
                    }
                   
                    JToken DetailsToken = new JObject();
                    JToken IniFileToken;
                    try
                    {
                        IniFileToken = JToken.Parse(response_string1);
                    }
                    catch(Exception ex)
                    {
                        IniFileToken = JToken.Parse("{}");
                    }
                    JsonMaker.UpdateJsonValue("$.status", IniFileToken, ref DetailsToken);
                    JsonMaker.ModifyArrayItem(ref ResponsesJson, "$.responses", "agency", agency_id, "UpdateAgencyConfig", DetailsToken);
                    if (!skip_get_form)
                    {
                        DetailsToken = new JObject();
                        try
                        {
                            IniFileToken = JToken.Parse(response_string2);
                        }
                        catch (Exception ex)
                        {
                            IniFileToken = JToken.Parse("{}");
                        }
                        JsonMaker.UpdateJsonValue("$.status", IniFileToken, ref DetailsToken);
                        JsonMaker.ModifyArrayItem(ref ResponsesJson, "$.responses", "agency", agency_id, "GetFormDataForAgency", DetailsToken);
                    }

               }
               return ResponsesJson; // (JToken)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ResponseMessages));

          }
          catch (WebException ex) { Logger.LogException(ex); return ex.Response.ResponseUri.ToString() + ":  " + ex.Message; }
          catch (HttpResponseException ex) { Logger.LogException(ex); return ex.Response.ToString(); ; }
          catch (Exception ex) { Logger.LogException(ex); return ex.Message; }
     }

    public static JToken SetupAgencies_old(object JsonData)
    {
        try
        {
            HttpResponseMessage ResponseMessage1;
            HttpResponseMessage ResponseMessage2;
            List<object> ResponseMessages = new List<object>();
            JToken agencies = (JToken)JsonMaker.GetIOSJsonExtract("$.agencies", JsonData);
            JArray AgenciesArray = (JArray)JsonConvert.DeserializeObject((agencies.ToString()));




            JToken section_ids = (JToken)JsonMaker.GetIOSJsonExtract("$.section_ids", JsonData);
            JToken agencies_sections = (JToken)JsonMaker.GetIOSJsonExtract("agencies_sections", JsonData);

            string url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + forms_document_name;
            string content;
            HttpResponseMessage response = UrlRequest(url, out content, "GET");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JToken jtoken = JToken.Parse(content);
                section_ids = (JToken)JsonMaker.GetIOSJsonExtract("section_ids", jtoken);
                agencies_sections = (JToken)JsonMaker.GetIOSJsonExtract("agencies_sections", jtoken);
            }

            StringBuilder ResponseMessageBuilder = new StringBuilder();
            JToken ResponsesJson = new JObject();
            foreach (JToken agency_token in AgenciesArray)
            {
                string agency_id = JsonMaker.GetIOSJsonExtract("$.agency_id", agency_token) ?? "";
                string neighborhood_id = JsonMaker.GetIOSJsonExtract("$.neighborhood_id", agency_token) ?? "";
                ResponseMessage1 = UpdateAgencyConfig(agency_id, neighborhood_id, "1");
                ResponseMessage2 = GetFormDataById(agency_id, section_ids, agencies_sections);
                //string ResponseMessageString = "{\"agency\": \"" + agency_id + "\", \"UpdateAgencyConfig Response\": " + JsonConvert.SerializeObject(ResponseMessage) + "}";
                JsonMaker.ModifyArrayItem(ref ResponsesJson, "$.responses", "agency", agency_id, "agency", agency_id);

                JToken DetailsToken = new JObject();
                JsonMaker.UpdateJsonValue("$.status", ResponseMessage1.StatusCode.ToString(), ref DetailsToken);
                JsonMaker.ModifyArrayItem(ref ResponsesJson, "$.responses", "agency", agency_id, "UpdateAgencyConfig", DetailsToken);

                DetailsToken = new JObject();
                JsonMaker.UpdateJsonValue("$.status", ResponseMessage2.StatusCode.ToString(), ref DetailsToken);
                JsonMaker.ModifyArrayItem(ref ResponsesJson, "$.responses", "agency", agency_id, "GetFormDataForAgency", DetailsToken);


                //ResponseMessageBuilder.Clear();
                //ResponseMessageBuilder.Append("{\"agency\": \"");
                //ResponseMessageBuilder.Append(agency_id);
                //ResponseMessageBuilder.Append("\", \"UpdateAgencyConfig Response\": ");
                //ResponseMessageBuilder.Append(JsonConvert.SerializeObject(ResponseMessage1));
                //ResponseMessageBuilder.Append(", \"GetFormDataForAgency Response\": ");
                //ResponseMessageBuilder.Append(JsonConvert.SerializeObject(ResponseMessage2));
                //ResponseMessageBuilder.Append("}");
                //ResponseMessages.Add(JsonConvert.DeserializeObject(ResponseMessageBuilder.ToString()));

                //ResponseMessageString = "{\"agency\": \"" + agency_id + "\", \"GetFormDataForAgency  Response\": " + JsonConvert.SerializeObject(ResponseMessage1) + "}";
                //ResponseMessages.Add(ResponseMessage);
            }
            return ResponsesJson; // (JToken)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ResponseMessages));

        }
        catch (WebException ex) { Logger.LogException(ex); return ex.Response.ResponseUri.ToString() + ":  " + ex.Message; }
        catch (HttpResponseException ex) { Logger.LogException(ex); return ex.Response.ToString(); ; }
        catch (Exception ex) { Logger.LogException(ex); return ex.Message; }
    }
    public static HttpResponseMessage GetFormDataById(string agency_id, JToken section_ids, JToken agencies_sections)
    {
        string response_string;
        return GetFormDataById(agency_id, section_ids, agencies_sections, out response_string);
    }
        public static HttpResponseMessage GetFormDataById(string agency_id, JToken section_ids, JToken agencies_sections, out string response_string)
     {
          try
          {
               HttpWebRequest webRequest;
               WebResponse response;
               StreamReader reader;
               StringBuilder JsonStringBuilder=new StringBuilder();
               byte[] byteArray;

               JToken PHP_JsonObject = new JObject();
               string PHP_JsonString = "";

               string url;
               System.Diagnostics.Stopwatch timer= new Stopwatch();


            // string agency_id = JsonMaker.GetIOSJsonExtract("$.agency_id", parameter_id).ToString();
            // JToken jtoken = (JToken)JsonMaker.GetIOSJsonExtract("$.section_ids", parameter_id);
            //   JArray agencies_array = (JArray)JsonConvert.DeserializeObject((agencies_sections.ToString()));
            ////JToken selected_agency = agencies_sections_array.Children().FirstOrDefault(x => x["id"].ToString() == agency_id);
            //    foreach (JObject agency_token in agencies_array)
            //    {
            //        foreach (JProperty prop in ((JObject)agency_token).Properties())
            //        {
            //            if (prop.Name==agency_id)  {
            //            if section_ids.co
            //            }
            //        }
                    
                
            //    }
                //foreach (KeyValuePair<string, JToken> sub_obj in (JObject)agencies_array)
                //{
                //    Console.WriteLine(sub_obj.Key);
                //}
                //agency_token.ke
                //    JArray sections_array = (JArray)JsonConvert.DeserializeObject((agency_token.ToString()));
                //}
                if (section_ids != null)
                {
                    JArray InSectionArray = (JArray)JsonConvert.DeserializeObject((section_ids.ToString()));

                    JArray agencies_array = (JArray)JsonConvert.DeserializeObject((agencies_sections.ToString()));
                    //JToken selected_agency = agencies_sections_array.Children().FirstOrDefault(x => x["id"].ToString() == agency_id);
                    foreach (JObject agency_token in agencies_array)
                    {
                        foreach (JProperty prop in ((JObject)agency_token).Properties())
                        {
                            if (prop.Name == agency_id)
                            {
                                JArray sections_array = (JArray)JsonConvert.DeserializeObject((agency_token.SelectToken(prop.Name).ToString()));
                                foreach (JToken section_token in sections_array) {
                                    if (InSectionArray.Children().FirstOrDefault(x => x.ToString() == section_token.ToString()) == null)
                                        InSectionArray.Add(section_token);
                                }
                                
                            }
                        }
                    }
                    timer = new Stopwatch();
                    timer.Start();
                    foreach (JToken section_token in InSectionArray)
                    {
                         string section_id = section_token.ToString();
                         url = ConfigurationManager.ConnectionStrings["GetFormDataUrl"].ToString(); // + "&agency_id=" + agency_id + "&section_id=" + section_id;
                         Dictionary<string, string> PostData = new Dictionary<string, string>();
                         PostData.Add("agency_id", agency_id);
                         PostData.Add("section_id", section_id);
                         string PostJson = JsonConvert.SerializeObject(PostData);
                         webRequest = (HttpWebRequest)WebRequest.Create(url);
                         AuthorizeRequest(url, ref webRequest);
                         webRequest.Method = "POST";

                         byteArray = Encoding.UTF8.GetBytes(PostJson);
                         webRequest.ContentType = "application/json";
                         webRequest.ContentLength = byteArray.Length;
                         // Get the request stream.
                         using (Stream dataStream = webRequest.GetRequestStream())
                         {
                              dataStream.Write(byteArray, 0, byteArray.Length);
                         }
      
                         response = webRequest.GetResponse();
                         //webRequest.CookieContainer = cookieContainer;
                         reader = new StreamReader(response.GetResponseStream()); 
                         string PHP_Section = reader.ReadToEnd();

                         
                         JArray OutSectionArray= new JArray();

                         //JToken tokenToAdd = JToken.Parse(JsonConvert.SerializeObject(newObject, Formatting.Indented));
                        // OutSectionArray.Add (new JToken())
                         //JToken ID_Token= (JToken) new JObject();;
                         //JsonMaker.UpdateJsonValue("$.id", section_id, ref ID_Token);
                         //JToken Data_Token= (JToken) new JObject();;
                         //JsonMaker.UpdateJsonValue("$.data", PHP_Section, ref Data_Token);
                         
                         //JToken Out_Section_Token=(JToken) new JObject();
                         JsonMaker.ModifyArrayItem(ref PHP_JsonObject, "$.sections", "id", section_id, "id", section_id);
                         JsonMaker.ModifyArrayItem(ref PHP_JsonObject, "$.sections", "id", section_id, "data", JsonMaker.GetIOSJsonExtract("$.data", PHP_Section));
                        // string asda = (JObject) (JsonMaker.GetIOSJsonExtract("$.data", PHP_Section).ToString());
                         //("sections","[]");
                       //  Out_Section_Token.Children().add

                         //section_item.
                         //OutSectionArray.Add()
                         //jobject.Add("sections",)
                       //  JsonString=PHP_Json;
                         
                    }
                    timer.Stop();
                    Logger.LogAction("Agency " + agency_id + ":" + System.Environment.NewLine + " POST elapsed time: " + timer.Elapsed, "CouchStatus");
               }


                Logger.LogAction("GetFormDataById", "Trace");
                url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + "dbForms-" + agency_id;
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                AuthorizeRequest(url, ref webRequest);
                webRequest.Method = "GET";

               try
               {
                    timer.Start();
                    response = webRequest.GetResponse();
                    timer.Stop();
                    Logger.LogAction("Agency " + agency_id +  ":" + System.Environment.NewLine + " GET elapsed time: " + timer.Elapsed, "CouchStatus");
                    reader = new StreamReader(response.GetResponseStream());
                    string Couch_Json = reader.ReadToEnd();
                    string rev = JsonMaker.GetIOSJsonExtract("$._rev", (object)Couch_Json);

                    if (!String.IsNullOrEmpty(rev))
                    {
                         JsonMaker.UpdateJsonValue("$._rev", rev, ref PHP_JsonObject);
                         //PHP_Json = HttpContext.Current.Session["json_out"].ToString();
                    }
                   
                    Logger.LogAction(url, "CouchStatus");
               }
               catch (Exception ex)
               {

               }
               JsonMaker.UpdateJsonValue("$._id", "dbForms-" + agency_id, ref PHP_JsonObject);
               JsonMaker.UpdateJsonValue("$.type", "dbForms", ref PHP_JsonObject);
               JsonMaker.UpdateJsonValue("$.Updated", DateTime.Now.ToString(), ref PHP_JsonObject);
            //Logger.LogAction(PHP_Json, "CouchStatus");
                webRequest = (HttpWebRequest)WebRequest.Create(url);
                AuthorizeRequest(url, ref webRequest);
                webRequest.Method = "PUT";
                webRequest.ContentType = "application/json; charset=UTF-8";
                webRequest.Accept = "application/json";
               

            //   JObject JsonObject = JObject.Parse(@PHP_JsonObject.ToString());
            //  // var PHP_Chars = PHP_JsonObject.ToString().ToCharArray().Where(e => e!='\n' && e!= '\r');

            //  char[] chars=PHP_JsonObject.ToString().ToCharArray().Where(e => e != '\n' && e != '\r').ToArray();
            //  byteArray = Encoding.UTF8.GetBytes(chars);
            ////  var b = Encoding.UTF8.GetBytes(PHP_JsonObject.ToString()).ToList();
            //   byteArray = Utilities.ObjectToByteArray(Encoding.UTF8.GetBytes(PHP_JsonObject.ToString()).Where(e => e != '\n' && e != '\r').ToList()); // (byte[])Encoding.UTF8.GetBytes(PHP_JsonObject.ToString()).Where(e => e != '\n' && e != '\r');
               byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(PHP_JsonObject)); //Encoding.UTF8.GetBytes (PHP_JsonObject.ToString().Replace(System.Environment.NewLine, "").Replace("\\r\\n", "")); //.Replace("'", "~~").Replace("\"", "'").Replace("~~", "'"));
               webRequest.ContentLength = byteArray.Length;
               using (Stream dataStream = webRequest.GetRequestStream())
               {
                    dataStream.Write(byteArray, 0, byteArray.Length);
               }
               timer.Start();
               response = webRequest.GetResponse();
               timer.Stop();
               Logger.LogAction("Agency " + agency_id + System.Environment.NewLine + "PUT elapsed time: " + timer.Elapsed, "CouchStatus");
               bool status = false;
               string responseFromServer;
               HttpResponseMessage response_msg = new HttpResponseMessage(HttpStatusCode.OK);
               using (reader = new StreamReader(response.GetResponseStream()))
               {
                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                    Logger.LogAction(((HttpWebResponse)response).StatusDescription, "CouchStatus");
                    responseFromServer = reader.ReadToEnd();
                    Logger.LogAction(JsonConvert.DeserializeObject(responseFromServer).ToString(), "CouchStatus");
                    response_msg.Content = new StringContent(responseFromServer, Encoding.UTF8, "application/json");
                    // Display the content.
                    Console.WriteLine(responseFromServer);
                    response_string = responseFromServer;
                    status = ((HttpWebResponse)response).StatusDescription.ToLower().Contains("ok") || ((HttpWebResponse)response).StatusDescription.ToLower().Contains("created");
               }
               return response_msg;

          }
          catch (Exception ex) {
            Logger.LogException(ex); throw ex;
        }
     }
     public static void UpdateTimer()
     {
       //   List<string> AgencyConfig_ParameterIDs = (List<string>)HttpContext.Current.Application["Agency IDs"];
          if (AgencyConfig_ParameterIDs != null)
          {
               while (1==1) {
                    System.Windows.Forms.Application.DoEvents();
                    while (AgencyConfig_ParameterIDs.Count > 0)
                    {
                         System.Windows.Forms.Application.DoEvents();
                         string ParameterID = AgencyConfig_ParameterIDs[0];
                         string agency_id = ParameterID.Split('@')[0];
                         string neighborhood_id = "";
                         if (ParameterID.Contains("@"))
                              neighborhood_id = ParameterID.Split('@')[1];
                         UpdateAgencyConfig(agency_id, neighborhood_id, "1");
                         AgencyConfig_ParameterIDs.RemoveAt(0);
                    }
                    System.Windows.Forms.Application.DoEvents();
                    //while (Pcr_ParameterIDs.Count > 0)
                    //{
                    //     System.Windows.Forms.Application.DoEvents();
                    //     string ParameterID = Pcr_ParameterIDs[0];
                    //     PCR2COUCH(ParameterID);
                    //     Pcr_ParameterIDs.RemoveAt(0);
                    //}
                    while(SetupAgencies_ParameterIDs.Count>0)
                    {
                         System.Windows.Forms.Application.DoEvents();
                         object ParameterID = SetupAgencies_ParameterIDs[0];
                         JToken token_status = SetupAgencies(ParameterID);
                         string status = JsonConvert.SerializeObject(token_status);
                         Logger.LogAction(JsonConvert.DeserializeObject(status).ToString(), "SetupAgencies");
                         SetupAgencies_ParameterIDs.RemoveAt(0);
                    }
               }
              // HttpContext.Current.Application["Agency IDs"] = AgencyConfig_ParameterIDs;
          }
     }
     public static HttpResponseMessage SetupForms(string agency_id)
     {
          string url = ConfigurationManager.ConnectionStrings["ConfigCouchUrl"].ToString() + forms_document_name;
          string content;
          HttpResponseMessage response = UrlRequest(url, out content, "GET");
          if (response.StatusCode == HttpStatusCode.OK)
          {
                JToken jtoken = JToken.Parse(content);
                JToken section_ids = (JToken)JsonMaker.GetIOSJsonExtract("dynamicForms.section_ids", jtoken);
                JToken agencies_sections = (JToken)JsonMaker.GetIOSJsonExtract("dynamicForms.agencies_sections", jtoken);
                return GetFormDataById(agency_id, section_ids, agencies_sections);
          }
          else
               return response;
     }
     public static void GetFormDataForAgency()
     {
          //   List<string> AgencyFormdata_ParameterIDs = (List<string>)HttpContext.Current.Application["Agency IDs"];
          if (AgencyFormdata_ParameterIDs != null)
          {
               while (AgencyFormdata_ParameterIDs.Count > 0)
               {
                    System.Windows.Forms.Application.DoEvents();
                    string ParameterID = AgencyFormdata_ParameterIDs[0];
                    string agency_id = JsonMaker.GetIOSJsonExtract("$.agency_id", ParameterID).ToString();
                    JToken section_ids = (JToken)JsonMaker.GetIOSJsonExtract("$.section_ids", ParameterID);
                    JToken agencies_sections = (JToken)JsonMaker.GetIOSJsonExtract("agencies_sections", ParameterID);
                    GetFormDataById(agency_id, section_ids, agencies_sections);
                    AgencyFormdata_ParameterIDs.RemoveAt(0);
               }
               // HttpContext.Current.Application["Agency IDs"] = AgencyFormdata_ParameterIDs;
          }
     }
     //public void StartWebRequest(HttpWebRequest request)
     //{
     //     this.webRequest = request;
     //     webRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), null);
     //}

     //private static void FinishWebRequest(IAsyncResult asyncResult)
     //{
     //     HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
     //     try
     //     {
     //          using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult))
     //          {
     //               Stream responseStream = response.GetResponseStream();
     //               using (StreamReader sr = new StreamReader(responseStream))
     //               {
     //                    //Need to return this response 
     //                    string strContent = sr.ReadToEnd();
     //               }
     //          }
     //     }
     //     catch (Exception ex)
     //     {
     //          throw ex;
     //     }
     //}
     //public void FinishWebRequest(IAsyncResult result)
     //{
     //     //string Output=result.ge()).ReadToEnd();

     //     Stream responseStream = webRequest.EndGetResponse(result).GetResponseStream();
     //     return responseStream.re;
     //     return
     //}
     public static  void BackgroundWork()
     {
          var worker = new BackgroundWorker();

          worker.DoWork += (sender, args) =>
          {
               args.Result = new WebClient().DownloadString("");
          };

          worker.RunWorkerCompleted += (sender, e) =>
          {
               if (e.Error != null)
               {

               }
               else
               {
               }
          };

          worker.RunWorkerAsync();
     }


    public static void NavigateToSite(string url)
    {
        Thread t = new Thread(StartBrowser);
        t.SetApartmentState(ApartmentState.STA);
        t.Start(url);
        t.Join();
        //ParameterizedThreadStart ptsd = new ParameterizedThreadStart(StartBrowser);
        //Thread t = new Thread(tsd);
        //Thread firstThread = new Thread(new ThreadStart(StartBrowser));
        Thread thread = new Thread(delegate ()
        {
            Logger.LogAction("Breakpoint_a1", "Breakpoints");


        });
        Logger.LogAction("Breakpoint_b", "Breakpoints");
        thread.SetApartmentState(ApartmentState.STA);
        Logger.LogAction("Breakpoint_c", "Breakpoints");
        thread.Start();
        Logger.LogAction("Breakpoint_d", "Breakpoints");
        //thread.Join();
    }

    private static void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
        Logger.LogAction("Document Completed", "Activity");
        browse_done = true;
        //WebBrowser browser = sender as WebBrowser;
        //using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height))
        //{
        //     browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
        //     using (MemoryStream stream = new MemoryStream())
        //     {
        //          bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //          byte[] bytes = stream.ToArray();
        //          imgScreenShot.Visible = true;
        //          imgScreenShot.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(bytes);
        //     }
        //}
    }
   
    static bool browse_done = false;

    static void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
    {

        Logger.LogAction("Document Navigated", "Activity");
        browse_done = true;
    }
    public static void StartBrowser(object UrlOfReport)
    {

        using (WebBrowser browser = new WebBrowser())
        {
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
            browser.Navigated += new WebBrowserNavigatedEventHandler(webBrowser_Navigated);
            browse_done = false;
            Logger.LogAction("Start Navigate Thread", "Activity");
            browser.Visible = true;
            browser.ScrollBarsEnabled = false;
            browser.AllowNavigation = true;
            browser.Navigate(UrlOfReport.ToString());
            browser.Width = 1024;
            browser.Height = 768;
            browser.ScriptErrorsSuppressed = true;
            //                browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
            System.Threading.Thread.Sleep(5000);
            Logger.LogAction("browse_done: " + browse_done.ToString(), "Activity");
            System.Diagnostics.Stopwatch stopwatch = Stopwatch.StartNew();
            while (!browse_done)
            {
                System.Windows.Forms.Application.DoEvents();
                System.Threading.Thread.Sleep(1000);
                if (stopwatch.Elapsed.TotalSeconds > 20)
                {
                    Logger.LogAction("More than 20 seconds", "Activity");
                    break;
                }
            }
            Logger.LogAction("loop done", "Activity");
            Logger.LogAction("browse_done: " + browse_done.ToString(), "Activity");
            stopwatch.Stop();

        }
    }
    //public void GetResponseAsync(HttpWebRequest request, Action<HttpWebResponse> gotResponse)
    //{
    //     if (request != null)
    //     {
    //          request.BeginGetRequestStream((r) =>
    //          {
    //               try
    //               { // there's a try/catch here because execution path is different from invokation one, exception here may cause a crash
    //                    HttpWebResponse response =(HttpWebResponse) request.EndGetResponse(r);
    //                    if (gotResponse != null)
    //                         gotResponse(response);
    //               }
    //               catch (Exception x)
    //               {
    //                    Console.WriteLine("Unable to get response for '" + request.RequestUri + "' Err: " + x);
    //               }
    //          }, null);
    //     }
    //}


    //public void DoWithResponse(HttpWebRequest request, Action<HttpWebResponse> responseAction)
    //{

    //     Action wrapperAction = () =>
    //     {
    //          request.BeginGetResponse(new AsyncCallback((iar) =>
    //          {
    //               var response = (HttpWebResponse)((HttpWebRequest)iar.AsyncState).EndGetResponse(iar);
    //               responseAction(response);
    //          }), request);
    //     };
    //     wrapperAction.BeginInvoke(new AsyncCallback((iar) =>
    //     {
    //          var action = (Action)iar.AsyncState;
    //          action.EndInvoke(iar);
    //     }), wrapperAction);
    //}
}