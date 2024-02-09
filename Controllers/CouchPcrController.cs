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
using System.Web.Http.Cors;

namespace WebApiPoster.Controllers
{
    [DisableCors]
    public class CouchPcrController : ApiController
    {
        private string OriginalDBUrl;
        private string TemporaryDBUrl;
        [HttpGet]
        public HttpResponseMessage CleanCouchService()
        {
            OriginalDBUrl = ConfigTools.GetConfigValue("OriginalDBUrl", "http://CemsCouchAdmin:CemsCouchAdmin%21Pass5984@192.168.1.154:5984/app-pcrs");
            TemporaryDBUrl = ConfigTools.GetConfigValue("TemporaryDBUrl", "http://CemsCouchAdmin:CemsCouchAdmin%21Pass5984@192.168.1.154:5984/rep-app-pcrs");
            HttpContext.Current.Session["LogDirectory"] = "CleanCouchService";
            if (DeleteTemporaryDB())
            {
                //if (CopyDB("app-pcrs", "rep-app-pcrs"))
                if (CopyDB(OriginalDBUrl, TemporaryDBUrl))
                {
                    if (DeleteOriginalDB())
                    {
                        //                        if (CopyDB("rep-app-pcrs", "app-pcrs"))
                        if (CopyDB(TemporaryDBUrl, OriginalDBUrl))
                            DeleteTemporaryDB();
                    }
                }

            }
            return (HttpResponseMessage) HttpContext.Current.Session["status"];

        }
        [HttpGet]
        public HttpResponseMessage CleanHeapCouchService()
        {
            OriginalDBUrl = ConfigTools.GetConfigValue("HeapOriginalDBUrl", "http://CemsCouchAdmin:CemsCouchAdmin%21Pass5984@192.168.1.154:5984/app-heap");
            TemporaryDBUrl = ConfigTools.GetConfigValue("HeapTemporaryDBUrl", "http://CemsCouchAdmin:CemsCouchAdmin%21Pass5984@192.168.1.154:5984/rep-app-heap");
            HttpContext.Current.Session["LogDirectory"] = "CleanCouchService";
            if (DeleteTemporaryDB())
            {
                //if (CopyDB("app-pcrs", "rep-app-pcrs"))
                if (CopyDB(OriginalDBUrl, TemporaryDBUrl))
                {
                    if (DeleteOriginalDB())
                    {
                        //                        if (CopyDB("rep-app-pcrs", "app-pcrs"))
                        if (CopyDB(TemporaryDBUrl, OriginalDBUrl))
                            DeleteTemporaryDB();
                    }
                }

            }
            return (HttpResponseMessage)HttpContext.Current.Session["status"];

        }
        public bool DeleteTemporaryDB()
        {

            string url = TemporaryDBUrl; // ConfigTools.GetConfigValue("TemporaryDBUrl", "http://CemsCouchAdmin:CemsCouchAdmin%21Pass5984@192.168.1.154:5984/rep-app-pcrs");
            
            string content;
            
            //HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(url);
            WebHeaderCollection WebHeaderCollection = new WebHeaderCollection();
            //WebHeaderCollection.Add("Authorization", "Basic Q2Vtc0NvdWNoQWRtaW46Q2Vtc0NvdWNoQWRtaW4hUGFzczU5ODQ=");
            HttpResponseMessage response = UrlRequests.UrlRequest(url, out content, "GET", WebHeaders: WebHeaderCollection);
            if (response.StatusCode == HttpStatusCode.OK)
                response = UrlRequests.UrlRequest(url, out content, "DELETE", WebHeaders: WebHeaderCollection);
            response.Headers.Add("url", url);
            response.Headers.Add("operation", "DeleteTemporaryDB");
            HttpContext.Current.Session["status"] = response;
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound;
            
        }
        public bool DeleteOriginalDB()
        {
            string url = OriginalDBUrl; // ConfigTools.GetConfigValue("OriginalDBUrl", "http://CemsCouchAdmin:CemsCouchAdmin%21Pass5984@192.168.1.154:5984/app-pcrs");
            
            string content;
            WebHeaderCollection WebHeaderCollection = new WebHeaderCollection();
            // WebHeaderCollection.Add("Authorization", "Basic Q2Vtc0NvdWNoQWRtaW46Q2Vtc0NvdWNoQWRtaW4hUGFzczU5ODQ=");
            string PostmanToken = ConfigTools.GetConfigValue("PostmanToken", "3dda11d4-5daf-461d-bf70-e37d7d8d4ab6");
            WebHeaderCollection.Add("Postman-Token", PostmanToken);
            WebHeaderCollection.Add("Cache-Control", "no-cache");
            HttpResponseMessage response = UrlRequests.UrlRequest(url, out content, "GET", WebHeaders: WebHeaderCollection);
            if (response.StatusCode == HttpStatusCode.OK)
                response = UrlRequests.UrlRequest(url, out content, "DELETE", WebHeaders: WebHeaderCollection);
            response.Headers.Add("url", url);
            response.Headers.Add("operation", "DeleteOriginalDB");
            HttpContext.Current.Session["status"] = response;
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NotFound;

        }
       
        public bool CopyDB(string param_source_url, string param_dest_url)
        {
            string url = ConfigTools.GetConfigValue("ReplicateDBUrl", "http://CemsCouchAdmin:CemsCouchAdmin%21Pass5984@192.168.1.154:5984/_replicate");
            
            string content;
            StringBuilder param_json = new StringBuilder();
            param_json.Append("{");
            param_json.Append("\"source\": { ");
            param_json.Append("\"headers\": { ");
            param_json.Append("\"Authorization\": \"`0'\"");
            param_json.Append("},");
            param_json.Append("\"url\": \"`1'\"");
            param_json.Append("},");
            param_json.Append("\"target\": {");
            param_json.Append("\"headers\": {");
            param_json.Append("\"Authorization\": \"`2'\"");
            param_json.Append("},");
            param_json.Append("\"url\": \"`3'\"");
            param_json.Append("},");
            param_json.Append("\"create_target\": true, ");
            param_json.Append("\"filter\": \"app/deletedfilter\", ");
            param_json.Append("\"continuous\": false");
            param_json.Append("}");
            string param_authorization= "Basic Q2Vtc0NvdWNoQWRtaW46Q2Vtc0NvdWNoQWRtaW4hUGFzczU5ODQ =";
            //string param_source_url = source;
            //string param_dest_url = destination;
            string json_string = param_json.ToString().Replace("{", "@").Replace("}", "#").Replace("`","{").Replace("'", "}");

            string data = String.Format(json_string, param_authorization, param_source_url, param_authorization, param_dest_url).Replace("@", "{").Replace("#", "}");



            //            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            WebHeaderCollection WebHeaderCollection = new WebHeaderCollection();
            //WebHeaderCollection.Add("Authorization", "Basic Q2Vtc0NvdWNoQWRtaW46Q2Vtc0NvdWNoQWRtaW4hUGFzczU5ODQ=");
            HttpResponseMessage response = UrlRequests.UrlRequest(url, out content, "POST", data: data);
            string doc_write_failures = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                object obj_doc_write_failures = JsonMaker.GetIOSJsonExtract("$.history[0].doc_write_failures", content);
                doc_write_failures = obj_doc_write_failures==null ? "" : obj_doc_write_failures.ToString();
            }
            response.Headers.Add("url", url);
            response.Headers.Add("data", data);
            response.Headers.Add("operation", "Copy " + param_source_url + " to " + param_dest_url);
            HttpContext.Current.Session["status"] = response;

        
            return response.StatusCode == HttpStatusCode.OK && doc_write_failures == "0";

        }
    }
}
