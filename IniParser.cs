using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApiPoster.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.Linq.SqlClient;

namespace WebApiPoster
{
     public class IniParser
     {

          IniData m_data;
          JToken m_json;
          string agency_id;
          static Dictionary<string, string> IniMappings = new Dictionary<string, string>();
        private static void InitDictionary()
          {

                IniMappings.Clear();
                //IniMappings.Add("1087", "$.dispatch.buttons.runTypes|id:patientCancelled");
                //IniMappings.Add("1083", "$.dispatch.buttons.runTypes|id:patientDOAR");
                //IniMappings.Add("1083a", "$.dispatch.buttons.runTypes|id:patientDOA");
                //IniMappings.Add("1096", "$.dispatch.buttons.runTypes|id:patientGone");
                //IniMappings.Add("1090", "$.dispatch.buttons.runTypes|id:patientNotfound");
                //IniMappings.Add("1082", "$.dispatch.buttons.runTypes|id:patientTxTrans");

                IniMappings.Add("pcr_dispatch_text_1087dtype", "$.dispatch.buttons.runTypes.options|id:patientCancelled|label");
                IniMappings.Add("pcr_dispatch_text_1083dtype", "$.dispatch.buttons.runTypes.options|id:patientDOAR|label");
                IniMappings.Add("pcr_dispatch_text_1083ddtype", "$.dispatch.buttons.runTypes.options|id:patientDOA|label");
                IniMappings.Add("pcr_dispatch_text_1096dtype", "$.dispatch.buttons.runTypes.options|id:patientGone|label");
                IniMappings.Add("pcr_dispatch_text_1090dtype", "$.dispatch.buttons.runTypes.options|id:patientNotfound|label");
                IniMappings.Add("pcr_dispatch_text_1082dtype", "$.dispatch.buttons.runTypes.options|id:patientTxTrans|label");
            
                IniMappings.Add("pcr_dispatch_display_1087", "$.dispatch.buttons.runTypes.options|id:patientCancelled|display");
                IniMappings.Add("pcr_dispatch_display_1083", "$.dispatch.buttons.runTypes.options|id:patientDOAR|display");
                IniMappings.Add("pcr_dispatch_display_1083a", "$.dispatch.buttons.runTypes.options|id:patientDOA|display");
                IniMappings.Add("pcr_dispatch_display_1096", "$.dispatch.buttons.runTypes.options|id:patientGone|display");
                IniMappings.Add("pcr_dispatch_display_1090", "$.dispatch.buttons.runTypes.options|id:patientNotfound|display");
                IniMappings.Add("pcr_dispatch_display_1082", "$.dispatch.buttons.runTypes.options|id:patientTxTrans|display");

                IniMappings.Add("pcr_dispatch_display_dispatchmethod", "$.dispatch.buttons.dispatch.dispatch.method.display");
                IniMappings.Add("pcr_dispatch_display_outofcity", "$.dispatch.inputs.outOfNYC.value");
                IniMappings.Add("pcr_dispatch_display_sirens", "$.dispatch.buttons.lightsAndSirensUsed.display");

                IniMappings.Add("pcr_dispatch_display_locationcode", "$.dispatch.inputs.locationCode.display");
                IniMappings.Add("pcr_dispatch_display_calllocationaddress", "$.dispatch.inputs.callLocation.street.display");
                IniMappings.Add("pcr_dispatch_display_apartment", "$.dispatch.inputs.callLocation.apt.display");
                IniMappings.Add("pcr_dispatch_display_room", "$.dispatch.inputs.callLocation.room.display");
                IniMappings.Add("pcr_dispatch_display_facility", "$.dispatch.inputs.callLocation.facility.display");
                IniMappings.Add("pcr_dispatch_display_borough", "$.dispatch.inputs.borough.display");
                IniMappings.Add("pcr_dispatch_display_transportedfrom", "$.dispatch.dispatch.transportedfrom.display");
                IniMappings.Add("pcr_dispatch_display_mileage", "$.dispatch.inputs.mileage.display");
                IniMappings.Add("pcr_dispatch_display_locationCode", "$.dispatch.inputs.locationCode.display");
                IniMappings.Add("pcr_dispatch_display_assignedtime", "$.dispatch.inputs.timeline.assigned.display");
                IniMappings.Add("pcr_dispatch_display_enroutetime", "$.dispatch.inputs.timeline.enRoute.display");
                IniMappings.Add("pcr_dispatch_display_onscenetime", "$.dispatch.inputs.timeline.onScene.display");
                IniMappings.Add("pcr_dispatch_display_ptcontacttime", "$.dispatch.inputs.timeline.ptContact.display");
                IniMappings.Add("pcr_dispatch_display_fromscenetime", "$.dispatch.inputs.timeline.fromScene.display");
                IniMappings.Add("pcr_dispatch_display_atdestinationtime", "$.dispatch.inputs.timeline.atDestination.display");
                IniMappings.Add("pcr_dispatch_display_inservicetime", "$.dispatch.inputs.timeline.inService.display");
               

                IniMappings.Add("pcr_dispatch_text_dispatchmethod", "$.dispatch.buttons.dispatch.dispatch.method.label");
                IniMappings.Add("pcr_dispatch_text_calllocationaddress", "$.dispatch.inputs.callLocation.street.label");
                IniMappings.Add("pcr_dispatch_text_apartment", "$.dispatch.inputs.callLocation.apt.label");
                IniMappings.Add("pcr_dispatch_text_room", "$.dispatch.inputs.callLocation.room.label");
                IniMappings.Add("pcr_dispatch_text_facility", "$.dispatch.inputs.callLocation.facility.label");
                IniMappings.Add("pcr_dispatch_text_crossstreet", "$.dispatch.inputs.callLocation.crossStreet.label");
                IniMappings.Add("pcr_dispatch_text_borough", "$.dispatch.inputs.borough.label");
                IniMappings.Add("pcr_dispatch_text_transportedfrom", "$.dispatch.dispatch.transportedfrom.label");
                IniMappings.Add("pcr_dispatch_text_locationCode", "$.dispatch.inputs.locationCode.label");
                IniMappings.Add("pcr_dispatch_text_assignedtime", "$.dispatch.inputs.timeline.assigned.label");
                IniMappings.Add("pcr_dispatch_text_enroutetime", "$.dispatch.inputs.timeline.enRoute.label");
                IniMappings.Add("pcr_dispatch_text_onscenetime", "$.dispatch.inputs.timeline.onScene.label");
                IniMappings.Add("pcr_dispatch_text_ptcontacttime", "$.dispatch.inputs.timeline.ptContact.label");
                IniMappings.Add("pcr_dispatch_text_fromscenetime", "$.dispatch.inputs.timeline.fromScene.label");
                IniMappings.Add("pcr_dispatch_text_atdestinationtime", "$.dispatch.inputs.timeline.atDestination.label");
                IniMappings.Add("pcr_dispatch_text_inservicetime", "$.dispatch.inputs.timeline.inService.label");
                IniMappings.Add("pcr_dispatch_text_calltype", "$.dispatch.buttons.callTypes.label");


                IniMappings.Add("demographics.inputs.patientDetail.primaryPhone", "phone1");
                IniMappings.Add("demographics.inputs.patientDetail.secondaryPhone", "phone2");
                IniMappings.Add("demographics.inputs.patientDetail.dob", "dateofbirth");
                IniMappings.Add("demographics.inputs.patientInfo.defaultState", "state");
                IniMappings.Add("demographics.inputs.insurance", "insurancetab");
                IniMappings.Add("demographics.inputs.insurance.primaryInsurance", "insurance");
                IniMappings.Add("demographics.inputs.insurance.secondaryInsurance", "insurance2");
                IniMappings.Add("demographics.inputs.insurance.primaryInsurance.policyID", "insurance");

                IniMappings.Add("demographics.inputs.physicianInfo", "physiciancontact");


                IniMappings.Add("pcr_vitals_display_time", "$.vitals.inputs.time.display");
                IniMappings.Add("pcr_vitals_display_patientscondition", "$.vitals.buttons.patientCondition.display");
                IniMappings.Add("pcr_vitals_display_respiration", "$.vitals.buttons.respirationDescription.display");
                IniMappings.Add("pcr_vitals_display_bloodpressurediastolic", "$.vitals.buttons.patientCondition.display");
                IniMappings.Add("pcr_vitals_display_bloodpressuresystolic", "$.vitals.buttons.patientCondition.display");
                IniMappings.Add("pcr_vitals_display_pulseregularity", "$.vitals.buttons.patientCondition.display");
                IniMappings.Add("pcr_vitals_display_pulse", "$.vitals.buttons.patientCondition.display");
                IniMappings.Add("pcr_vitals_display_mentalstatus", "$.vitals.buttons.mentalStatus.display");
                IniMappings.Add("pcr_vitals_display_skintemperature", "$.vitals.buttons.skin.skinTemperature.display");
                IniMappings.Add("pcr_vitals_display_skincondition", "$.vitals.buttons.skin.skinCondition.display");
                IniMappings.Add("pcr_vitals_display_skincolor", "$.vitals.buttons.skin.skinColor.display");
                IniMappings.Add("pcr_vitals_display_pupils", "$.vitals.buttons.pupilLeft.display,$.vitals.buttons.pupilRight.display");
                IniMappings.Add("pcr_vitals_display_leftlungsound", "$.vitals.buttons.lungSound.values|leftIns|display,$.vitals.buttons.lungSound.values|leftIns|display");
                IniMappings.Add("pcr_vitals_display_rightlungsound", "$.vitals.buttons.lungSound.values|rightIns|display,$.vitals.buttons.lungSound.values|rightIns|display");
                IniMappings.Add("pcr_vitals_display_eyeresponse", "$.vitals.buttons.response.values|eyes|display");
                IniMappings.Add("pcr_vitals_display_verbalresponse", "$.vitals.buttons.response.values|verbal|display");
                IniMappings.Add("pcr_vitals_display_motorresponse", "$.vitals.buttons.response.values|motor|display");
                IniMappings.Add("pcr_vitals_display_gcsscore", "$.vitals.inputs.gcs.display");
                IniMappings.Add("pcr_vitals_display_traumascore", "$.vitals.inputs.traumaScore.display");
                IniMappings.Add("pcr_vitals_display_takenby", "$.vitals.inputs.takenBy.display");

                IniMappings.Add("pcr_rma_display_rmatype", "$.rma.buttons.rmaTypes.display");

                IniMappings.Add("pcr_narrativenotes_display_additionalnotes", "$.notes.inputs.additional.display");
                IniMappings.Add("pcr_narrativenotes_display_treatments", "$.notes.inputs.treatments.display");
                IniMappings.Add("pcr_narrativenotes_display_unusualocurrence", "$.notes.inputs.unusualOccurances.display");
                IniMappings.Add("pcr_narrativenotes_minimum_additionalnotes", "$.notes.inputs.additional.minLength");


                IniMappings.Add("pcr_narcotics_display_edchart", "$.narcotics.inputs.edChart.display");
                IniMappings.Add("pcr_narcotics_display_urn", "$.narcotics.inputs.urn.display");
                IniMappings.Add("pcr_narcotics_display_telemetryfacility", "$.narcotics.inputs.telemetryFacility.display");
                IniMappings.Add("pcr_narcotics_display_orderedby", "$.narcotics.buttons.orderedBy.display");
                IniMappings.Add("pcr_narcotics_display_amountused", "$.narcotics.inputs.amountUsed.display");
                IniMappings.Add("pcr_narcotics_display_amountwasted", "$.narcotics.inputs.amountWasted.display");
                IniMappings.Add("pcr_narcotics_display_amountordered", "$.narcotics.inputs.amountOrdered.display");
                IniMappings.Add("pcr_narcotics_display_repeatdose", "$.narcotics.inputs.repeatDosesOrdered.display");
                IniMappings.Add("pcr_narcotics_display_vialsealnumber", "$.narcotics.inputs.csVialSealNumber.display");
                IniMappings.Add("pcr_narcotics_validate_vialsealnumber", "$.narcotics.inputs.csVialSealNumber.required");
                IniMappings.Add("pcr_narcotics_validate_edchart", "$.narcotics.inputs.edChart.required");
                IniMappings.Add("pcr_narcotics_validate_urn", "$.narcotics.inputs.urn.required");
                IniMappings.Add("pcr_narcotics_validate_narcotickitnumber", "$.narcotics.inputs.narcoticKitNumber.required");
                IniMappings.Add("pcr_narcotics_validate_narcotickitsealnumber", "$.narcotics.inputs.narcoticKitSealNumber.required");

                IniMappings.Add("pcr_hipaa_display_reasonpatientcannotsign", "$.hipaa.inputs.reasonPatientCannotSign.display");
                IniMappings.Add("pcr_hipaa_display_reasonpatientcannotsign2", "$.hipaa.inputs.reasonPatientAndRepCannotSign.display");

                IniMappings.Add("pcr_pcs_display_reasonfortrip", "$.pcs.inputs.reasonsForTrip.display");
                IniMappings.Add("pcr_pcs_display_reasonforambulance", "$.pcs.inputs.reasonsForAmbulance.display");

          }
          private void UpdateDispatch()
          {
               try { 
                    SectionData Section = m_data.Sections.GetSectionData("dispatch_section");
                    foreach (var key in Section.Keys.Where(a=>IniMappings.ContainsKey(a.KeyName)))
                    {
                         string mapping_value = IniMappings[key.KeyName];
                         string[] mapping_array = mapping_value.Split('|');
                         string JsonPath = mapping_array[0];
                         dynamic ModifyValue="none";
                         if (key.KeyName.Contains("text"))
                         {
                              ModifyValue = key.Value.Replace("\"", "").Replace("'", "");
                         }
                         if (key.KeyName.Contains("display"))
                         {
                              ModifyValue = key.Value.Replace("\"", "").Replace("'", "") == "0" ? true : false;
                         }
                         if (ModifyValue.ToString()=="none") continue;

                         if (mapping_array.Length < 2)
                         {
                              JsonMaker.UpdateJsonValue(JsonPath, ModifyValue, ref m_json);
                         }
                         else
                         {
                              string SearchField = mapping_array[1].Split(':')[0];
                              string SearchValue = mapping_array[1].Split(':')[1];
                              string ModifyField = mapping_array[2];
                              JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                              
                         }
                    }
               }
               catch (Exception ex) { Logger.LogException(ex); return; }
          }
        private void UpdateTreatment()
        {
            try
            {
                // Temperature value = Tempurature.Medium;
                int[] Attributes = { (int)DispathMethodAttributes.Text, (int)DispathMethodAttributes.Display };
                //bool[] is_text_options = { true, false };
                foreach (int Attribute in Attributes)
                {
                    string JsonPath = "$.treatmentNodes";
                    string SectionName;
                    string SourceField;
                    string TargetField;
                    string TargetValue;
                    switch (Attribute)
                    {
                        case (int)DispathMethodAttributes.Text:
                            SectionName = "treatment_section";
                            SourceField = "text";
                            TargetField = "label";
                            break;
                        default:
                            SectionName = "treatment_section";
                            SourceField = "display";
                            TargetField = "display";
                            break;
                    }
                    SectionData Section = m_data.Sections.GetSectionData(SectionName);
                    JArray MethodsArray = (JArray)m_json.SelectToken(JsonPath);
                    if (MethodsArray == null) return;
                    if (Section == null) continue;
                    foreach (KeyData key_data in Section.Keys)
                    {
                        string SearchField = "id";
                        string method_id = key_data.KeyName.Replace("pcr_disposition_" + SourceField + "_", "");
                        JToken method = MethodsArray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString().ToLower().Replace(" ", "") == method_id);
                        //m_json.SelectToken(JsonPath +"[?(replace(lower(@.id), ' ','') == '" + method_id  + "')]");
                        if (method != null)
                        {
                            string SearchValue = method["id"].ToString();
                            string ModifyField = TargetField;
                            object ModifyValue = key_data.Value.Replace("\"", "").Replace("'", "");
                            ModifyValue = (Attribute == (int)DispathMethodAttributes.Text ? ModifyValue : (ModifyValue.ToString() == "1" ? true : false));
                            JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                        }
                    }

                 

                }
            }
            catch (Exception ex) { Logger.LogException(ex); return; }
        }

