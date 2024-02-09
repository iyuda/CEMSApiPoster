using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class hipaa_types : PcrBase
        {


        public string type { get; set; }

        public hipaa_types(string id)
            : base(id, "hipaa_types")
            {

            }
        public hipaa_types(string TableName, JsonInputSection PcrObj)
            : base(TableName, PcrObj)
            {

            }
        public void HandleRecord(int InsertUpdate = 0)
            {

            this.InsertUpdateAction(InsertUpdate);
            }
        }
    }