using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
namespace WebApiPoster.PCR
    {
    public class Dispatch : PcrBase
        {

      
        public Dispatch(string id)
            : base(id,"pcr_dispatch")
            {
            if (!string.IsNullOrEmpty(this.address_id)) address_object = new Address(address_id, true);
            if (!string.IsNullOrEmpty(facility_id)) facility_object = new Business(facility_id, true);

            //this.objAddress = new Address(address_id);
            //this.objFacility = new Business(facility_id);
            }
        public Dispatch(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            this.id = PcrObj["id"];
            this.Retrieve();
            this.date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            this.cad = PcrObj["cad"];
            this.transported_from = PcrObj["transported_from"];
            this.town_id = null;
            this.cross_street = null;
            this.assigned = PcrObj["assigned"];
            this.en_route_63 = PcrObj["en_route"];
            this.on_scene_84 = PcrObj["on_scene"];
            this.pt_contact = PcrObj["pt_contact"];
            this.from_scene_82 = PcrObj["from_scene"];
            this.at_destination = PcrObj["at_destination"];
            this.in_service = PcrObj["in_service"];
            this.pt_count = null;
            this.dispatch_method = PcrObj["dispatch_method"];
            this.phone = null;
            this.mileage_begin = PcrObj["mileage_begin"];
            if (! Utilities.IsNumeric (mileage_begin)) mileage_begin=null;
            this.mileage_end = PcrObj["mileage_end"];
            if (!Utilities.IsNumeric(mileage_end)) mileage_end = null;
            if (PcrObj["address_id"]+""!="") this.address_id = PcrObj["address_id"];
            if (PcrObj["facility_id"]+""!="") this.facility_id = PcrObj["facility_id"]; ;
            this.call_type = PcrObj["call_type"];
            this.CallReceivedTime = null;
            this.neighborhood = null;

            this.objAddress = new Address(address_id);
            this.objAddress.address = PcrObj["address"];
            
            string city_id = this.objAddress.GetCityIdByName(PcrObj["city"]);
            this.objAddress.city_id = String.IsNullOrEmpty(city_id)?null:city_id;

            string state_id = this.objAddress.GetStateIdByName (PcrObj["state"]);
            this.objAddress.state_id = String.IsNullOrEmpty(state_id) ? null : state_id;

            string zip_id = this.objAddress.GetZipIdByName (PcrObj["zip"]);
            this.objAddress.zip_id = String.IsNullOrEmpty(zip_id) ? null : zip_id;

            this.objAddress.country_id = this.objAddress.GetCountryIdByName("United States Of America");
            //this.objAddress = new Address(PcrObj["address"], PcrObj["city"], PcrObj["state"], PcrObj["zip"]);
            //if (address_id!="") objAddress.id=address_id;
            this.objFacility = new Business(facility_id);
            this.objFacility.name = PcrObj["facility_name"];
            //if (facility_id != "") objFacility.id = facility_id;
            }
          public Dispatch()
        {
             this.TableName = "pcr_dispatch";
        }

          public void MapIntoIOSJson()
          {
               try
               {
                    JsonMaker.UpdateJsonValue("$.Dispatch.DispatchInfo@DispatchInfo_CAD", cad);

                    JsonMaker.UpdateJsonValue("$.Dispatch.DispatchInfo@DispatchInfo_TransportedFrom", transported_from);

                    JsonMaker.UpdateJsonValue("$.Dispatch.CallLocation@CallLocation_RespondedFrom", cross_street);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Timeline@Timeline_Assigned", assigned);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Timeline@Timeline_EnRoute", en_route_63);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Timeline@Timeline_OnScene", on_scene_84);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Timeline@Timeline_PatientContact", pt_contact);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Timeline@Timeline_FromScene", from_scene_82);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Timeline@Timeline_AtDestination", at_destination);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Timeline@Timeline_InService", in_service);
                    JsonMaker.UpdateJsonValue("$.Dispatch.DispatchInfo@DispatchInfo_DispatchMethod", dispatch_method);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Mileage@Mileage_Begin", mileage_begin);
                    JsonMaker.UpdateJsonValue("$.Dispatch.Mileage@Mileage_End", mileage_end);
                    
                    Town town = new Town(this.town_id);
                    town.MapIntoIOSJson("$.Dispatch.DispatchInfo@DispatchInfo");

                    Address address = new Address(this.address_id);
                    address.MapIntoIOSJson("$.Dispatch.CallLocation@CallLocation");

                    Business facility = new Business(this.facility_id);
                    facility.MapIntoIOSJson("$.Dispatch.CallLocation@CallLocation");

                   
                    //object WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch", JsonData.ToString());
                    //JObject JsonObject = (JObject)JsonConvert.DeserializeObject(WorkJson.ToString());
                    //List<string> DriverList = new List<string>();
                    //foreach (JToken token in JsonObject.SelectToken(""))
                    //{
                    //     if (token.Path.StartsWith("Members@"))
                    //     {
                    //          DriverList.Add(JsonMaker.GetIOSJsonExtract("$." + token.Path, WorkJson));
                    //     }
                    //}
                    //WorkJson = (object)JsonMaker.GetIOSJsonExtract("$.Dispatch.Members.model.buttons", JsonData.ToString());
                    //if (WorkJson != null)
                    //{
                    //     JArray MembersArray = (JArray)JsonConvert.DeserializeObject(WorkJson.ToString());

                    //     foreach (JToken token in MembersArray)
                    //     {
                    //          Members member = new Members();
                    //          member.pcr_id = this.id;
                    //          member.MapFromIOSJson((object)token, DriverList);
                    //     }
                    //}

               }
               catch (Exception ex) { Logger.LogException(ex); }
          }
        public void MapFromIOSJson(object JsonData)
        {
             try
             {
                    cad= JsonMaker.GetIOSJsonExtract("$.DispatchInfo@DispatchInfo_CAD", JsonData);
                    transported_from = JsonMaker.GetIOSJsonExtract("$.DispatchInfo@DispatchInfo_TransportedFrom", JsonData);

                    cross_street = JsonMaker.GetIOSJsonExtract("$.CallLocation@CallLocation_RespondedFrom", JsonData);
                    assigned = JsonMaker.GetIOSJsonExtract("$.Timeline@Timeline_Assigned", JsonData);
                    en_route_63 = JsonMaker.GetIOSJsonExtract("$.Timeline@Timeline_EnRoute", JsonData);
                    on_scene_84 = JsonMaker.GetIOSJsonExtract("$.Timeline@Timeline_OnScene", JsonData);
                    pt_contact = JsonMaker.GetIOSJsonExtract("$.Timeline@Timeline_PatientContact", JsonData);
                    from_scene_82 = JsonMaker.GetIOSJsonExtract("$.Timeline@Timeline_FromScene", JsonData);
                    at_destination = JsonMaker.GetIOSJsonExtract("$.Timeline@Timeline_AtDestination", JsonData);
                    in_service = JsonMaker.GetIOSJsonExtract("$.Timeline@Timeline_InService", JsonData);
                    pt_count = "1";
                    dispatch_method = JsonMaker.GetIOSJsonExtract("$.DispatchInfo@DispatchInfo_DispatchMethod", JsonData);

                   // phone= JsonMaker.GetIOSJsonExtract("$.DispatchInfo@DispatchInfo_FirstName", JsonData);
                    mileage_begin = JsonMaker.GetIOSJsonExtract("$.Mileage@Mileage_Begin", JsonData);
                    mileage_end = JsonMaker.GetIOSJsonExtract("$.Mileage@Mileage_End", JsonData);
                   // neighborhood = JsonMaker.GetIOSJsonExtract("$.CallLocation@CallLocation_Name", JsonData);
                    //pt_person_object = new Person(JsonData, "PatientInfo@PatientInfo", true);
                    //emr_contact_person_object = new Person(JsonData, "EmergencyContact@EmergencyContact");
                    //phy_contact_person_object = new Person(JsonData, "PhysicianInfo@PhysicianInfo");
                    //primary_policy_object = new Insurance_Policy(JsonData, "PrimaryInsurance@PrimaryInsurance");


                    Town town = new Town(JsonData, "$.DispatchInfo@DispatchInfo");
                    if (town.town_name != null)
                    {
                         town.HandleRecord();
                         this.town_id = town.id;
                    }
                    Address address = new Address(JsonData, "$.CallLocation@CallLocation");
                    if (address.address  != null)
                    {
                         address.InsertUpdateAction();
                         this.address_id = address.id;
                    }
                    Business facility = new Business(JsonData, "$.CallLocation@CallLocation");
                    if (facility.name != null)
                    {
                         facility.InsertUpdateAction();
                         this.facility_id = facility.id;
                    }
                    //neighborhood = JsonMaker.GetIOSJsonExtract("$.DispatchInfo@DispatchInfo_FirstName", JsonData);
                    //call_type= JsonMaker.GetIOSJsonExtract("$.DispatchInfo@DispatchInfo_FirstName", JsonData);
                    //CallReceivedTime= JsonMaker.GetIOSJsonExtract("$.DispatchInfo@DispatchInfo_FirstName", JsonData);

              

                    HandleRecord();
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        private Address objAddress;
        private Business objFacility;
        public string date { get; set; }
        public string cad { get; set; }
        public string transported_from { get; set; }
        public string town_id { get; set; }
        public string cross_street { get; set; }
        public string assigned { get; set; }
        public string en_route_63 { get; set; }
        public string on_scene_84 { get; set; }
        public string pt_contact { get; set; }
        public string from_scene_82 { get; set; }
        public string at_destination { get; set; }
        public string in_service { get; set; }
        public string pt_count { get; set; }
        public string dispatch_method { get; set; }
        public string phone { get; set; }
        public string mileage_begin { get; set; }
        public string mileage_end { get; set; }
        public string address_id { get; set; }
        public string facility_id { get; set; }
        public string call_type { get; set; }
        public string CallReceivedTime { get; set; }
        public string neighborhood { get; set; }
        public Address address_object { get; set; }
        public Business facility_object { get; set; }

        public void HandleRecord(int InsertUpdate = 0)
            {
            ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {
            if (this.objAddress !=null) {
               this.objAddress.MakeSureItExists();
               this.address_id = objAddress.id;
        }
            //if (objAddress.id + "" == "")
            //    this.address_id = objAddress.GetAddressIdByAddress();
            //else
            if (this.objFacility != null)
            {
                 this.objFacility.MakeSureItExists();
                 this.facility_id = objFacility.id;
            }
            //if (objFacility.id + "" == "")
            //    {
            //    this.objFacility.MakeSureItExists();
            //    this.facility_id = objFacility.GetFacilityByName();
            //    }
            }
        }
    }