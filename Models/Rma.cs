using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Rma :BaseClass 
        {


        public string rma_text { get; set; }
        public string medical_assistance_refused { get; set; }
        public string signature { get; set; }
        public string witness_signature { get; set; }
        public string witness_name { get; set; }
        public string patient_legal_guardian { get; set; }
        public text_blocks rma_text_object { get; set; }

        public Rma()
        {
             this.TableName = "pcr_rma";

        }
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

        public void MapIntoIOSJson()
        {
             try
             {
                  JsonMaker.UpdateJsonValue("$.RMA.signer.signer", witness_name);
                  JsonMaker.UpdateJsonValue("$.RMA.Sig1_Encoding.Sig1_Encoding", signature);
                  JsonMaker.UpdateJsonValue("$.RMA.Sig2_Encoding.Sig2_Encoding", witness_signature);   

             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapFromIOSJson(object JsonData)
        {

             try
             {
                  witness_name = JsonMaker.GetIOSJsonExtract("$.signer.signer", JsonData);
                  signature = JsonMaker.GetIOSJsonExtract("$.Sig1_Encoding.Sig1_Encoding", JsonData);
                  witness_signature = JsonMaker.GetIOSJsonExtract("$.Sig2_Encoding.Sig2_Encoding", JsonData);
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
            if (rma_text != null) Utilities.ValidateField("text_blocks", rma_text);

            }
        }
    }