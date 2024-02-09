using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Languages : BaseClass
        {


        public string language { get; set; }
        public string code { get; set; }

        public Languages(string id)
            : base(id, "Languages")
            {

            }
        public Languages(string TableName, JsonInputSection PcrObj)
            : base(TableName, PcrObj)
            {

            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction(InsertUpdate);
            }
        }
    }