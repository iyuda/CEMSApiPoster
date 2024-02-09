using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
namespace WebApiPoster.PCR
    {
    public class Members : PcrBase
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
        public void MapIntoIOSJson(string PathPrefix,int  DriverNo=0)
        {
             //string UserUser=GetUserUserByID();
             //string UserName=GetUserNameByID();
             if (!JsonMaker.ModifyArrayItem(PathPrefix, "id", _user, "choice", "on"))
                  JsonMaker.ModifyArrayItem(PathPrefix, "id", _last_name + ", " + _first_name, "choice", "on");

             //JsonMaker.UpdateJsonValue(PathPrefix + ".id", _user);
             //JsonMaker.UpdateJsonValue(PathPrefix + ".img", "check-circle-o");
             //JsonMaker.UpdateJsonValue(PathPrefix + ".str", _last_name + ", " + _first_name);
             //JsonMaker.UpdateJsonValue(PathPrefix + ".choice", "on");
             if (DriverNo > 0)
             {
                  JsonMaker.UpdateJsonValue("$.Dispatch.Members@Members_M" + DriverNo.ToString(),  _last_name + ", " + _first_name);
             }
             //JsonMaker.UpdateJsonValue(PathPrefix + "_MiddleInitial", mi);
             //JsonMaker.UpdateJsonValue(PathPrefix + "_LastName", last_name);
        }
        public void MapFromIOSJson(object JsonData, List<string> DriverList)
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
                            if (choice+"" =="on")
                              HandleRecord();
                            else
                              Delete(new string[] { "pcr_id", "user_id" }, new string[] { this.pcr_id, this.user_id });
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
                  string rv = (string)cmd.ExecuteScalar();
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
                  string rv = (string)cmd.ExecuteScalar();
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
                  string rv = (string)cmd.ExecuteScalar();
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
                    string rv = (string)cmd.ExecuteScalar();
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