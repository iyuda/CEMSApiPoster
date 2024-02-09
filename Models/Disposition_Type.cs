using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;

namespace WebApiPoster.Models
    {
    public class Disposition_Type : BaseClass
        {
        public Disposition_Type()
            {

            }
        public Disposition_Type(string id)
            : base(id, "Disposition_Type")
            {

            }
        public Disposition_Type(string TableName, JsonInputSection PcrObj)
            : base(TableName, PcrObj)
            {

            }

        public string type_name { get; set; }
        public string code { get; set; }
        public string modifier { get; set; }

        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction(InsertUpdate);
            }

        }
    }