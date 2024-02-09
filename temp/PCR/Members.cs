using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
namespace WebApiPoster.Models
    {
    public class Members : PcrBase
        {
        public Members(string TableName, string id)
            : base(TableName, id)
            {

            }
        public Members(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties())
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }


        public string pcr_id { get; set; }
        public string user_id { get; set; }
        public string in_charge { get; set; }
        public string driver { get; set; }
        public string transport { get; set; }
        public string documentary { get; set; }
        public string scene_only { get; set; }
        public string active { get; set; }



        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {

            if (!string.IsNullOrEmpty(pcr_id))
                {
                Pcr pcr = new Pcr("Pcr", pcr_id);
                pcr.HandleRecord();
                }
            if (!string.IsNullOrEmpty(user_id ))
                {
                Users user = new Users("Users", user_id);
                user.HandleRecord();
                }

            }

        }
    }