        const string Emergency = "callTypesEmergency";
        const string NonEmergency = "callTypesNonEmergency";
        private void UpdateCallTypes()
        {
            try
            {

                string JsonPath = "$.dispatch.buttons.callTypes.options";
                string SelectQuery;
                List<All_Buttons> ButtonsList;
                SelectQuery = "(select a.* from all_buttons a where dynamic_button_id = 'b1c2e7e0-03c5-11e0-b203-508ed78c3875' and a.section_id in('59c2464d-0618-11e0-9c66-0ba20c568bb6', '6f09bf67-e512-11e3-8fa5-842b2b4bbc99') and agency_id= '" + this.agency_id + "' order by name)"; //   ifnull(agency_id, '" + this.agency_id + "') = '" + this.agency_id +"')";
                ButtonsList = Utilities.GetClassList<All_Buttons>(SelectQuery);
                if (ButtonsList.Count == 0)
                {
                    SelectQuery = "(select a.* from all_buttons a where dynamic_button_id = 'b1c2e7e0-03c5-11e0-b203-508ed78c3875' and a.section_id in('59c2464d-0618-11e0-9c66-0ba20c568bb6', '6f09bf67-e512-11e3-8fa5-842b2b4bbc99') and ifnull(agency_id,'')='' order by name)"; 
                    ButtonsList = Utilities.GetClassList<All_Buttons>(SelectQuery);
                }


                JArray CallTypesArray = (JArray)m_json.SelectToken(JsonPath);
                CallTypesArray.Clear();
                List<string> ButtonIDs = new List<string>();
                foreach (All_Buttons button in ButtonsList)
                {
                    JObject item = new JObject();
                    item["id"] = button.id;
                    item["label"] = button.Name;
                    if (button.section_id== "59c2464d-0618-11e0-9c66-0ba20c568bb6")
                        item["emergency"] = false;                    
                    else
                        item["emergency"] = true;
                    if (button.agency_id == null)
                    {

                    }
                    if (!ButtonIDs.Contains(button.ButtonID))
                    {
                        CallTypesArray.Add((JToken)item);
                        ButtonIDs.Add(button.ButtonID);
                    }
                    else
                    {

                    }

                  
                }
            }
            catch (Exception ex) { Logger.LogException(ex); return; }
        }
        private void UpdateCallTypes(string call_type)
        {
            try
            {

                //          if ($callType == "emergency"){
                //    $callTypeButtons = AllButtons::model()->findAll(array("condition"=>"dynamic_button_id='b1c2e7e0-03c5-11e0-b203-508ed78c3875' and t.section_id = '6f09bf67-e512-11e3-8fa5-842b2b4bbc99' and t.agency_id='".$agency_id."'", "order"=>"t.Name"));
                //              if (empty($callTypeButtons))
                //              {
                //         $callTypeButtons = AllButtons::model()->findAll(array("condition"=>"dynamic_button_id='b1c2e7e0-03c5-11e0-b203-508ed78c3875' and agency_id is null", "order"=>"t.Name"));
                //              }

                //          }else{

                //$callTypeButtons = AllButtons::model()->findAll(array("condition"=>"dynamic_button_id='b1c2e7e0-03c5-11e0-b203-508ed78c3875' and t.section_id = '59c2464d-0618-11e0-9c66-0ba20c568bb6' and t.agency_id='".$agency_id."'", "order"=>"t.Name"));
                //              if (empty($callTypeButtons))
                //              {

                //          $callTypeButtons = AllButtons::model()->findAll(array("condition"=>"dynamic_button_id='b1c2e7e0-03c5-11e0-b203-508ed78c3875' and t.agency_id='802fa7c4-9abc-11e1-bbb3-842b2b4bbc99'", "order"=>"t.Name"));
                //              }
                //          }

                //string JsonPath = "$.dispatch.buttons." + call_type + ".options";
                string JsonPath = "$.dispatch.buttons.callTypes.options";
                string SelectQuery;
                List<All_Buttons> ButtonsList;
                if (call_type == Emergency) { 
                    SelectQuery = "(select a.* from all_buttons a where dynamic_button_id = 'b1c2e7e0-03c5-11e0-b203-508ed78c3875' and a.section_id = '6f09bf67-e512-11e3-8fa5-842b2b4bbc99' and agency_id= '" + this.agency_id + "' order by name)"; //   ifnull(agency_id, '" + this.agency_id + "') = '" + this.agency_id +"')";
                    ButtonsList = Utilities.GetClassList<All_Buttons>(SelectQuery);
                    if (ButtonsList.Count == 0)
                    {
                        SelectQuery = "(select a.* from all_buttons a where dynamic_button_id = 'b1c2e7e0-03c5-11e0-b203-508ed78c3875' and agency_id is null order by name)";
                        ButtonsList = Utilities.GetClassList<All_Buttons>(SelectQuery);
                    }
                }
                else
                {
                    SelectQuery = "(select a.* from all_buttons a where dynamic_button_id = 'b1c2e7e0-03c5-11e0-b203-508ed78c3875' and a.section_id = '59c2464d-0618-11e0-9c66-0ba20c568bb6' and agency_id= '" + this.agency_id + "' order by name)"; //   ifnull(agency_id, '" + this.agency_id + "') = '" + this.agency_id +"')";
                    ButtonsList = Utilities.GetClassList<All_Buttons>(SelectQuery);
                    if (ButtonsList.Count == 0)
                    {
                        SelectQuery = "(select a.* from all_buttons a where dynamic_button_id = 'b1c2e7e0-03c5-11e0-b203-508ed78c3875' and agency_id='802fa7c4-9abc-11e1-bbb3-842b2b4bbc99' order by name)";
                        ButtonsList = Utilities.GetClassList<All_Buttons>(SelectQuery);
                    }
                }
                JArray CallTypesArray = (JArray)m_json.SelectToken(JsonPath);
                if (call_type == Emergency)  CallTypesArray.Clear();
                List<string> ButtonIDs = new List<string>();
                foreach (All_Buttons button in ButtonsList)
                {
                    JObject item = new JObject();
                    item["id"] = button.id;
                    item["label"] = button.Name;
                    if (call_type == Emergency)
                        item["emergency"] = true;
                    else
                        item["emergency"] = false;
                    if (button.agency_id == null)
                        if (button.agency_id == null)
                    {

                    }
                    if (!ButtonIDs.Contains(button.ButtonID))
                    {
                        CallTypesArray.Add((JToken)item);
                        ButtonIDs.Add(button.ButtonID );
                    }
                    else
                    {

                    }

                    //ButtonIDs.Add(button.ButtonID + ";"+ button.agency_id);
                    //JToken item = CallTypesArray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString().ToLower().Replace(" ", "") == method_id);
                    //if (CallTypesArray.Contains(button.id)) { 

                    //}
                    //CallTypesArray.Add((JToken)item);
                }
            }
            catch (Exception ex) { Logger.LogException(ex); return; }
        }
        private void UpdateDispositions()
        {
            try
            {
                // Temperature value = Tempurature.Medium;
                int[] Attributes = { (int)DispathMethodAttributes.Text, (int)DispathMethodAttributes.Display};
                //bool[] is_text_options = { true, false };
                foreach (int Attribute in Attributes)
                {
                    string JsonPath = "$.dispatch.buttons.dispositions.options";
                    string SectionName;
                    string SourceField;
                    string TargetField;
                    string TargetValue;
                    switch (Attribute)
                    {
                        case (int)DispathMethodAttributes.Text:
                            SectionName = "disposition_section";
                            SourceField = "text";
                            TargetField = "label";
                            break;
                        default:
                            SectionName = "disposition_section";
                            SourceField = "display";
                            TargetField = "display";
                            break;
                    }
                    SectionData Section = m_data.Sections.GetSectionData(SectionName);
                    JArray MethodsArray = (JArray)m_json.SelectToken(JsonPath);
                    if (MethodsArray == null) return;
                    if (Section == null) continue;
                    foreach (KeyData key_data in Section.Keys)
                    {
                        string SearchField = "id";
                        string method_id = key_data.KeyName.Replace("pcr_disposition_" + SourceField + "_", "");
                        
                        JToken method = MethodsArray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString().ToLower().Replace(" ", "") == method_id);
                        //m_json.SelectToken(JsonPath +"[?(replace(lower(@.id), ' ','') == '" + method_id  + "')]");
                        if (method != null)
                        {
                            string SearchValue = method["id"].ToString();
                            string ModifyField = TargetField;
                            object ModifyValue = key_data.Value.Replace("\"", "").Replace("'", "");
                            string toReplace = Regex.Match(ModifyValue.ToString(), @"\(([^\}]+)\)").Groups[1].Value;
                            if (this.agency_id == "39f97c74-81f0-11e7-89c8-d8cb8a365043" && toReplace.StartsWith("10"))
                                ModifyValue=(new Regex(@"\(([^\}]+)\)")).Replace(ModifyValue.ToString(), "").Trim();
                            ModifyValue = (Attribute == (int)DispathMethodAttributes.Text ? ModifyValue : (ModifyValue.ToString() == "1" ? true : false));
                            JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                        }
                    }

                    //foreach (JToken method in MethodsArray)
                    //{
                    //    string method_id = method.SelectToken("$.id").ToString().Replace(" ", "").ToLower();
                    //    string key_name = "pcr_disposition_" + SourceField + "_" + method_id;
                    //    if (!Section.Keys.ContainsKey(key_name) && SourceField == "display")
                    //    {
                    //        string SearchField = "id";
                    //        string SearchValue = method.SelectToken("id").ToString();
                    //        string ModifyField = TargetField;
                    //        object ModifyValue = "false";
                    //        JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                    //    }
                    //}

                }
            }
            catch (Exception ex) { Logger.LogException(ex); return; }

        }
        private void UpdateDispatchMethods()
        {
            try
            {
                // Temperature value = Tempurature.Medium;
                int[] Attributes = { (int)DispathMethodAttributes.Text, (int)DispathMethodAttributes.Display, (int)DispathMethodAttributes.Emergency };
                //bool[] is_text_options = { true, false };
                foreach (int Attribute in Attributes)
                {
                    string JsonPath = "$.dispatch.buttons.dispatch.dispatch.method.options";
                    string SectionName;
                    string SourceField;
                    string TargetField;
                    string TargetValue;
                    switch (Attribute)
                    {
                        case (int)DispathMethodAttributes.Text:
                            SectionName = "dispatchmethodtext_section";
                            SourceField = "text";
                            TargetField = "label";
                            break;
                        case (int)DispathMethodAttributes.Display:
                            SectionName = "dispatchmethods_section";
                            SourceField = "display";
                            TargetField = "display";
                            break;
                        default:
                            SectionName = "dispatchmethodtype_section";
                            SourceField = "emergency";
                            TargetField = "emergency";
                            break;
                    }
                    SectionData Section = m_data.Sections.GetSectionData(SectionName);
                    JArray MethodsArray = (JArray)m_json.SelectToken(JsonPath);
                    if (MethodsArray == null) return;
                    if (Section == null) continue;
                    foreach (KeyData key_data in Section.Keys)
                    {
                        string SearchField = "id";
                        string method_id = key_data.KeyName.Replace("pcr_dispatchmethods_" + SourceField + "_", "");
//                        JToken method = MethodsArray.Children().FirstOrDefault(x => x.SelectToken(SearchField).ToString().ToLower().Replace(" ","") == method_id);
//m_json.SelectToken(JsonPath +"[?(replace(lower(@.id), ' ','') == '" + method_id  + "')]");
//                      if (method != null)
//                    {
                        string SearchValue = method_id; // method["id"].ToString();
                        string ModifyField = TargetField;
                        object ModifyValue = key_data.Value.Replace("\"", "").Replace("'", "");
                        ModifyValue = (Attribute == (int)DispathMethodAttributes.Text ? ModifyValue : (ModifyValue.ToString() == "1" ? true : false));
                        JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue, inify_search:true);
      //                  }
                    }

                    foreach (JToken method in MethodsArray)
                    {
                        string method_id = method.SelectToken("$.id").ToString().Replace(" ", "").ToLower();
                        string key_name = "pcr_dispatchmethods_" + SourceField + "_" + method_id;
                        if (!Section.Keys.ContainsKey(key_name) && SourceField == "display")
                        {
                            string SearchField = "id";
                            string SearchValue = method.SelectToken("id").ToString();
                            string ModifyField = TargetField;
                            object ModifyValue = "false";
                            JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                        }
                    }

                }
            }
            catch (Exception ex) { Logger.LogException(ex); return; }

        }
        enum DispathMethodAttributes { Text = 0, Display = 1, Emergency = 2 };
        private void UpdateDispatchMethods_old()
          {
               try
               {
                // Temperature value = Tempurature.Medium;
                    int[] Attributes = { (int)DispathMethodAttributes.Text, (int)DispathMethodAttributes.Display, (int)DispathMethodAttributes.Emergency };
                    //bool[] is_text_options = { true, false };
                    foreach (int Attribute in Attributes)
                    {
                        string JsonPath = "$.dispatch.buttons.dispatch.dispatch.method.options";
                        string SectionName;
                        string SourceField;
                        string TargetField;
                        string TargetValue;
                        switch (Attribute)
                        {
                            case (int) DispathMethodAttributes.Text:
                                SectionName = "dispatchmethodtext_section";
                                SourceField = "text";
                                TargetField = "label";
                                break;
                            case (int)DispathMethodAttributes.Display:
                                SectionName = "dispatchmethods_section";
                                SourceField = "display";
                                TargetField = "display";
                                break;
                            default:
                                SectionName = "dispatchmethodtype_section";
                                SourceField = "emergency";
                                TargetField = "emergency";
                                break;
                        }
                        SectionData Section = m_data.Sections.GetSectionData(SectionName);
                        JArray MethodsArray = (JArray)m_json.SelectToken(JsonPath);
                        if (MethodsArray == null) return;
                        if (Section == null) continue;
                        foreach (JToken method in MethodsArray)
                        {
                            string method_id = method.SelectToken("$.id").ToString().Replace(" ", "").ToLower();
                            string key_name = "pcr_dispatchmethods_" + SourceField + "_" + method_id;
                            if (Section.Keys.ContainsKey(key_name))
                            {
                                //string update_path = "$." + (is_text ? "label" : "display");
                                //string get_value = Section.Keys.GetKeyData(key_name).Value;
                                //string update_value = (is_text ? get_value : (get_value.Replace("\"", "") == "0" ? "true" : "false"));
                                //JToken work_token = method;
                                //JsonMaker.UpdateJsonValue(update_path, update_value.Replace("\"", ""), ref work_token);
                                string SearchField = "id";
                                string SearchValue = method.SelectToken("id").ToString();
                                string ModifyField = TargetField;
                                object ModifyValue = Section.Keys.GetKeyData(key_name).Value.Replace("\"", "").Replace("'", "");
                                ModifyValue = (Attribute==(int) DispathMethodAttributes.Text ? ModifyValue : (ModifyValue.ToString() == "1" ? true : false));
                                JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                            }
                            else if (SourceField == "display")
                            {
                                string SearchField = "id";
                                string SearchValue = method.SelectToken("id").ToString();
                                string ModifyField = TargetField;
                                object ModifyValue = "false";
                                JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                            }

                        }

                    }
               }
               catch (Exception ex) { Logger.LogException(ex); return; }

          }

        private void UpdateAssessment()
        {
            try
            {
                
                string SelectQuery;
                List<All_Buttons> ButtonsList;
                SelectQuery = "(select a.*, m.name as map_name, section_name, section_label from all_buttons a " +
                    "inner join sections s on a.section_id=s.id "+
                    "left outer join map_all_buttons m on a.id=m.all_buttons_id " +
                    "where dynamic_button_id = '56516e25-03c6-11e0-b203-508ed78c3875' " +
                    "and ifnull(a.agency_id, m.agency_id)= '" + this.agency_id + "' " +
                    "and ifnull(m.display,1)=1 " +
                    "and lower(replace(section_label, ' ','')) REGEXP 'reasonforambulance|reasonfortrip' " +
                    "order by a.name) ";
                    //and a.section_id in('59c2464d-0618-11e0-9c66-0ba20c568bb6', '6f09bf67-e512-11e3-8fa5-842b2b4bbc99')
                ButtonsList = Utilities.GetClassList<All_Buttons>(SelectQuery);

                string JsonPath = "$.pcs";
                JToken RootToken = m_json.SelectToken(JsonPath);
                //JArray CallTypesArray = (JArray)m_json.SelectToken(JsonPath);
                //CallTypesArray.Clear();

                foreach (All_Buttons button in ButtonsList)
                {
                    //JObject item = new JObject();
                    //item["id"] = button.id;
                    //item["label"] = button._map_name ?? button.Name;
                    string section_name = button._section_label.Replace(" ", "").ToLower();

                    //JToken section_token = RootToken.SelectToken("").FirstOrDefault(x => x.Path.ToLower().EndsWith(section_name+ (button.button_type=="Button"? "buttons" : "inputs")));
                    //var regex = new Regex(sDischargePort, RegexOptions.IgnoreCase);
                    //var search_token = from section_token in RootToken.SelectToken("")
                    //                where Regex.IsMatch(section_name + (button.button_type == "Button" ? "buttons" : "inputs"), x_array[len - 1].ToLower().Replace("button", "").Replace("inputs", "") + "*" + button.button_type.ToLower());
                    //                .Single().PortCode;
                    JToken section_token = RootToken.SelectToken("").FirstOrDefault(x =>
                    {
                        string[] x_array = x.Path.Split('.');
                        int len = x_array.Length;
                        string last_node = x_array[len - 1].ToLower().Replace("buttons", "").Replace("inputs", "");
                        //return SqlMethods.Like(section_name + (button.button_type == "Button" ? "buttons" : "inputs"), x_array[len - 1].ToLower().Replace("button", "%button").Replace("inputs", "%inputs"));
                        //return Regex.IsMatch(section_name + (button.button_type == "Button" ? "buttons" : "inputs"), x_array[len - 1].ToLower().Replace("button", "*button").Replace("inputs", "*inputs"));
                        return !String.IsNullOrWhiteSpace(section_name) && last_node !="" && section_name.StartsWith(last_node) && x.Path.EndsWith((button.button_type == "Button" ? "Buttons" : "Inputs"));
                    });
                    if (section_token!=null)
                    {
                        JArray section_array = (JArray)m_json.SelectToken("$."+section_token.Path);
                        string SearchField = "id";
                        string SearchValue = button.id;
                        string ModifyField = "label";
                        string ModifyValue = !String.IsNullOrEmpty(button._map_name) ? button._map_name : button.Name;
                        JsonMaker.ModifyArrayItem(ref m_json, section_token.Path, SearchField, SearchValue, ModifyField, ModifyValue);
                    }
                   

                }

            }
            catch (Exception ex) { Logger.LogException(ex); return; }
        }
        private void UpdateDemographics()
          {
               try
               {
                    List<string> Sections = new List<string> {"patientInfo", "patientDetail", "physicianInfo", "emergencyContact", "insurance"};
                    bool[] is_text_options = { true, false };
                    List<string> Categories = new List<string> { "display", "text", "validate"};
                    foreach (string Category in Categories)
                    {
                         foreach (string section in Sections)
                         {
                              UpdateDemographicsSection(section, Category);
                         }
                    }
               }
               catch (Exception ex) { Logger.LogException(ex); return; }
          }
          private void UpdateDemographicsSection(string section_name, string SourceField)
          {
               try
               {
                //pcr_demographics_validate_insurance
                    string JsonPath = "$.demographics.inputs."+section_name;
                    SectionData Section = m_data.Sections.GetSectionData("demographics_section");
                //foreach (var key in Section.Keys.Where(a => a.KeyName.Contains("_display_") || a.KeyName.Contains("_text_")))
                //{
                //     string SearchField;
                //     string SearchValue;
                //     string ModifyField;
                //     string mapping_value;
                //     string KeyName;
                //     if (!IniMappings.ContainsKey(key.KeyName))
                //     {
                //          KeyName = key.KeyName.Split('_')[key.KeyName.Split('_').Length - 1];
                //          SearchField = mapping_value.Split('|')[1].Split(':')[0];
                //          SearchValue = mapping_value.Split('|')[1].Split(':')[1];
                //          ModifyField = mapping_value.Split('|')[2];
                //     }
                //     else
                //     {
                //          mapping_value = IniMappings[key.KeyName];
                //          SearchField = mapping_value.Split('|')[1].Split(':')[0];
                //          SearchValue = mapping_value.Split('|')[1].Split(':')[1];
                //          ModifyField = mapping_value.Split('|')[2];
                //     }
                //     if (key.KeyName.Contains("text"))
                //     {
                //          string ModifyValue = key.Value.Replace("\"", "");
                //          JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                //     }
                //     if (key.KeyName.Contains("display"))
                //     {
                //          bool ModifyValue = key.Value.Replace("\"", "") == "0" ? true : false;
                //          JsonMaker.ModifyArrayItem(ref m_json, JsonPath, SearchField, SearchValue, ModifyField, ModifyValue);
                //     }

                //}
                    string ModifyField="";
                    switch (SourceField)
                    {
                    case "display":
                        ModifyField = "display";
                        break;
                    case "text":
                        ModifyField = "label";
                        break;
                    case "validate":
                        ModifyField = "required";
                        break;
                    }
                    //string ModifyField = is_text ? "label" : "display";
                    //string SourceField = is_text ? "text" : "display";
                    foreach (JToken sub_token in m_json.SelectToken(JsonPath))
                    {
                        string last_node_name = sub_token.Path.Split('.')[sub_token.Path.Split('.').Length - 1];
                        string map_path;
                        if (last_node_name == "display" && SourceField == "display")
                        {
                            last_node_name = sub_token.Path.Split('.')[sub_token.Path.Split('.').Length - 2];
                            map_path = sub_token.Path.Replace(".display","");
                        }
                        else
                            map_path = sub_token.Path;  
                        string key_name = "pcr_demographics_" + SourceField + "_" +last_node_name.ToLower() ;
                        
                        if (IniMappings.ContainsKey(map_path))
                         {
                              string new_name = IniMappings[map_path];
                              key_name = "pcr_demographics_" + SourceField + "_" + new_name;
                         }
                         key_name = key_name.ToLower();
                         if (Section.Keys.ContainsKey(key_name))
                         {
                            object ModifyValue = Section.Keys.GetKeyData(key_name).Value.Replace("\"", "").Replace("'", "");
                            switch (SourceField)
                            {
                                case "display":
                                    ModifyValue = (ModifyValue.ToString() == "0" ? true : false);
                                break;
                                case "validate":
                                    ModifyValue = (ModifyValue.ToString() == "1" ? true : false);
                                break;
                            }
                            //JsonMaker.UpdateJsonValue(JsonPath + "." + last_node_name + "." + ModifyField, JToken.Parse(JsonConvert.SerializeObject(ModifyValue, Formatting.Indented)), ref m_json);
                            JsonMaker.UpdateJsonValue("$." + map_path + "." + ModifyField, JToken.Parse(JsonConvert.SerializeObject(ModifyValue, Formatting.Indented)), ref m_json);

                    }

                    }

               }
               catch (Exception ex) { Logger.LogException(ex); return; }

          }

        private void UpdateRemainingMappings()
        {

        }
            private void UpdateOthers()
          {
               try
               {
                    List<string> Sections = new List<string> { "vitals", "notes", "narcotics", "rma", "hipaa", "pcs" };
                    List<string> Categories = new List<string> { "display", "text", "minimum" };
                    foreach (string section in Sections)
                    {
                         SectionData section_data = m_data.Sections.GetSectionData(section + "_section");
                         string section_name_in_key = section != "notes" ? section : "narrativenotes";

                    //    foreach (KeyData key_data in section_data.Keys)
                    //    {
                    //        if (IniMappings.ContainsKey(key_data.KeyName))
                    //        {
                    //            KeyValuePair<string, string> item = IniMappings.FirstOrDefault(x => x.Key.StartsWith("pcr_" + section_name_in_key));  //new KeyValuePair<string, string>() ; //=null; // = IniMappings[key_data.KeyName];
                    //            object ModifyValue = key_data.Value; // section_data.Keys.GetKeyData(item.Key).Value.Replace("\"", "").Replace("'", "");

                    //            switch (item.Key.Split('_')[2])
                    //            {
                    //                case "text":
                    //                    break;
                    //                case "minimum":
                    //                case "maximum":
                    //                    if (Utilities.IsNumeric(ModifyValue))
                    //                        ModifyValue = System.Convert.ToInt32(ModifyValue);
                    //                    break;
                    //                default:
                    //                    ModifyValue = ModifyValue.ToString() == "0" ? true : false;
                    //                    break;
                    //            }

                    //            JsonMaker.UpdateJsonValue(item.Value, JToken.Parse(JsonConvert.SerializeObject(ModifyValue, Formatting.Indented)), ref m_json);
                    //        }
                    //        else
                    //        {
                    //            string json_path="$."+
                    //            KeyValuePair<string, string> item = new KeyValuePair<string, string>(key_data.KeyName,  ; 
                    //            object ModifyValue = key_data.Value; // section_data.Keys.GetKeyData(item.Key).Value.Replace("\"", "").Replace("'", "");
                    //        }
                    //}

                    foreach (KeyValuePair<string, string> item in IniMappings.Where(x => x.Key.StartsWith("pcr_" + section_name_in_key)))
                    {
                        if (section_data.Keys.ContainsKey(item.Key))
                        {
                            object ModifyValue = section_data.Keys.GetKeyData(item.Key).Value.Replace("\"", "").Replace("'", "");
                            switch (item.Key.Split('_')[2])
                            {
                                case "text":
                                    break;
                                case "minimum":
                                case "maximum":
                                    if (Utilities.IsNumeric(ModifyValue))
                                        ModifyValue = System.Convert.ToInt32(ModifyValue);
                                    break;
                                case "display":
                                    ModifyValue = ModifyValue.ToString() == "0" ? true : false;
                                    break;
                                case "validate":
                                    ModifyValue = ModifyValue.ToString() == "1" ? true : false;
                                    break;
                            }
                            // bool is_text = !item.Key.Contains("_display_") ? true : false;
                            //ModifyValue = (is_text ? ModifyValue : (ModifyValue.ToString() == "0" ? true : false));
                            JsonMaker.UpdateJsonValue(item.Value, JToken.Parse(JsonConvert.SerializeObject(ModifyValue, Formatting.Indented)), ref m_json);
                        }
                    }
                }
                   
               }
               catch (Exception ex) { Logger.LogException(ex); return; }
          }
          private void UpdateTextBlocks()
          {
               //string field_name="text";
               //string table_name="text_blocks";
               //string search_field=";
               //string search_value;
               //string return_field
               try
               {
                    string JsonPath = "$.hipaa.buttons.statementLanguages";
                    JArray StatementLanguagesArray = (JArray)m_json.SelectToken(JsonPath);
                    if (StatementLanguagesArray == null) return;
                    foreach (JToken StatementLanguage in StatementLanguagesArray)
                    {

                         string language_id = StatementLanguage.SelectToken("$.id").ToString().Replace(" ", "").ToLower();
                         string text = Utilities.GetDataValue("text_blocks t inner join languages l on t.language=l.id and text like '%payment%'",
                                             new string[] {"code", "agency_id"},
                                             new string[] {language_id, agency_id},
                                             "text");
                         if (!String.IsNullOrEmpty(text))
                              JsonMaker.ModifyArrayItem(ref m_json, JsonPath, "id", language_id, "statement", text);
                         }
                }
               catch (Exception ex) { Logger.LogException(ex); return; }
          }

          static IniParser()
          {
               InitDictionary();
          }
          public IniParser(string IniFileName, string agency_id)
          {
               try
               {
                    this.agency_id = agency_id;
                    var parser = new FileIniDataParser();
                    string IniFileContent = (HttpContext.Current.Session["IniFileContent"] ?? "").ToString();
                    if (!String.IsNullOrEmpty(IniFileContent))
                    {
                        Logger.LogAction("agency: " + agency_id + System.Environment.NewLine + "Using File Content" + System.Environment.NewLine, "IniParsing");
                        IniData data = parser.ReadData(new StreamReader(new MemoryStream(System.Text.Encoding.ASCII.GetBytes(IniFileContent))));
                        this.m_data = data;
                        return;
                    }
                    
                    else if (File.Exists(IniFileName))
                    {
                        Logger.LogAction("agency: " + agency_id + System.Environment.NewLine + IniFileName + System.Environment.NewLine, "IniParsing");
                        IniData data = parser.ReadFile(IniFileName);
                         this.m_data = data;
                    }
                    else
                         this.m_data = null;
                    
               }
               catch (Exception ex) { Logger.LogException(ex); return; }
          }
          /// <summary>
          /// Parse a JSON object and return it as a dictionary of strings with keys showing the heirarchy.
          /// </summary>
          /// <param name="token"></param>
          /// <param name="nodes"></param>
          /// <param name="parentLocation"></param>
          /// <returns></returns>
          public static bool ParseJson(JToken token, Dictionary<string, string> nodes, string parentLocation = "")
          {
               if (token.HasValues)
               {
                    foreach (JToken child in token.Children())
                    {
                         if (token.Type == JTokenType.Property)
                              parentLocation += "/" + ((JProperty)token).Name;
                         ParseJson(child, nodes, parentLocation);
                    }

                    // we are done parsing and this is a parent node
                    return true;
               }
               else
               {
                    // leaf of the tree
                    if (nodes.ContainsKey(parentLocation))
                    {
                         // this was an array
                         nodes[parentLocation] += "|" + token.ToString();
                    }
                    else
                    {
                         // this was a single property
                         if (parentLocation.Contains("/buttons")|| parentLocation.Contains("/inputs"))
                              nodes.Add(parentLocation, token.ToString());
                    }

                    return false;
               }
          }

          public JObject UpdateJsonFromIni(JToken Json_Object, bool is_dynamic=false)
          {
               try
               {
                    this.m_json=Json_Object;
                if (!is_dynamic)
                {
                    if (this.m_data != null)
                    {
                        UpdateDispatch();
                        UpdateDispatchMethods();
                        //UpdateCallTypes();
                        UpdateCallTypes(Emergency);
                        UpdateCallTypes(NonEmergency);
                        UpdateDispositions();
                        UpdateDemographics();
                        UpdateOthers();
                        UpdateAssessment();
                    }
                    UpdateTextBlocks();
                }
                else if (this.m_data != null)
                {

                }
                    //JToken Out_Json = Json_Object;
                    //Dictionary<string, string> nodes = new Dictionary<string, string>();
                    //ParseJson(Json_Object, nodes);
                    //string WorkKey;
                    //foreach (var Section in m_data.Sections)
                    //{    
                    //     foreach (var Key in Section.Keys)
                    //     {
                    //          WorkKey=Key.KeyName;
                    //          foreach(KeyValuePair<string, string> node in nodes)
                    //          {
                    //               if (IniParser.IniMappings.ContainsKey(WorkKey))
                    //               {
                    //                    WorkKey = IniParser.IniMappings[WorkKey];
                    //                    if (WorkKey==node.Key.Split('/')[node.Key.Split('/').Length-1]) {
                    //                         JsonMaker.UpdateJsonValue("$." + node.Key.Replace("/", "."), Key.Value, ref Out_Json);
                    //                         break;
                    //                    }
                    //               }
                    //          }

                    //     }
                    //}

                    //foreach (JToken field in Out_Json) {
                    //      foreach (var Section in m_data.Sections)
                    //      {
                    //           if (Section.Keys.ContainsKey(AlternateKey)) 
                    //                JsonMaker.UpdateJsonValue(field.Path, Section.Keys[AlternateKey].ToString(), ref field);
                    //      }
                    //     m_data.Sections.Contains()
                    //}
                    //= IniParser.IniMappings.ContainsKey(VitalID) ? IniParser.IniMappings[VitalID] : ""
                    return (JObject)JsonConvert.DeserializeObject(m_json.ToString());
               }
               catch (Exception ex) { Logger.LogException(ex); return null; }
          }
          public JObject ConvertIniToJson()
          {
               try
               {
                    JToken Out_Json = (JToken)new JObject(); ;
                    //JsonMaker.UpdateJsonValue("$.id", section_id, ref ID_Token);
                    foreach (var Section in m_data.Sections)
                    {
                         string SectionPrefix = Section.SectionName.Split('_')[0];
                         foreach (var Key in Section.Keys)
                         {
                              string WorkKeyName = Key.KeyName;
                              if (!String.IsNullOrEmpty(SectionPrefix)) {
                                   WorkKeyName = WorkKeyName.Replace("pcr_" + SectionPrefix + "_", "");
                                   string[] KeyTail = WorkKeyName.Split('_');
                                   if (KeyTail.Length < 2)
                                        JsonMaker.UpdateJsonValue("$." + Section.SectionName + "." + WorkKeyName, Key.Value.Replace("\"", ""), ref Out_Json);
                                   else
                                   {
                                        string Level2Key = KeyTail[0];
                                        string Level1Key = KeyTail[1];
                                        bool Skip=false;
                                        switch (Level2Key)
                                        {
                                             case "prevent":
                                             case "warn":
                                             case "validate":
                                                  Skip = true;
                                                  break;
                                        }
                                        if (!Skip) JsonMaker.UpdateJsonValue("$." + Section.SectionName + "." + Level1Key + "." + Level2Key, Key.Value.Replace("\"", ""), ref Out_Json);
                                   }
                              }
                              else
                              {

                                   JsonMaker.UpdateJsonValue("$." + Section.SectionName + "." + WorkKeyName, Key.Value.Replace("\"", ""), ref Out_Json);
                              }
                              
                         }
                    }
                    //m_data.Sections.
                    return (JObject)Out_Json;
               }
               catch (Exception ex) { Logger.LogException(ex); return null; }
          }
     }
}