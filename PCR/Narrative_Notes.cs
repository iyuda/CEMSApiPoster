using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
    {
    public class Narrative_Notes:PcrBase 
        {

        public string objective_subjective_notes { get; set; }
        public string incident_report { get; set; }
        public string xml_crew_member_signature { get; set; }
        public string receiving_representative_name { get; set; }
        public string crew_member_name { get; set; }
        public string xml_receiving_facility_repres { get; set; }
        public string additional_notes { get; set; }
        public Users crew_member_name_object { get; set; }
        
      
        public Narrative_Notes(string id):base(id,"pcr_narrative_notes")
            {
            if (!string.IsNullOrEmpty(crew_member_name)) crew_member_name_object = new Users(crew_member_name);
            }
        public Narrative_Notes(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            this.id = PcrObj["id"];
            this.objective_subjective_notes = PcrObj["objective_subjective_notes"];
            this.incident_report = PcrObj["incident_report"];
            this.xml_crew_member_signature = PcrObj["xml_crew_member_signature"];
            this.receiving_representative_name = PcrObj["receiving_representative_name"];

            string crew_member_name = PcrObj["crew_member_name"];
            this.crew_member_name = String.IsNullOrEmpty(crew_member_name) ? null : crew_member_name;

            this.xml_receiving_facility_repres = PcrObj["xml_receiving_facility_repres"];
            this.additional_notes = PcrObj["additional_notes"];
            }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {

            if (!string.IsNullOrEmpty(crew_member_name))
                {
                Users users = new Users(crew_member_name);
                users.HandleRecord();
                }
           

            }
        }
    }