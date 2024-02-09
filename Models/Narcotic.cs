using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Narcotic:BaseClass 
        {
  
        public string call_location { get; set; }
        public string patient_name { get; set; }
        public string patient_address { get; set; }
        public string patient_csz { get; set; }
        public string patient_ssn { get; set; }
        public string chief_complaint { get; set; }
        public string receiving_hospital { get; set; }
        public string disp_code { get; set; }
        public string ed_chart { get; set; }
        public string paramedic_name { get; set; }
        public string paramedic_signature { get; set; }
        public string witness_name { get; set; }
        public string witness_signature { get; set; }
        public string midazolam_wasted { get; set; }
        public string diazepam_wasted { get; set; }
        public string lorazepam_wasted { get; set; }
        public string morphine_sulfate_wasted { get; set; }
        public string fentanyl_wasted { get; set; }
        public string midazolam_number { get; set; }
        public string diazepam_number { get; set; }
        public string lorazepam_number { get; set; }
        public string morphine_sulfate_number { get; set; }
        public string fentanyl_number { get; set; }
        public string number { get; set; }
        public Address call_location_object { get; set; }
        public Address patient_address_object { get; set; }
        public Person patient_name_object { get; set; }

        private void InitObjects()
            {
            if (!string.IsNullOrEmpty(call_location)) call_location_object = new Address(call_location, true);
            if (!string.IsNullOrEmpty(patient_address)) patient_address_object = new Address(patient_address, true);
            if (!string.IsNullOrEmpty(patient_name)) patient_name_object = new Person(patient_name);

            }

        public Narcotic(string id):base(id,"pcr_Narcotic")
            {
            InitObjects();
            }
        public Narcotic(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            this.id = PcrObj["id"];

            string call_location = PcrObj["call_location"];
            this.call_location = String.IsNullOrEmpty(call_location) ? null : call_location;

            string patient_name = PcrObj["patient_name"];
            this.patient_name = String.IsNullOrEmpty(patient_name) ? null : patient_name;


            string patient_address = PcrObj["patient_address"];
            this.patient_address = String.IsNullOrEmpty(patient_address) ? null : patient_address;

            this.patient_csz = PcrObj["patient_csz"];
            this.patient_ssn = PcrObj["patient_ssn"];
            this.chief_complaint = PcrObj["chief_complaint"];
            this.receiving_hospital = PcrObj["receiving_hospital"];
            this.disp_code = PcrObj["disp_code"];
            this.ed_chart = PcrObj["ed_chart"];
            this.paramedic_name = PcrObj["paramedic_name"];
            this.paramedic_signature = PcrObj["paramedic_signature"];
            this.witness_name = PcrObj["witness_name"];
            this.witness_signature = PcrObj["witness_signature"];
            this.midazolam_wasted = PcrObj["midazolam_wasted"];
            this.diazepam_wasted = PcrObj["diazepam_wasted"];
            this.lorazepam_wasted = PcrObj["lorazepam_wasted"];
            this.morphine_sulfate_wasted = PcrObj["morphine_sulfate_wasted"];
            this.fentanyl_wasted = PcrObj["fentanyl_wasted"];
            this.midazolam_number = PcrObj["midazolam_number"];
            this.diazepam_number = PcrObj["diazepam_number"];
            this.lorazepam_number = PcrObj["lorazepam_number"];
            this.morphine_sulfate_number = PcrObj["morphine_sulfate_number"];
            this.fentanyl_number = PcrObj["fentanyl_number"];
            this.number = PcrObj["number"];
            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {
            if (call_location != null) Utilities.ValidateField("address", call_location);
            
            if (patient_address != null) Utilities.ValidateField("address", patient_address);

            if (patient_name != null) Utilities.ValidateField("person", patient_name);
            
            }
        }
    }