using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Demographic:PcrBase 
        {

        public string pt_person { get; set; }
        public string emr_contact_person { get; set; }
        public string phy_contact_person { get; set; }
        public string primary_policy { get; set; }
        public string secondary_policy { get; set; }
        public string terciary_insurance { get; set; }
        public string agency_id { get; set; }


        public Demographic(string TableName, string id):base(TableName ,id)
            {
            this.Retrieve();
            }
        public Demographic(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            this.id = PcrObj["id"];

            string pt_person = PcrObj["pt_person"];
            this.pt_person = String.IsNullOrEmpty(pt_person) ? null : pt_person;

            string emr_contact_person = PcrObj["emr_contact_person"];
            this.emr_contact_person = String.IsNullOrEmpty(emr_contact_person) ? null : emr_contact_person;

            string phy_contact_person = PcrObj["phy_contact_person"];
            this.phy_contact_person = String.IsNullOrEmpty(phy_contact_person) ? null : phy_contact_person;

            string primary_policy = PcrObj["primary_policy"];
            this.primary_policy = String.IsNullOrEmpty(primary_policy) ? null : primary_policy;

            string secondary_policy = PcrObj["secondary_policy"];
            this.secondary_policy = String.IsNullOrEmpty(secondary_policy) ? null : secondary_policy;

            string terciary_insurance = PcrObj["terciary_insurance"];
            this.terciary_insurance = String.IsNullOrEmpty(terciary_insurance) ? null : terciary_insurance;

            string agency_id = PcrObj["agency_id"];
            this.agency_id = String.IsNullOrEmpty(agency_id) ? null : agency_id;

            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            
            }
        public void ValidateFields()
            {
            if (pt_person != null) Utilities.ValidateField("person", pt_person);
            if (emr_contact_person != null) Utilities.ValidateField("person", emr_contact_person);
            if (phy_contact_person != null) Utilities.ValidateField("person", phy_contact_person);

            Insurance_Policy insurance_policy;
            if (!string.IsNullOrEmpty(primary_policy))
                {
                insurance_policy = new Insurance_Policy("insurance_policy", primary_policy);
                insurance_policy.HandleRecord();
                }
            if (!string.IsNullOrEmpty(secondary_policy))
                {
                insurance_policy = new Insurance_Policy("insurance_policy", secondary_policy);
                insurance_policy.HandleRecord();
                }
            if (!string.IsNullOrEmpty(terciary_insurance))
                {
                insurance_policy = new Insurance_Policy("insurance_policy", terciary_insurance);
                insurance_policy.HandleRecord();
                }
            if (!string.IsNullOrEmpty(agency_id))
                {
                Agency agency = new Agency("agency", agency_id);
                agency.HandleRecord();
                }


            }
        }
    }