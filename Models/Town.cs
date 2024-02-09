using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Town : BaseClass 
        {
  
        public string town_name { get; set; }
        public string location_code { get; set; }


        public Town(string id)
             : base(id, "town")
            {

            }
        public Town(string TableName, JsonInputSection PcrObj):base(TableName , PcrObj)
            {

            }
        public void MapIntoIOSJson(string PathPrefix)
        {
             JsonMaker.UpdateJsonValue(PathPrefix + "_Borough", town_name);
             JsonMaker.UpdateJsonValue(PathPrefix + "_LocationCode", location_code);
        }
        public Town(object JsonData, string PathPrefix)
             
        {
             this.TableName = "town";
             town_name = JsonMaker.GetIOSJsonExtract(PathPrefix + "_Borough", JsonData);
             location_code = JsonMaker.GetIOSJsonExtract(PathPrefix + "_LocationCode", JsonData);
             if (location_code == null) return;
             if (location_code.Length>10)
                  location_code = location_code.Substring(0, 10);
          }
        public void HandleRecord(int InsertUpdate = 0)
            {
               if (this.id + "" == "") this.id = this.GetIDByName();
               this.InsertUpdateAction(InsertUpdate);
            }

        public string GetIDByName()
        {


             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from town where town_name = '" + this.town_name + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar() + "";
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        }

    }