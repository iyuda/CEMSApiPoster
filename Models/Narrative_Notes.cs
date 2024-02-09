using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
    {
    public class Narrative_Notes:BaseClass 
        {

        public string objective_subjective_notes { get; set; }
        public string incident_report { get; set; }
        public string xml_crew_member_signature { get; set; }
        public string receiving_representative_name { get; set; }
        public string crew_member_name { get; set; }
        public string xml_receiving_facility_repres { get; set; }
        public string additional_notes { get; set; }
        public Users crew_member_name_object { get; set; }

        public Narrative_Notes()
        {
             this.TableName = "pcr_narrative_notes";
           //  InitObjects(true);
         //    MapFromIOSJson(JsonData);
        }
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
            if (crew_member_name_object != null) crew_member_name_object.HandleRecord();
            //if (!string.IsNullOrEmpty(crew_member_name))
            //    {
            //    Users users = new Users(crew_member_name);
            //    users.HandleRecord();
            //    }
           

            }
        public void MapIntoIOSJson(string pcr_id)
        {
             try
             {

                  JsonMaker.UpdateJsonValue("$.Narrative.signer.signer", receiving_representative_name);
                  JsonMaker.UpdateJsonValue("$.Narrative.Sig1_Encoding.Sig1_Encoding", xml_crew_member_signature);
                  JsonMaker.UpdateJsonValue("$.Narrative.Sig2_Encoding.Sig2_Encoding", xml_receiving_facility_repres);
                  JsonMaker.UpdateJsonValue("$.['Narrative'].['UnusualOccurance.UnusualOccurance_Occurance'].['UnusualOccurance_Occurance']", incident_report);
                  JsonMaker.UpdateJsonValue("$.Narrative.ObjectiveNotes.ObjectiveNotes", objective_subjective_notes);
                  JsonMaker.UpdateJsonValue("$.Narrative.AdditionalNotes.AdditionalNotes", additional_notes);   
                  Inputs pcr_input = new Inputs(); pcr_input.MapNarrativeIntoIOSJson(pcr_id, "PD");
                  pcr_input = new Inputs(); pcr_input.MapNarrativeIntoIOSJson(pcr_id, "Shield");
                  pcr_input = new Inputs(); pcr_input.MapNarrativeIntoIOSJson(pcr_id, "Precinct");

             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapInputsFromIOSJson(object JsonData, string pcr_id)
        {
             try
             {
                  Inputs pcr_input = new Inputs(); pcr_input.MapNarrativefromIOSJson(JsonData, pcr_id, "PD");
                  pcr_input = new Inputs(); pcr_input.MapNarrativefromIOSJson(JsonData, pcr_id, "Shield");
                  pcr_input = new Inputs(); pcr_input.MapNarrativefromIOSJson(JsonData, pcr_id, "Precinct");

                  HandleRecord();
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapFromIOSJson(object JsonData)
        {
             try
             {
                  //Person person = new Person(JsonData, "$.CallLocation@CallLocation");
                  receiving_representative_name = JsonMaker.GetIOSJsonExtract("$.signer.signer", JsonData);
                  xml_crew_member_signature = JsonMaker.GetIOSJsonExtract("$.['Sig1_Encoding'].['Sig1_Encoding']", JsonData);
                  xml_receiving_facility_repres = JsonMaker.GetIOSJsonExtract("$.['Sig2_Encoding'].['Sig2_Encoding']", JsonData);
                  incident_report = JsonMaker.GetIOSJsonExtract("$.['UnusualOccurance.UnusualOccurance_Occurance'].['UnusualOccurance_Occurance']", JsonData);
                  objective_subjective_notes = JsonMaker.GetIOSJsonExtract("$.ObjectiveNotes.ObjectiveNotes", JsonData); 
                  additional_notes = JsonMaker.GetIOSJsonExtract("$.AdditionalNotes.AdditionalNotes", JsonData); 
                  //crew_member_name?????

                  HandleRecord();
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        }
    }