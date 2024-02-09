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
    public class ZollController : ApiController
    {
        
        //[HttpPost]
        //public object GetPcrFromZoll([FromBody] object JsonData)
        //{
        //    try
        //    {
        //        //Pcr.OutgoingJson = new JObject();
        //        HttpContext.Current.Session["LogDirectory"] = "GetPcrFromZoll";
               
               
        //        JToken LeadData = JsonMaker.GetIOSJsonExtract("$.LeadData", JsonData);
        //        foreach (JToken token in LeadData.SelectToken(""))
        //        {
        //            switch(token.Path)
        //            {
        //                case "LeadV6":
        //                    JArray EkgArray = (JArray)token.SelectToken("");
        //                    if (EkgArray != null)
        //                        foreach (JToken ekg in EkgArray)
        //                        {
        //                            button["choice"] = "off";
        //                        }
        //                    break;
        //            }
        //        }


        //            demographic.MapFromIOSJson();
        //        string SelectQuery = "(select p.*, section_name, ButtonID, name  from pcr_buttons p inner join all_buttons a on p.button_id=a.id left outer join sections s on a.section_id=s.id where  pcr_id = '" + pcr_id + "' and section_name='pmhMappmhA-iList')";
        //        List<Buttons> ButtonsList = Utilities.GetClassList<Buttons>(SelectQuery);

        //        foreach (Buttons button in ButtonsList)
        //        {
        //            button.LoadGenButton("GEN:" + button._section_name);
        //        }
        //        Json_pcr json_pcr = new Json_pcr(pcr_id, "0");
        //        json_pcr.data = HttpContext.Current.Session["json_out"].ToString();  //Pcr.OutgoingJson.ToString();
        //        json_pcr.sent = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");
        //        json_pcr.last_name = last_name;
        //        json_pcr.dob = Convert.ToDateTime(dob).ToString("yyyy-MM-dd hh:mm:ss");
        //        json_pcr.HandleRecord();
        //        return pcr_id;

        //    }
        //    catch (Exception ex) { Logger.LogException(ex); return false; }
        //}
    }
}
