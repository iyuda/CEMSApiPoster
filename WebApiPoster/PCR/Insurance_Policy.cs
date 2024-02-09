using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApiPoster.PCR
    {
    public class Insurance_Policy:PcrBase 
        {

        public string insurance { get; set; }
        public string policy_no { get; set; }
        public string group_no { get; set; }
        public string policy_holder { get; set; }
 
        public Insurance insurance_object { get; set; }
        public Person policy_holder_object { get; set; }

        private void InitObjects()
            {
            if (!string.IsNullOrEmpty(insurance)) insurance_object = new Insurance(insurance);
            if (!string.IsNullOrEmpty(policy_holder)) policy_holder_object = new Person(policy_holder);

            }

        public Insurance_Policy(string id) : base(id,"insurance_policy")
            {
            InitObjects();
            }
        public Insurance_Policy(string TableName, JsonInputSection PcrObj)
            : base(TableName, PcrObj)
            {
            InitObjects();
            }

        public void MapIntoIOSJson(string PathPrefix)
        {
             JsonMaker.UpdateJsonValue(PathPrefix + "_PolicyID", policy_no);
             JsonMaker.UpdateJsonValue(PathPrefix + "_GroupNumber", group_no);

             insurance_object = new Insurance(this.insurance);
             insurance_object.MapIntoIOSJson(PathPrefix);
             

        }
        public Insurance_Policy(object JsonData, string PathPrefix)
        {

          this.TableName = "insurance_policy";
          policy_no = JsonMaker.GetIOSJsonExtract("$." + PathPrefix + "_PolicyID", JsonData);
          group_no = JsonMaker.GetIOSJsonExtract("$."+ PathPrefix +"_GroupNumber", JsonData);
          insurance_object = new Insurance(JsonData, PathPrefix);
          //policy_holder_object = new Person(JsonData, "PatientInfo@PatientInfo", true);

          if (insurance_object.name != null)
          {
               insurance = insurance_object.id;
               insurance_object.HandleRecord();
          }
          //if (policy_holder_object.last_name != null)
          //{
          //     policy_holder = policy_holder_object.id;
          //     policy_holder_object.HandleRecord();
          //}
         if (policy_no != null) HandleRecord();

        }
        public void HandleRecord(int InsertUpdate = 0)
            {
            if (insurance != null) Utilities.ValidateField("insurance", insurance);
            if (policy_holder != null) Utilities.ValidateField("person", policy_holder);
            this.InsertUpdateAction();
            }
        }
        }
    