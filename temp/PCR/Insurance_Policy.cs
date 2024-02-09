using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Insurance_Policy:PcrBase 
        {

        public string insurance { get; set; }
        public string policy_no { get; set; }
        public string group_no { get; set; }
        public string policy_holder { get; set; }


        public Insurance_Policy(string TableName, string id) : base(TableName , id)
            {
            this.Retrieve();
            }
      
        public void HandleRecord(int InsertUpdate = 0)
            {
            if (insurance == null) insurance = Guid.NewGuid().ToString();
            if (policy_holder == null) policy_holder = Guid.NewGuid().ToString();
            Utilities.ValidateField("insurance", insurance);
            Utilities.ValidateField("person", policy_holder);
            this.InsertUpdateAction();
            }
        }
        }
    