using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Agency : PcrBase 
        {
        public string business_id { get; set; }

        public Agency(string TableName, string id):base(TableName ,id)
            {
            this.Retrieve();
            }
      
        public void HandleRecord(int InsertUpdate = 0)
            {
            if (business_id == null) business_id = Guid.NewGuid().ToString();
            Utilities.ValidateField("business", business_id);
            this.InsertUpdateAction(InsertUpdate);
            }
        }
    }