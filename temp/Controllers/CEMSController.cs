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
using MysqlFunctions;
using WebApiPoster.Models;
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
        private string strConnectionString = DbConnect.GetFixedConnectionString(ConfigurationManager.ConnectionStrings["localSQL"].ToString());
        private static string ParseUserName = "TestUser";
        private static string ParsePassword = "TestPass";

        public IHttpActionResult Get()
        {
            return Ok();
        }
        // localhost:api/CEMS/GetCads
        [HttpGet]
        public string GetCads(string device_id = "%", string cad_id = "%")
        {
            if (device_id == "%" && cad_id == "%")
                return "";
            ExtendedDataTable dt = GetCadTable(device_id, cad_id);
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
        public string GetCadsRaw(string device_id = "%", string cad_id = "%")
        {
            ExtendedDataTable dt = GetCadTable(device_id, cad_id);
            List<object> myList = new List<object>();
            foreach (DataRow dr in dt.Rows)
            {
                myList.Add(dr.ItemArray);
            }

            string json = JsonConvert.SerializeObject(myList);
            return json;
        }

        [HttpGet]
        public string GetPCR(string pcr_id = "%")
        {
            StringBuilder Pcr_Json = new StringBuilder();
            
            Pcr pcr = new Pcr("PCR",pcr_id);
            Pcr_Json.Append(GetPCRSection(pcr) + System.Environment.NewLine);

            Dispatch Dispatch = new Dispatch("pcr_Dispatch", pcr.pcr_dispatch_id);
            Pcr_Json.Append("," +GetPCRSection(Dispatch) + System.Environment.NewLine);
            Address address = new Address(Dispatch.address_id,true);
            Pcr_Json.Append("," + GetPCRSection(address) + System.Environment.NewLine);
            Facility facility = new Facility(Dispatch.facility_id, true);
            Pcr_Json.Append("," + GetPCRSection(facility) + System.Environment.NewLine);

            Demographic Demographic = new Demographic("pcr_Demographic", pcr.pcr_demographic_id);
            Pcr_Json.Append("," +GetPCRSection(Demographic) + System.Environment.NewLine);

            Assessment Assessment = new Assessment("pcr_Assessment", pcr.pcr_assessment_id);
            Pcr_Json.Append("," +GetPCRSection(Assessment) + System.Environment.NewLine);

            Narrative_Notes Narrative_Notes = new Narrative_Notes("pcr_Narrative_Notes", pcr.pcr_narrative_notes_id);
            Pcr_Json.Append("," +GetPCRSection(Narrative_Notes) + System.Environment.NewLine);

            Rma Rma = new Rma("pcr_Rma", pcr.pcr_rma_id);
            Pcr_Json.Append("," +GetPCRSection(Rma) + System.Environment.NewLine);

            Apcf Apcf = new Apcf("pcr_Apcf", pcr.pcr_apcf_id);
            Pcr_Json.Append("," +GetPCRSection(Apcf) + System.Environment.NewLine);

            Disposition Disposition = new Disposition("pcr_Disposition", pcr.pcr_disposition_id);
            Pcr_Json.Append("," +GetPCRSection(Disposition) + System.Environment.NewLine);

            ems_run Ems_Run = new ems_run("Ems_Run", pcr.ems_run);
            Pcr_Json.Append("," +GetPCRSection(Ems_Run) + System.Environment.NewLine);

            Narcotic Narcotic = new Narcotic("pcr_Narcotic", pcr.pcr_narcotics_id);
            Pcr_Json.Append("," +GetPCRSection(Narcotic) + System.Environment.NewLine);

            Models.Authorization Authorization = new Models.Authorization("pcr_Authorization", pcr.pcr_authorization_id);
            Pcr_Json.Append("," + GetPCRSection(Authorization));

            
            
            
            
            
            //Pcr_Json.Append(GetPCRSection("PCR", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Dispatch", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Demographic", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Assessment", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Narrative_Notes", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Rma", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Apcf", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Disposition", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Ems_Run", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Narcotic", pcr_id));
            //Pcr_Json.Append(System.Environment.NewLine);
            //Pcr_Json.Append("," + GetPCRSection("Authorization", pcr_id));
            return Pcr_Json.ToString ();
        }

        public string GetPCRSection(dynamic obj)
            {
            try
                {

                //Assembly Asm = Assembly.Load(Assembly.GetExecutingAssembly().FullName);
                //Type PCRSectionType = Asm.GetTypes().First(t => t.Name.ToUpper() == ClasssName.ToUpper());
                //dynamic objSectionOut = Activator.CreateInstance(PCRSectionType, TableName, PcrSection);
               // List<Pcr> myList =  obj.GetType().GetProperties().ToList(); //obj.GetType().GetProperties().ToList(); // new List<object>();

                //foreach (PropertyInfo prop in obj.GetType().GetProperties())
                //    {
                //    myList.Add(prop);
                //    }
                string json = "\"" + obj.GetTableName() + "\": " + JsonConvert.SerializeObject(obj);
                return json;
                }
            catch (Exception ex)
                {
                return ErrorLog.LogException(ex);
                }
            }

        public string GetPCRSection(string table_name, string pcr_id = "%")
        {
        try
            {
            ExtendedDataTable dt;
            if (table_name == "PCR")
                dt = GetPcrTable(pcr_id);
            else
                dt = GetPcrDetailTable(table_name, pcr_id);

            List<object> myList = dt.toList(); // new List<object>();

            string json = "\"" + table_name + "\": " + JsonConvert.SerializeObject(myList);
            return json;
            }
        catch (Exception ex)
            {
            return ErrorLog.LogException(ex);
            }
        }




        private void UpdateDownloadedCad(string device_id = "%", string cad_id = "%")
        {
            try
            {
                using (MySqlConnection cn = new MySqlConnection(strConnectionString))
                {
                    cn.Open();
                    string UpdateSql = "update cad_number set downloaded_time = CURRENT_TIMESTAMP() where (ifnull(downloaded_time,0)=0 or year(downloaded_time)=1970) and id like '" + cad_id + "' and agency_id like '" + device_id + "'";
                    MySqlCommand cmd = new MySqlCommand(UpdateSql, cn);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ErrorLog.LogException(ex);
            }
        }
        private ExtendedDataTable GetPcrTable(string pcr_id)
        {
            ExtendedDataTable dt = new ExtendedDataTable();
            try
            {
                using (MySqlConnection cn = new MySqlConnection(strConnectionString))
                {
                    cn.Open();

                    string QueryString = "SELECT * FROM PCR" + System.Environment.NewLine +
                                        " where id = '" + pcr_id + "'";
                    MySqlCommand cmd = new MySqlCommand(QueryString, cn);
                    dt.Load(cmd.ExecuteReader());
                }
            }
            catch (Exception ex) { ErrorLog.LogException(ex); return null; }
            return dt;
        }

        private ExtendedDataTable GetPcrDetailTable(string DetailSuffix, string pcr_id)
        {
            ExtendedDataTable dt = new ExtendedDataTable();
            try
            {
                using (MySqlConnection cn = new MySqlConnection(strConnectionString))
                {
                    cn.Open();
                    string TableName = (DetailSuffix != "Ems_Run" ? "Pcr_" : "") + DetailSuffix;
                    string IDFieldName = TableName + (DetailSuffix != "Ems_Run" ? DetailSuffix != "Narcotic"? "_id":"s_id" :"");

                    string QueryString = "SELECT d.* FROM PCR p inner join " + TableName + " d " + System.Environment.NewLine +
                                         "on  p." + IDFieldName + " = d.id" + System.Environment.NewLine +
                                         "where p.id = '" + pcr_id + "'";
                    MySqlCommand cmd = new MySqlCommand(QueryString, cn);
                    dt.Load(cmd.ExecuteReader());
                }
            }
            catch (Exception ex) { ErrorLog.LogException(ex); return null; }
            return dt;
        }

        //private ExtendedDataTable GetAddressTable(string DetailSuffix, string pcr_id)
        //    {
        //    ExtendedDataTable dt = new ExtendedDataTable();
        //    try
        //        {
        //        using (MySqlConnection cn = new MySqlConnection(strConnectionString))
        //            {
        //            cn.Open();
        //            string TableName = (DetailSuffix != "Ems_Run" ? "Pcr_" : "") + DetailSuffix;
        //            string IDFieldName = TableName + (DetailSuffix != "Ems_Run" ? DetailSuffix != "Narcotic" ? "_id" : "s_id" : "");

        //            string QueryString = "SELECT d.* FROM PCR p inner join " + TableName + " d " + System.Environment.NewLine +
        //                                 "on p." + IDFieldName + " = d.id" + System.Environment.NewLine +
        //                                 "where p.id = '" + pcr_id + "'";
        //            MySqlCommand cmd = new MySqlCommand(QueryString, cn);
        //            dt.Load(cmd.ExecuteReader());
        //            }
        //        }
        //    catch (Exception ex) { ErrorLog.LogException(ex); return null; }
        //    return dt;
        //    }

        private ExtendedDataTable GetCadTable(string device_id = "%", string cad_id = "%")
        {
            ExtendedDataTable dt = new ExtendedDataTable();
            //List<object> obj = new List<object>();
            try
            {
                using (MySqlConnection cn = new MySqlConnection(strConnectionString))
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
                                             "where a.id like '" + cad_id + "' and a.agency_id like '" + device_id + "'" +
                                             " and (ifnull(downloaded_time,0)=0 or year(downloaded_time)=1970)";
                    //where a.id = '70d9fafb-64da-4eb3-b3d8-f99950952474' and and a.agency_id = '90b16e1c-aa76-11e5-b94a-842b2b4bbc99';
                    MySqlCommand cmd = new MySqlCommand(QueryString, cn);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@cad_id", cad_id);
                    //cmd.Parameters.AddWithValue("@device_id", device_id);
                    dt.Load(cmd.ExecuteReader());

                    //MySqlDataAdapter Adapter;
                    //Adapter = new MySqlDataAdapter(cmd);
                    //MySqlCommandBuilder cb = new MySqlCommandBuilder(Adapter);
                    //Adapter.Fill(dt);


                }
            }
            catch (Exception ex) { ErrorLog.LogException(ex); return null; }
            return dt;

        }


        [HttpPost]
        public Boolean PostJsonToSql([FromBody] object JsonData)
        {
            try
            {
                //var javaScriptSerializer = new JavaScriptSerializer();
                //List<PostedDataRow> root1 = javaScriptSerializer.Deserialize<List<PostedDataRow>>(JsonData.ToString ());
                // string JsonData = System.IO.File.ReadAllText(JsonFileName);
                //	List<object> JsonObject = (List<object>)JsonConvert.DeserializeObject(JsonData.ToString(), typeof(List<object>));
                //List<PostedDataRow> JsonList = (List<PostedDataRow>)JsonConvert.DeserializeObject(JsonData.ToString(), typeof(List<PostedDataRow>));
                JObject JsonObject = (JObject)JsonConvert.DeserializeObject(JsonData.ToString());
                ParseAndPostJson(JsonObject);
                //List<PostedDataRow> rownew = new List<PostedDataRow>();

                //rownew = (JArray)JsonObject;
                //	SendJsonToParse();
                return true;
            }
            catch (Exception ex) { ErrorLog.LogException(ex); return false; }

        }


        public void ParseAndPostJson(JObject JsonObject)
        {
        try { 
            DataTable table = new DataTable();
            string TableName;
            foreach (JToken token in JsonObject.SelectToken(""))
            {
                
                //if (!TableName.ToLower().StartsWith("pcr")) TableName = "pcr_" + TableName;
                //DataRow row = table.NewRow();
                //Assembly Asm = Assembly.Load(Assembly.GetExecutingAssembly().FullName);
                //Type PCRSectionType = Asm.GetTypes().First(t => t.Name.ToUpper() == TableName.ToUpper());
                //var PCRVar = Activator.CreateInstance(PCRSectionType);
                JsonInputSection PcrSection = new JsonInputSection();
                foreach (JObject Fields in token)
                    {
                    foreach (JToken Field in Fields.Children())
                        {
                        var Property = Field as JProperty;
                        PcrSection[Property.Name] = Property.Value.ToString();
                        //PCRVar.GetType().GetProperty(Property.Name).SetValue(PCRVar, Property.Value.ToString());
                        //if (!table.Columns.Contains(Property.Name))
                        //    table.Columns.Add(Property.Name, typeof(System.String));
                        //row[Property.Name] = Property.Value;
                        }

                    }
                //table.Rows.Add(row);

                
                switch (token.Path.ToLower ())
                    {
                    case "pcr":
                    case "ems_run":
                        TableName = token.Path.ToString();
                        break;
                    default:
                        TableName = "pcr_" + token.Path.ToString().ToLower ();
                        break;
                }
                Assembly Asm = Assembly.Load(Assembly.GetExecutingAssembly().FullName);
                Type PCRSectionType = Asm.GetTypes().First(t => t.Name.ToUpper() == token.Path.ToUpper());
                dynamic objSectionOut = Activator.CreateInstance(PCRSectionType, TableName, PcrSection);
                objSectionOut.HandleRecord();
           }
                    
              

                
            }
            catch (Exception ex) { ErrorLog.LogException(ex); }
                //switch (TableName )
                //{
                //    case "PCR":

                //        case "Dispatch":
                //}
                //PcrRow pcr=new PcrRow() ;
                //JToken WorkItem;
                //if (token.SelectToken("Table") != null)
                //    WorkItem = token.SelectToken("Table");
                //else
                //    WorkItem = token.SelectToken("");
                //foreach (var prop in pcr.GetType().GetProperties())
                //{
                //    prop.SetValue (pcr, )
                //    Console.WriteLine("{0}={1}", prop.Name, prop.GetValue(pcr, null));
                //}
               
               // PostRecordToSql(PCRVar, TableName);
            //}

            //return table;

        }


        //public void PostRecordToSql(object objPCR, string DestTableName)
        //{
        //    try
        //    {

        //        string strSQL = ConfigurationManager.ConnectionStrings["localSQL"].ToString();
        //        using (MySqlConnection cn = new MySqlConnection(strSQL))
        //        {
        //            cn.Open();

                 

        //            switch (objPCR.GetType().Name)
        //            {
        //                case "Pcr":
        //                    Pcr objPcrOut = (Pcr)objPCR;
        //                    objPcrOut.HandleRecord();
        //                    break;
        //                case "Dispatch":
        //                    DispatchOut objDispatchOut = new DispatchOut((Dispatch)objPCR);
        //                    objDispatchOut.HandleRecord();
        //                    break;
        //            }
                    
        //}
				
        //    }
        //    catch (Exception ex) { }
        //}
     
     
   

//protected Boolean SendJsonToParse()
//{


//    try
//    {
//        ParseObject user = ParseObject.Create("Parse.ParseUser"); // new ParseUser();
//                                                                  //user.Username = ParseUserName;
//                                                                  //user.Password = ParsePassword;


//        //	user.setPassword(mPassword);
//        //	user.signUp();
//        //		ParseQuery<ParseUser> query = ParseUser.GetQuery();
//        //query.whereEqualTo("gender", "female");
//        //		ParseQuery<ParseUser> query = ParseUser.Query.WhereEqualTo("username", ParseUserName);
//        //		Task objects =  query.FindAsync();
//        //		query.FindAsync(new FindCallback<ParseUser>() {
//        //			  public void done(List<ParseUser> objects, ParseException e) {
//        //			    if (e == null) {
//        //				   // The query was successful.
//        //			    } else {
//        //				   // Something went wrong.
//        //			    }
//        //			  }
//        //			});
//        //		query.FindAsync()
//        //		  public void done(List<ParseUser> objects, ParseException e) {
//        //		    if (e == null) {
//        //			   // The query was successful.
//        //		    } else {
//        //			   // Something went wrong.
//        //		    }
//        //		  }
//        //		});
//        //ParseUser user = ParseObject.Create <ParseUser>(); 
//        //user = ParseObject.Create();
//        // Parse: Look up given user
//        //	L.d("UserLoginTask::doInBackground - checking if user '%s' exists", mUser);
//        //ParseQuery<ParseUser> query = ParseUser.GetQuery(); //ParseUser.class).whereEqualTo("username", mUser);
//        //ParseQuery<ParseUser> query = ParseUser.GetQuery("UserName");
//        //ParseQuery<ParseUser> query = ParseQuery.GetQuery(user.ClassName);
//        ParseQuery<ParseObject> Query = Parse.ParseObject.GetQuery(user.ClassName).WhereEqualTo("username", ParseUserName);

//        var Task = ParseUser.Query.WhereExists("facebookID").FindAsync().ContinueWith(t =>
//        {

//            if (t.IsFaulted || t.IsCanceled)
//            {
//                Console.WriteLine("t.Exception=" + t.Exception);
//                //cannot load friendlist
//                return;
//            }
//            else
//            {
//                //fbFriends = t.Result;

//                //foreach (var result in fbFriends)
//                //{
//                //	string id = (string)result["facebookID"];

//                //	//facebookUserIDList is a List<string>
//                //	facebookUserIDList.Add(id);
//                //}

//                return;
//            }
//        });
//        Task task = Query.FindAsync();
//        //	ParseQuery<ParseUser> UserQuery = ParseQuery<ParseUser> Query.WhereEqualTo("username", ParseUserName);

//        //if (query. == 0) {
//        //	L.i("UserLoginTask::doInBackground - doesn't exist - signing up");
//        //	// Parse: User doesn't exist - create
//        //	user = new ParseUser();
//        //	user.s(mUser);
//        //	user.setPassword(mPassword);
//        //	user.signUp();
//        //} else {
//        //	L.d("UserLoginTask::doInBackground - exists - verifying password");
//        //	// Parse: User exists - verify password
//        //	ParseUser.logIn(mUser, mPassword);
//        //L.d("UserLoginTask::doInBackground - done!");

//        return true;
//    }
//    catch (ParseException e)
//    {
//        //L.e(e, "Authentication error");
//        return false;
//    }
//}

//        //public Boolean  SendJsonToParse(string JsonData)
//        //{
//        //	try
//        //	{
//        //		ParseUser User=new ParseUser();
//        //		User.Username = ParseUserName;
//        //		User.Password  = ParsePassword;
//        //		return true;
//        //	}
//        //	catch (Exception ex)
//        //	{
//        //		return false;
//        //	}
//        //}



	}

}
