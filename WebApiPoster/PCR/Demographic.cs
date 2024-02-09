using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace WebApiPoster.PCR
    {
    public class Demographic : PcrBase
        {

        public string pt_person { get; set; }
        public string emr_contact_person { get; set; }
        public string phy_contact_person { get; set; }
        public string primary_policy { get; set; }
        public string secondary_policy { get; set; }
        public string terciary_insurance { get; set; }
        public string agency_id { get; set; }

        public Person pt_person_object { get; set; }
        public Person emr_contact_person_object { get; set; }
        public Person phy_contact_person_object { get; set; }
        public Insurance_Policy primary_policy_object { get; set; }
        public Insurance_Policy secondary_policy_object { get; set; }
        public Insurance_Policy terciary_insurance_object { get; set; }
        public Agency agency_object { get; set; }

        private void InitObjects(bool Assign =false)
            {
                 if (pt_person != null || Assign== true) pt_person_object = new Person(pt_person);
                 if (emr_contact_person != null || Assign== true) emr_contact_person_object = new Person(emr_contact_person);
                 if (phy_contact_person != null || Assign== true) phy_contact_person_object = new Person(phy_contact_person);

                 if (primary_policy != null || Assign== true) primary_policy_object = new Insurance_Policy(primary_policy);
                 if (secondary_policy != null || Assign== true) secondary_policy_object = new Insurance_Policy(secondary_policy);
                 if (terciary_insurance != null || Assign== true) terciary_insurance_object = new Insurance_Policy(terciary_insurance);
                 if (agency_id != null || Assign== true) agency_object = new Agency(agency_id);
            }
        public Demographic(string id="")
            : base(id, "pcr_Demographic")
            {
            InitObjects();
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

            InitObjects();
            }
        public Demographic()
        {
             this.TableName = "pcr_Demographic";
           //  InitObjects(true);
         //    MapFromIOSJson(JsonData);
        }
        public void MapIntoIOSJson()
        {
             try
             {
                  pt_person_object.MapIntoIOSJson("$.Demographics.PatientInfo@PatientInfo", true);
                  emr_contact_person_object.MapIntoIOSJson("$.Demographics.EmergencyContact@EmergencyContact");
                  phy_contact_person_object.MapIntoIOSJson("$.Demographics.PhysicianInfo@PhysicianInfo");

                  primary_policy_object.MapIntoIOSJson("$.Demographics.PrimaryInsurance@PrimaryInsurance");
                  secondary_policy_object.MapIntoIOSJson("$.Demographics.SecondaryInsurance@SecondaryInsurance");
                  terciary_insurance_object.MapIntoIOSJson("$.Demographics.TerciaryInsurance@TerciaryInsurance");
                  
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapFromIOSJson(object JsonData, string agency_id)
        {
             try
             {
               pt_person_object = new Person(JsonData, "PatientInfo@PatientInfo", true);
               emr_contact_person_object = new Person(JsonData, "EmergencyContact@EmergencyContact");
               phy_contact_person_object = new Person(JsonData, "PhysicianInfo@PhysicianInfo");
               primary_policy_object = new Insurance_Policy(JsonData, "PrimaryInsurance@PrimaryInsurance");
               secondary_policy_object = new Insurance_Policy(JsonData, "SecondaryInsurance@SecondaryInsurance");
               terciary_insurance_object = new Insurance_Policy(JsonData, "TerciaryInsurance@TerciaryInsurance");

               this.pt_person = pt_person_object.id;
               this.emr_contact_person = emr_contact_person_object.id;
               this.phy_contact_person = phy_contact_person_object.id;
               this.primary_policy = primary_policy_object.id;
               this.secondary_policy = secondary_policy_object.id;
               this.terciary_insurance = secondary_policy_object.id;
               this.agency_id = agency_id;
               //string agency_id = PcrObj["agency_id"];
               //this.agency_id = String.IsNullOrEmpty(agency_id) ? null : agency_id;
               HandleRecord();
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }


        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);

            }
      
        public void ValidateFields()
            {
            if (pt_person_object != null) pt_person_object.HandleRecord();
            if (emr_contact_person_object != null) emr_contact_person_object.HandleRecord();
            if (phy_contact_person_object != null) phy_contact_person_object.HandleRecord();

            if (primary_policy_object != null) primary_policy_object.HandleRecord();
            if (secondary_policy_object != null) secondary_policy_object.HandleRecord();
            if (terciary_insurance_object != null) terciary_insurance_object.HandleRecord();

            if (agency_object != null) agency_object.HandleRecord();

            //if (pt_person != null) Utilities.ValidateField("person", pt_person);
            //if (emr_contact_person != null) Utilities.ValidateField("person", emr_contact_person);
            //if (phy_contact_person != null) Utilities.ValidateField("person", phy_contact_person);

            //Insurance_Policy insurance_policy;
            //if (!string.IsNullOrEmpty(primary_policy))
            //    {
            //    insurance_policy = new Insurance_Policy(primary_policy);
            //    insurance_policy.HandleRecord();
            //    }
            //if (!string.IsNullOrEmpty(secondary_policy))
            //    {
            //    insurance_policy = new Insurance_Policy(secondary_policy);
            //    insurance_policy.HandleRecord();
            //    }
            //if (!string.IsNullOrEmpty(terciary_insurance))
            //    {
            //    insurance_policy = new Insurance_Policy(terciary_insurance);
            //    insurance_policy.HandleRecord();
            //    }
            //if (!string.IsNullOrEmpty(agency_id))
            //    {
            //    Agency agency = new Agency(agency_id);
            //    agency.HandleRecord();
            //    }


            }
        }
    }