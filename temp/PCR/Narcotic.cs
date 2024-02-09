using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Narcotic:PcrBase 
        {
  
        private Address objAddress;
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

        public Narcotic(string TableName, string id):base(TableName ,id)
            {
            this.Retrieve();
            objAddress = new Address();
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
            if (call_location == null) call_location = Guid.NewGuid().ToString();
            Utilities.ValidateField("address", call_location);
            
            if (patient_address == null) patient_address = Guid.NewGuid().ToString();
            Utilities.ValidateField("address", patient_address);

            if (patient_name == null) patient_name = Guid.NewGuid().ToString();
            Utilities.ValidateField("person", patient_name);
            
            }
        }
    }