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
using WebApiPoster.PCR;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Parse;
using System.Threading.Tasks;
using System.Text;

namespace WebApiPoster.Controllers
    {
    public class CEMSController : ApiController
        {
        private static string ParseUserName = "TestUser";
        private static string ParsePassword = "TestPass";

        public IHttpActionResult Get()
            {
            return Ok();
            }
        // localhost:api/CEMS/GetCads
        [HttpGet]
        public string GetCads(string device_id = "%", string cad_id = "%", string bus_id="%")
            {
            if (device_id == "%" && cad_id == "%" && bus_id=="%")
                return "";
            ExtendedDataTable dt = GetCadTable(device_id, cad_id, bus_id);
            List<object> myList = dt.toList();  //new List<object>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //     myList.Add(dr.Table);
            //}
            //string json = new JavaScriptSerializer().Serialize(myList);
            string json = JsonConvert.SerializeObject(myList);
            //UpdateDownloadedCad(device_id, cad_id);
            //System.IO.File.WriteAllText("c:\\temp\\Output.json", json);
            return json;
            }
       
        [HttpGet]
        public string GetCadsRaw(string device_id = "%", string cad_id = "%", string bus_id = "%")
            {
            if (device_id == "%" && cad_id == "%" && bus_id == "%") return "";
            ExtendedDataTable dt = GetCadTable(device_id, cad_id, bus_id);
            List<object> myList = new List<object>();
            foreach (DataRow dr in dt.Rows)
                {
                myList.Add(dr.ItemArray);
                }

            string json = JsonConvert.SerializeObject(myList);
            return json;
            }
        [HttpGet]
        public string GetNewChange(string change_id = "%")
        {
             New_Change newchange = new New_Change(change_id);
             string Json_String = "{" + JsonMaker.GetJSON(newchange)+"}";

             Logger.LogAction(((JObject)JsonConvert.DeserializeObject(Json_String)).ToString(), "JSON_Gets");
             return Json_String;
        }

        [HttpGet]
        public string GetPCR(string pcr_id = "%")
            {
            Utilities.UseRequiredFields = true;
            StringBuilder Pcr_Json = new StringBuilder();

            Pcr pcr = new Pcr(pcr_id);
            Pcr_Json.Append(JsonMaker.GetJSON(pcr) + System.Environment.NewLine);

            Dispatch Dispatch = new Dispatch(pcr.pcr_dispatch_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Dispatch) + System.Environment.NewLine);

            List<Members> MembersList = Utilities.GetClassList<Members>("pcr_members", pcr_id, "pcr_id");
            Pcr_Json.Append("," + JsonMaker.GetJSONFromList(MembersList, Prefix:"pcr_members") + System.Environment.NewLine);
           
            Demographic Demographic = new Demographic(pcr.pcr_demographic_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Demographic) + System.Environment.NewLine);

            Assessment Assessment = new Assessment(pcr.pcr_assessment_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Assessment) + System.Environment.NewLine);

            Narrative_Notes Narrative_Notes = new Narrative_Notes(pcr.pcr_narrative_notes_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Narrative_Notes) + System.Environment.NewLine);

            Rma Rma = new Rma(pcr.pcr_rma_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Rma) + System.Environment.NewLine);

            Apcf Apcf = new Apcf(pcr.pcr_apcf_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Apcf) + System.Environment.NewLine);

            Disposition Disposition = new Disposition(pcr.pcr_disposition_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Disposition) + System.Environment.NewLine);

            Ems_run Ems_Run = new Ems_run(pcr.ems_run);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Ems_Run) + System.Environment.NewLine);

            Narcotic Narcotic = new Narcotic(pcr.pcr_narcotics_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Narcotic) + System.Environment.NewLine);

            PCR.Authorization Authorization = new PCR.Authorization(pcr.pcr_authorization_id);
            Pcr_Json.Append("," + JsonMaker.GetJSON(Authorization));

            string SelectQuery = "(SELECT a.* FROM pcr_buttons a inner join all_buttons b on a.button_id = b.id inner join sections c on b.section_id = c.id) buttons";
            List<Buttons> ButtonsList = Utilities.GetClassList<Buttons>(SelectQuery, pcr_id, "pcr_id");
            Pcr_Json.Append("," + JsonMaker.GetJSONFromList(ButtonsList, Prefix: "pcr_buttons") + System.Environment.NewLine);

            SelectQuery = "(SELECT a.* FROM pcr_inputs a inner join all_buttons b on a.input_id = b.id inner join sections c on b.section_id = c.id) inputs";
            List<Inputs> InputsList = Utilities.GetClassList<Inputs>(SelectQuery, pcr_id, "pcr_id");
            Pcr_Json.Append("," + JsonMaker.GetJSONFromList(InputsList, Prefix: "pcr_inputs") + System.Environment.NewLine);

            Pcr_Json.Insert(0, "{");
            Pcr_Json.Append("}");
            Logger.LogAction(((JObject)JsonConvert.DeserializeObject(Pcr_Json.ToString())).ToString (), "JSON_Gets");
            return Pcr_Json.ToString();
            }


        private void UpdateDownloadedCad(string device_id = "%", string cad_id = "%")
            {
            try
                {

                using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                    {
                    cn.Open();
                    string UpdateSql = "update cad_number set downloaded_time = CURRENT_TIMESTAMP() where (ifnull(downloaded_time,0)=0 or year(downloaded_time)=1970) and id like '" + cad_id + "' and agency_id like '" + device_id + "'";
                    MySqlCommand cmd = new MySqlCommand(UpdateSql, cn);
                    cmd.ExecuteNonQuery();
                    }
                }
            catch (Exception ex)
                {
                Logger.LogException(ex);
                }
            }


        private ExtendedDataTable GetCadTable(string device_id = "%", string cad_id = "%", string bus_id = "%")
            {
            ExtendedDataTable dt = new ExtendedDataTable();
            //List<object> obj = new List<object>();
            try
                {
                using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
                    {
                    cn.Open();
                    //List<string> SelectList = new List<string>();
                    StringBuilder SelectList = new StringBuilder();
                    SelectList.Append("a.id as cad_number_id,");
                    SelectList.Append("cad_number as cad_number_cad_number,");
                    SelectList.Append("a.agency_id as cad_number_agency_id,");
                    SelectList.Append("pcr_demographic_id as cad_number_pcr_demographic_id,");
                    SelectList.Append("pcr_disposition_id as cad_number_pcr_disposition_id,");
                    SelectList.Append("call_intake_id as cad_number_call_intake_id,");
                    SelectList.Append("pcr_id as cad_number_pcr_id,");
                    SelectList.Append("bus_id as cad_number_bus_id,");
                    SelectList.Append("date as cad_number_date,");
                    SelectList.Append("schedule_return as cad_number_schedule_return,");
                    SelectList.Append("downloaded as cad_number_downloaded,");
                    SelectList.Append("downloaded_time as cad_number_downloaded_time,");
                    SelectList.Append("cancelled as cad_number_cancelled,");
                    SelectList.Append("firstCrewMember as cad_number_firstCrewMember,");
                    SelectList.Append("secondCrewMember as cad_number_secondCrewMember,");
                    SelectList.Append("is_schedule_return as cad_number_is_schedule_return,");
                    SelectList.Append("is_cancelled as cad_number_is_cancelled,");
                    SelectList.Append("a.utc_insert as cad_number_utc_insert,");
                    SelectList.Append("a.utc_update as cad_number_utc_update,");
                    SelectList.Append("user_login_id as cad_number_user_login_id,");
                    SelectList.Append("schedule_cad_id as cad_number_schedule_cad_id,");
                    SelectList.Append("caller_name as cad_number_caller_name,");
                    SelectList.Append("caller_phone as cad_number_caller_phone,");
                    SelectList.Append("transfer_care as cad_number_transfer_care,");
                    SelectList.Append("is_transfer_care as cad_number_is_transfer_care,");
                    SelectList.Append("a.active as cad_number_active,");
                    SelectList.Append("is_dry_run as cad_number_is_dry_run,");
                    SelectList.Append("first_name as pcr_demographic_first_name,");
                    SelectList.Append("last_name as pcr_demographic_last_name,");
                    SelectList.Append("e.address as address_address,");
                    SelectList.Append("city.id as city_id,");
                    SelectList.Append("city.city_name as city_city,");
                    SelectList.Append("state.id as state_id,");
                    SelectList.Append("state.state_name as state_state,");
                    SelectList.Append("zip.id as zip_id,");
                    SelectList.Append("zip.zip_code as zip_zip,");
                    SelectList.Append("country.id as country_id,");
                    SelectList.Append("country.country_name as country_country");
                    string SelectString = SelectList.ToString();     // string.Join(",", SelectList);
                    string QueryString = "SELECT " + SelectString + " FROM cad_number a " + System.Environment.NewLine +
                                             "left outer join call_intake b " + System.Environment.NewLine +
                                             "on a.call_intake_id = b.id " + System.Environment.NewLine +
                                             "left outer join pcr_demographic c " + System.Environment.NewLine +
                                             "on a.pcr_demographic_id = c.id " + System.Environment.NewLine +
                                             "left outer join person d " + System.Environment.NewLine +
                                             "on c.pt_person = d.id " + System.Environment.NewLine +
                                             "left outer join address e " + System.Environment.NewLine +
                                             "on e.id = ifnull(b.facility_id, b.address_id) " + System.Environment.NewLine +
                                             "left outer join city on e.city_id=city.id " + System.Environment.NewLine +
                                             "left outer join state on e.state_id=state.id " + System.Environment.NewLine +
                                             "left outer join zip on e.zip_id=zip.id " + System.Environment.NewLine +
                                             "left outer join country on e.country_id=country.id " + System.Environment.NewLine +
                                             "where a.id like '" + cad_id + "' and a.agency_id like '" + device_id + "'" + " and a.bus_id like '" + bus_id + "'" +
                                             " and (ifnull(downloaded_time,0)=0 or year(downloaded_time)=1970)";
                    //where a.id = '70d9fafb-64da-4eb3-b3d8-f99950952474' and and a.agency_id = '90b16e1c-aa76-11e5-b94a-842b2b4bbc99';
                    MySqlCommand cmd = new MySqlCommand(QueryString, cn);

                    dt.Load(cmd.ExecuteReader());

                    }
                }
            catch (Exception ex) { Logger.LogException(ex); return null; }
            return dt;

            }

        

          [HttpPost]
        public Boolean PostPcrFromIOS([FromBody] object JsonData)
          {
             try
                {
                    Pcr pcr = new Pcr();
                    pcr.MapFromIOSJson(JsonData);
                    return true;
                }
            catch (Exception ex) { Logger.LogException(ex); return false; }

            }
         [HttpGet]
          public Boolean GetJsonFromEMS(string pcr_id)
          {
               try
               {
                    Pcr.OutgoingJson = "";
                    Pcr pcr = new Pcr();
                    Json_pcr jcon_pcr = new Json_pcr(pcr_id);
                    jcon_pcr.Retrieve();
                    //object JsonData = Utilities.GetIOSJson(pcr_id);
                    if (jcon_pcr.data == null) jcon_pcr.data = (new JObject()).ToString();
                    pcr.MapIntoIOSJson(jcon_pcr.data, pcr_id);
                    jcon_pcr.data = Pcr.OutgoingJson.ToString();
                    jcon_pcr.sent = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");
                    jcon_pcr.HandleRecord();
                    //Utilities.OutputIOSJson(Pcr.OutgoingJson.ToString(), pcr_id);
                    return true;
               }
               catch (Exception ex) { Logger.LogException(ex); return false; }

          }
         [HttpGet]
          public Boolean GetJsonFromIOS(string pcr_id)
          {
               try
               {
                    Pcr pcr = new Pcr();
                    Json_pcr jcon_pcr = new Json_pcr(pcr_id);
                    jcon_pcr.Retrieve();
                    if (jcon_pcr.data != null)
                         pcr.MapFromIOSJson(jcon_pcr.data, pcr_id);
                         jcon_pcr.received = Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd hh:mm:ss");
                         jcon_pcr.HandleRecord();
                    return true;
               }
               catch (Exception ex) { Logger.LogException(ex); return false; }

          }
        [HttpPost]
        public Boolean PostJsonToSql([FromBody] object JsonData)
            {
            try
                {
                JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JsonData.ToString());
                string TableName;
                foreach (JToken token in JsonObject.SelectToken(""))
                    {

                    foreach (JToken Fields in token)
                        {
                        int LastIndex = 0;
                        if (Fields.Type.ToString() == "Array") LastIndex = Fields.Count() - 1;


                        for (int i = 0; i <= LastIndex; i++)
                            {
                            JToken WorkFields;
                            if (Fields.Type.ToString() == "Array")
                                WorkFields = Fields.ToArray()[i];
                            else
                                WorkFields = Fields;
                            JsonInputSection PcrSection = new JsonInputSection();
                            foreach (JToken Field in WorkFields.Children())
                                {
                                var Property = Field as JProperty;
                                PcrSection[Property.Name] = Property.Value.ToString();
                                }
                            TableName = token.Path.ToString();
                            if (TableName.Contains("."))
                                TableName = TableName.Remove(0, TableName.IndexOf(".") + 1);
                            Assembly Asm = Assembly.Load(Assembly.GetExecutingAssembly().FullName);
                            Type PCRSectionType = Asm.GetTypes().First(t => token.Path.ToUpper().EndsWith(t.Name.ToUpper()));
                            dynamic objSectionOut = Activator.CreateInstance(PCRSectionType, TableName, PcrSection);
                            objSectionOut.HandleRecord();
                            }
                        }
                    }
                Logger.LogAction(JsonObject.ToString(), "JSON_Posts");
                return true;
                }
            catch (Exception ex) { Logger.LogException(ex); return false; }

            }




        }

    }
