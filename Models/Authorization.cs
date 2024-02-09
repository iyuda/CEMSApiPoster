﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Authorization:BaseClass 
        {
  //      private Address objAddress;
        public string xml_patient_signature { get; set; }
        public string xml_witness_signature { get; set; }
        public string witness_name { get; set; }
        public string reason_patient_incapable { get; set; }
        public string repres_signature { get; set; }
        public string repres_name { get; set; }
        public string reason_patient_incapable2 { get; set; }
        public string time_at_facility { get; set; }
        public string name_of_facility { get; set; }
        public string name_crewmember { get; set; }
        public string name_facility_repres { get; set; }
        public string signature_facility_repres { get; set; }
        public string signature_crewmember { get; set; }
        public string patient_legal_guardian { get; set; }
        public string patient_health_care_poa { get; set; }
        public string relative_benefits { get; set; }
        public string relative_affairs { get; set; }
        public string repres_agency { get; set; }
        public string patient_care_report { get; set; }
        public string facility_face_sheet { get; set; }
        public string medical_record { get; set; }
        public string facility_record { get; set; }
        public string auth_text { get; set; }
        public string type { get; set; }
        public string patient_repres { get; set; }
        public Person name_crewmember_object { get; set; }
        public Business name_of_facility_object { get; set; }
        public hipaa_types type_object { get; set; }
        public text_blocks auth_text_object { get; set; }

        private void InitObjects()
            {
            if (!string.IsNullOrEmpty(name_crewmember)) name_crewmember_object = new Person(name_crewmember);
            if (!string.IsNullOrEmpty(name_of_facility)) name_of_facility_object = new Business(name_of_facility);
            if (!string.IsNullOrEmpty(type)) type_object = new hipaa_types(type);
            if (!string.IsNullOrEmpty(auth_text)) auth_text_object = new text_blocks(auth_text);
            }
        public Authorization()
        {
             this.TableName = "pcr_Authorization";

        }
        public Authorization(string id):base(id,"pcr_Authorization")
            {
            InitObjects();

            }
        public Authorization(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            this.id = PcrObj["id"];
            this.xml_patient_signature = PcrObj["xml_patient_signature"];
            this.xml_witness_signature = PcrObj["xml_witness_signature"];
            this.witness_name = PcrObj["witness_name"];
            this.reason_patient_incapable = PcrObj["reason_patient_incapable"];
            this.repres_name = PcrObj["repres_name"];
            this.reason_patient_incapable2 = PcrObj["reason_patient_incapable2"];
            this.time_at_facility = PcrObj["time_at_facility"];
            this.name_of_facility = PcrObj["name_of_facility"];
            this.name_crewmember = PcrObj["name_crewmember"];
            this.name_facility_repres = PcrObj["name_facility_repres"];
            this.signature_facility_repres = PcrObj["signature_facility_repres"];
            this.signature_crewmember = PcrObj["signature_crewmember"];
            this.patient_legal_guardian = PcrObj["patient_legal_guardian"];
            this.patient_health_care_poa = PcrObj["patient_health_care_poa"];
            this.relative_benefits = PcrObj["relative_benefits"];
            this.relative_affairs = PcrObj["relative_affairs"];
            this.repres_agency = PcrObj["repres_agency"];
            this.patient_care_report = PcrObj["patient_care_report"];
            this.facility_face_sheet = PcrObj["facility_face_sheet"];
            this.medical_record = PcrObj["medical_record"];
            this.facility_record = PcrObj["facility_record"];
            this.auth_text = PcrObj["auth_text"];
            this.type = PcrObj["type"];
            this.patient_repres = PcrObj["patient_repres"];
            InitObjects();
            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction (InsertUpdate);
            }
        public void ValidateFields()
            {

            if (name_crewmember_object != null) name_crewmember_object.HandleRecord();
            if (name_of_facility_object != null) name_of_facility_object.HandleRecord();
            if (type_object != null) type_object.HandleRecord();
            if (auth_text_object != null) auth_text_object.HandleRecord();

           
            //if (!string.IsNullOrEmpty(name_crewmember)) Utilities.ValidateField("person", name_crewmember);

            //if (!string.IsNullOrEmpty(name_of_facility)) Utilities.ValidateField("business", name_of_facility);

            //if (!string.IsNullOrEmpty(type)) Utilities.ValidateField("hipaa_types", type);

            //if (!string.IsNullOrEmpty(auth_text)) Utilities.ValidateField("text_blocks", auth_text);            

            }
        public void MapIntoIOSJson()
        {
             try
             {
                  JsonMaker.UpdateJsonValue("$.['HIPAA'].['Sig1.1_Encoding'].['Sig1.1_Encoding']", xml_patient_signature);
                  JsonMaker.UpdateJsonValue("$.HIPAA.signer.signer", witness_name);
                  JsonMaker.UpdateJsonValue("$.['HIPAA'].['Sig2.1_Encoding'].['Sig2.1_Encoding']", xml_witness_signature);
                  JsonMaker.UpdateJsonValue("$.['HIPAA'].['Sig1.2_Encoding'].['Sig1.2_Encoding']", repres_signature);
                  JsonMaker.UpdateJsonValue("$.['HIPAA'].['Sig1.3_Encoding'].['Sig1.3_Encoding']", signature_crewmember);
                  JsonMaker.UpdateJsonValue("$.['HIPAA'].['Sig2.3_Encoding'].['Sig2.3_Encoding']", signature_facility_repres);

             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapFromIOSJson(object JsonData)
        {

             try
             {
                  xml_patient_signature = JsonMaker.GetIOSJsonExtract("$.['Sig1.1_Encoding'].['Sig1.1_Encoding']", JsonData);
                 // if (!xml_patient_signature.Contains("data:image/png;base64,"))  xml_patient_signature = "data:image/png;base64," + xml_patient_signature;
                  witness_name = JsonMaker.GetIOSJsonExtract("$.signer.signer", JsonData);
                  xml_witness_signature = JsonMaker.GetIOSJsonExtract("$.['Sig2.1_Encoding'].['Sig2.1_Encoding']", JsonData);
                  repres_signature = JsonMaker.GetIOSJsonExtract("$.['Sig1.2_Encoding'].['Sig1.2_Encoding']", JsonData);
                  signature_crewmember = JsonMaker.GetIOSJsonExtract("$.['Sig1.3_Encoding'].['Sig1.3_Encoding']", JsonData);
                  signature_facility_repres = JsonMaker.GetIOSJsonExtract("$.['Sig2.3_Encoding'].['Sig2.3_Encoding']", JsonData);
                  reason_patient_incapable = JsonMaker.GetIOSJsonExtract("$.nosign.nosign", JsonData);

                  HandleRecord();
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        }
    }