using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class Rma :PcrBase 
        {


        public string rma_text { get; set; }
        public string medical_assistance_refused { get; set; }
        public string signature { get; set; }
        public string witness_signature { get; set; }
        public string witness_name { get; set; }
        public string patient_legal_guardian { get; set; }
        public text_blocks rma_text_object { get; set; }
        
        public Rma(string id):base(id,"pcr_rma")
            {
             if (!string.IsNullOrEmpty(rma_text)) rma_text_object = new text_blocks(rma_text);
            }
        public Rma(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            this.id = PcrObj["id"];
            this.rma_text = PcrObj["rma_text"];
            this.medical_assistance_refused = PcrObj["medical_assistance_refused"];
            this.signature = PcrObj["signature"];
            this.witness_signature = PcrObj["witness_name"];
            this.witness_name = PcrObj["crew_member_name"];
            this.patient_legal_guardian = PcrObj["patient_legal_guardian"];
            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {
            if (rma_text != null) Utilities.ValidateField("text_blocks", rma_text);

            }
        }
    }