using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace WebApiPoster.Models
    {
    public class Members : BaseClass
        {
        public string pcr_id { get; set; }
        public string user_id { get; set; }
        public string in_charge { get; set; }
        public string driver { get; set; }
        public string transport { get; set; }
        public string documentary { get; set; }
        public string scene_only { get; set; }
        public string active { get; set; }
        public string _user { get; set; }
        public string _last_name { get; set; }
        public string _first_name { get; set; }
        public Users user_object { get; set; }

        public Members() { TableName = "pcr_members"; }
        public Members(string id, string SearchField="id")
            : base(id, "pcr_members", SearchField)
            {
            if (!string.IsNullOrEmpty(user_id)) user_object = new Users(user_id);
            }
        public Members(string TableName, JsonInputSection PcrObj)
            : base(TableName, PcrObj)
            {

            }
        public Members(object JsonData, string PathPrefix)
             
        {
             this.TableName = "pcr_members";
        }
        public static void ClearPcrMembers(string pcr_id)
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "delete from pcr_members where pcr_id = '" + pcr_id + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  cmd.ExecuteNonQuery();
             }
        }
        public bool ModifyArrayItem(string Path)
        {
             try
             {
                  object JsonData = HttpContext.Current.Session["json_out"]; 
                  string JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'"); //.Replace(".", "@");
                  JToken JsonToken = JObject.Parse(@JsonString);
                  JArray jarray = (JArray)JsonToken.SelectToken(Path);
                  string SearchField = "id";
                  string SearchValue = _user;

                  JToken SearchToken = null;
                  if (jarray == null)
                  {
                       JsonMaker.UpdateJsonValue(Path, "[]");
                       JsonData = HttpContext.Current.Session["json_out"]; ;
                       JsonString = JsonData.ToString().Replace("'", @"\'").Replace("\"", "'");
                       JsonToken = JObject.Parse(@JsonString);
                       jarray = (JArray)JsonToken.SelectToken(Path);
                  }
                  //return false;
                  else
                  {
                       SearchToken = jarray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString() == SearchValue);
                  }
               
                  if (SearchToken == null)
                  {

                       JObject ArrayItem = new JObject();
                       ArrayItem[SearchField] = SearchValue;
                       ArrayItem["choice"] = "on";;
                       ArrayItem["img"] = "check-circle-o";
                       ArrayItem["str"] = _last_name + ", " + _first_name;

                       jarray.Add(ArrayItem);

                  }
                  else
                       SearchToken.SelectToken("choice").Replace("on");
                  HttpContext.Current.Session["json_out"] = (JObject)JsonToken;
                  StringBuilder message = new StringBuilder();
                  message.Append("Members.ModifyArrayItem: " + System.Environment.NewLine);
                  message.Append("Path: " + Path + System.Environment.NewLine);
                  message.Append(System.Environment.NewLine);
                  Logger.LogJsonUpdates(message.ToString());
                  return true;
             }
             catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public void MapIntoIOSJson(int index)
        {
             JsonMaker.UpdateJsonValue("$.Crew.MemberButton" + index.ToString() + "@name",_last_name+", "+_first_name);
             JsonMaker.UpdateJsonValue("$.Crew.MemberButton" + index.ToString() + "@badge", _user);
             JsonMaker.UpdateJsonValue("$.Crew.MemberButton" + index.ToString() + "@doc", documentary=="1"?"true":"false");
             JsonMaker.UpdateJsonValue("$.Crew.MemberButton" + index.ToString() + "@scene", scene_only == "1" ? "true" : "false");
             JsonMaker.UpdateJsonValue("$.Crew.MemberButton" + index.ToString() + "@transport", transport == "1" ? "true" : "false");
             JsonMaker.UpdateJsonValue("$.Crew.MemberButton" + index.ToString() + "@level", driver=="1"?"Driver":(in_charge=="1"?"Crew Chief":"Crew Member"));

        }
        public void MapIntoIOSJson(string PathPrefix,int  DriverNo=0)
        {
             //string UserUser=GetUserUserByID();
             //string UserName=GetUserNameByID();
             this.ModifyArrayItem(PathPrefix);
             //this.ModifyArrayItem(PathPrefix, "id", _last_name + ", " + _first_name, "choice", "on");


             if (DriverNo > 0)
             {
                  JsonMaker.UpdateJsonValue("$.Dispatch.Members@Members_M" + DriverNo.ToString(),  _last_name + ", " + _first_name);
             }
             //JsonMaker.UpdateJsonValue(PathPrefix + "_MiddleInitial", mi);
             //JsonMaker.UpdateJsonValue(PathPrefix + "_LastName", last_name);
        }
        public static List<Members> MapFromIOSJson(object JsonData, string pcr_id)
        {
             Members.ClearPcrMembers(pcr_id);
             List<Members> CrewMembersList = new List<Members>();

             string CL_Count = JsonMaker.GetIOSJsonExtract("$.Crew.CrewList.count", JsonData);
             if (CL_Count == null) CL_Count = "0";

             for (int i = 0; i < Convert.ToInt16(CL_Count); i++)
             {
                  string name = JsonMaker.GetIOSJsonExtract("$.Crew.MemberButton" + i.ToString() + "@name", JsonData) ?? "";
                  string badge = JsonMaker.GetIOSJsonExtract("$.Crew.MemberButton" + i.ToString() + "@badge", JsonData) ?? "";
                  string doc = JsonMaker.GetIOSJsonExtract("$.Crew.MemberButton" + i.ToString() + "@doc", JsonData) ?? "";
                  string level = JsonMaker.GetIOSJsonExtract("$.Crew.MemberButton" + i.ToString() + "@level", JsonData) ?? "";
                  string scene = JsonMaker.GetIOSJsonExtract("$.Crew.MemberButton" + i.ToString() + "@scene", JsonData) ?? "";
                  string transport = JsonMaker.GetIOSJsonExtract("$.Crew.MemberButton" + i.ToString() + "@transport", JsonData) ?? "";

                  if (!String.IsNullOrEmpty(badge) || !String.IsNullOrEmpty(name))
                  {
                       Members member = new Members();
                       member.pcr_id = pcr_id;
                       if (String.IsNullOrEmpty(badge) && !String.IsNullOrEmpty(name))
                            member.user_id = Users.GetIDByNameAndAgency(out badge, name, HttpContext.Current.Session["agency_id"].ToString());
                       else if (!String.IsNullOrEmpty(badge))
                            member.user_id = Users.GetIDByNameAndAgency(out badge, badge, HttpContext.Current.Session["agency_id"].ToString());
                       member._user = badge;
                       member.documentary = doc.ToLower() == "true" ? "1" : "0";
                       member.scene_only = scene.ToLower() == "true" ? "1" : "0";
                       member.transport = transport.ToLower() == "true" ? "1" : "0";
                       member.driver = level.ToLower() == "driver" ? "1" : "0";
                       member.in_charge = level.ToLower() == "crew chief" ? "1" : "0";
                       member.HandleRecord();
                       CrewMembersList.Add(member);
                  }
             }

             if (CrewMembersList.Count > 0) return CrewMembersList;


             object WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch", JsonData.ToString());
             JObject JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());


             foreach (JToken token in JsonObject.SelectToken("").Where(t => t.Path.StartsWith("Members.")))
             {
                  Members member = new Members();
                  member.pcr_id = HttpContext.Current.Session["pcr_id"].ToString();
                  string badge;
                  member.user_id = Users.GetIDByNameAndAgency(out badge, JsonMaker.GetIOSJsonExtract("$." + token.Path.Replace("Members.", "Members@"), WorkJson), HttpContext.Current.Session["agency_id"].ToString());
                  member._user = badge;
                  member.HandleRecord();
                  CrewMembersList.Add(member);
                  // }
             }
             return CrewMembersList;


             //WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch.Members.model.buttons", JsonData.ToString());
             //if (WorkJson != null)
             //{
             //     Members.ClearPcrMembers(pcr_id);
             //     JArray MembersArray = (JArray)JsonConvert.DeserializeObject(WorkJson.ToString());

             //     foreach (JToken token in MembersArray)
             //     {
             //          Members member = new Members();
             //          member.pcr_id = pcr_id;
             //          member.MapFromIOSJson((object)token, CrewMembersList);
             //     }
             //}


        }
        public void MapFromIOSJson_old(object JsonData, List<string> DriverList)
        {
             try
             {
                  string user = JsonMaker.GetIOSJsonExtract("$.id", JsonData);
                  string choice = JsonMaker.GetIOSJsonExtract("$.choice", JsonData);
                  string name = JsonMaker.GetIOSJsonExtract("$.str", JsonData);
                  if (user != null )
                  {
                       this.user_id = GetUserIDByUser(user);
                       if (this.user_id==null)
                            this.user_id = GetUserIDByName(name);
                       if (this.user_id != null)
                       {
                            string temp_user_id;
                            foreach (string Driver in DriverList)
                            {
                                 temp_user_id = GetUserIDByName(Driver);
                                 if (temp_user_id == this.user_id)
                                 {
                                      this.driver = "1";
                                 }
                            }
                            if (!String.IsNullOrEmpty(this.pcr_id) && !String.IsNullOrEmpty(this.user_id)) { 
                                 Delete(new string[] { "pcr_id", "user_id" }, new string[] { this.pcr_id, this.user_id });
                                 if (choice+"" =="on") HandleRecord();
                            }
                       }
                  }




             }
             catch (Exception ex) { Logger.LogException(ex); }
        }

        public void MapFromIOSJson_old(object JsonData, List<Members> CrewList)
        {
             try
             {
                  string user = JsonMaker.GetIOSJsonExtract("$.id", JsonData);
                  string choice = JsonMaker.GetIOSJsonExtract("$.choice", JsonData);
                  string name = JsonMaker.GetIOSJsonExtract("$.str", JsonData);
                  if (user != null)
                  {
                       this.user_id = GetUserIDByUser(user);
                       if (this.user_id == null)
                            this.user_id = GetUserIDByName(name);
                       if (this.user_id != null)
                       {
                            string temp_user_id;
                            foreach (Members CrewMember in CrewList)
                            {
                                 temp_user_id = CrewMember.user_id;
                                 if (temp_user_id == this.user_id)
                                 {
                                      this.documentary = CrewMember.documentary;
                                      this.scene_only = CrewMember.scene_only;
                                      this.transport = CrewMember.transport;
                                      this.driver = CrewMember.driver;
                                      this.in_charge = CrewMember.in_charge;
                                 }
                            }
                            if (!String.IsNullOrEmpty(this.pcr_id) && !String.IsNullOrEmpty(this.user_id))
                            {
                                // Delete(new string[] { "pcr_id", "user_id" }, new string[] { this.pcr_id, this.user_id });
                                 if (choice + "" == "on") HandleRecord();
                            }
                       }
                  }




             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapFromIOSJson(object JsonData, int driver_index)
        {
             try
             {

                    string name = JsonMaker.GetIOSJsonExtract("$.Members@Members_M"+driver_index.ToString(), JsonData);
                    if (name != null) {
                         this.user_id = GetUserIDByName(name);
                         this.driver="1";
                         if (this.user_id != null) 
                               HandleRecord();
                    }
                  
                 

                  
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public string GetUserUserByID()
        {

             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select u.badge_number from pcr_members m inner join users u on m.user_id=u.id inner join person p on u.person_id=p.id where user_id = '" + this.user_id + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        public string GetUserNameByID()
        {
  

             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select u.last_name+', '+first_name as name from pcr_members m inner join users u on m.user_id=u.id inner join person p on u.person_id=p.id where user_id = '" + this.user_id + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        public string GetUserIDByUser(string user)
        {

             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select u.id from pcr_members m inner join users u on m.user_id=u.id inner join person p on u.person_id=p.id where user = '" + user.Trim() + "' or  badge_number = '" + user.Trim() + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        public string GetUserIDByName(string name) 
          {
               string last_name = name.Split(',')[0];
               string first_name="";
               if (name.Contains(",")) first_name = name.Split(',')[1];
          
               using (MySqlConnection cn = new MySqlConnection( DbConnect.ConnectionString))
                    {
                    cn.Open();
                    string SqlString = "select u.id from pcr_members m inner join users u on m.user_id=u.id inner join person p on u.person_id=p.id where last_name = '" + last_name.Trim() + "' and first_name = '" + first_name.Trim() +"'";
                    MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                    string rv = cmd.ExecuteScalar()+"";
                    //rv = rv == null ? "" : rv.ToString();
                    return rv; 
                    }
          }
        public void HandleRecord(int InsertUpdate = 0)
            {
            //if (ValidateFields())
                this.ValidateFields();
                this.InsertUpdateAction(InsertUpdate);
            //else
            //    Logger.LogError("Members Table Insert/Update Violation for id # " + this.id);
            }

        public void  ValidateFields()
            {
            Utilities.ValidateField("pcr", pcr_id);
            Utilities.ValidateField("Users", user_id);
            //Boolean rv=true;
            //if (!string.IsNullOrEmpty(pcr_id))
            //    {
            //    rv = Utilities.Exists("PCR", pcr_id);
            //    }
            //else if (!string.IsNullOrEmpty(user_id ))
            //    {
            //    rv = Utilities.Exists("Users", user_id);
            //    }
            //return rv;
            }

        }
    }