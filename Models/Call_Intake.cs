using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class Call_Intake : BaseClass
     {
            public string call_type { get; set; }
            public string call_info { get; set; }
            public string urgency { get; set; }
            public string reason_for_appointment { get; set; }
            public string reason_for_ambulance { get; set; }
            public string pick_up { get; set; }
            public string appointment { get; set; }
            public string requested { get; set; }
            public string comments { get; set; }
            public string come_back { get; set; }
            public string facility_id { get; set; }
            public string address_id { get; set; }
            public string trip_type { get; set; }
            public string assigned_to { get; set; }
            public string assigned { get; set; }
            public string en_route_63 { get; set; }
            public string on_scene_84 { get; set; }
            public string pt_contact { get; set; }
            public string from_scene_82 { get; set; }
            public string at_destination_81 { get; set; }
            public string in_service { get; set; }
            public string reason_for_ambulance_index { get; set; }
            public string latest_time { get; set; }
            public string latest_status { get; set; }
            public string trip_notes { get; set; }
            public string current_location { get; set; }
            public string additional_crew_members { get; set; }
            public string fd_showed_up { get; set; }
            public string driver_name { get; set; }
            public string pickup_room { get; set; }
            public string dropoff_room { get; set; }
            public string logisticare { get; set; }
            public string authorization { get; set; }
            public string pickup_phone { get; set; }
            public string dropoff_phone { get; set; }
            public string validation_status { get; set; }
            public string insurance_id { get; set; }
            public string insurance_other { get; set; }
            public string pt_address_id { get; set; }
            public string eligibility { get; set; }
            public string eligibility_timeset { get; set; }
            public string mileage_distance { get; set; }
            public string requesting_business_id { get; set; }
            public string price { get; set; }
            public string delay_notes { get; set; }
            public string require_stairchair { get; set; }
            public string updated_pick_up { get; set; }
            public string attempt_pcs { get; set; }
            public string require_bariatric { get; set; }
            public string als_requested { get; set; }
            public string fd_dispatched { get; set; }
            public string confirm_status { get; set; }


          public Call_Intake() { TableName = "call_intake"; }
          public Call_Intake(string id)
               : base(id, "call_intake")
          {

          }
          public Call_Intake(string TableName, JsonInputSection PcrObj)
               : base(TableName, PcrObj)
          {

          }
          public Call_Intake(object JsonData, string PathPrefix)
          {
               this.TableName = "call_intake";
          }
          public void HandleRecord(int InsertUpdate = 0)
          {

               //Utilities.ValidateField("business", business_id);
               this.InsertUpdateAction(InsertUpdate);
          }

          //public bool MapIntoDispositionJson()
          //{
          //     string Section = "Disposition";
          //     try
          //     {

          //          if (!this.Exists())
          //               return false;

          //          this.Retrieve();

          //          if (this.address_id != null)
          //          {
          //               Address address = new Address(this.address_id, true);
          //               address.MapIntoIOSJson("$." + Section + ".Location@Location");
          //          }
          //          if (this.facility_id != null)
          //          {
          //               Business facility = new Business(this.facility_id,true);
          //               facility.MapIntoIOSJson("$." + Section + ".Location@Location");
          //          }
          //          return true;
          //     }
          //     catch (Exception ex) { Logger.LogException(ex); return false; }
          //}         
          public bool MapIntoDispatchJson()
          {
               string Section = "Dispatch";
               try
               {

                    if (!this.Exists())
                         return false;

                    this.Retrieve();

                    JsonMaker.UpdateJsonValue("$." + Section + ".Timeline@Timeline_Assigned", Convert.ToDateTime(assigned).ToString("HH:mm"));
                    JsonMaker.UpdateJsonValue("$." + Section + ".Timeline@Timeline_EnRoute", Convert.ToDateTime(en_route_63).ToString("HH:mm"));
                    JsonMaker.UpdateJsonValue("$." + Section + ".Timeline@Timeline_OnScene", Convert.ToDateTime(on_scene_84).ToString("HH:mm"));
                    JsonMaker.UpdateJsonValue("$." + Section + ".Timeline@Timeline_PatientContact", Convert.ToDateTime(pt_contact).ToString("HH:mm"));
                    JsonMaker.UpdateJsonValue("$." + Section + ".Timeline@Timeline_FromScene", Convert.ToDateTime(from_scene_82).ToString("HH:mm"));
                    JsonMaker.UpdateJsonValue("$." + Section + ".Timeline@Timeline_AtDestination", Convert.ToDateTime(at_destination_81).ToString("HH:mm"));
                    JsonMaker.UpdateJsonValue("$." + Section + ".Timeline@Timeline_InService", Convert.ToDateTime(in_service).ToString("HH:mm"));
                    JsonMaker.UpdateJsonValue("$." + Section + ".DispatchInfo@DispatchInfo_DispatchMethod", urgency);
                    if (!String.IsNullOrEmpty(call_type))
                    {
                         string strToSplit = call_type.Replace("(", "~").Replace(")", "~");
                         Buttons button = new Buttons();
                         button._name = strToSplit;
                         if (strToSplit.Split('~').Length > 2)
                              button.LoadButton("Dispatch", "CALLTYPE." + strToSplit.Split('~')[1], "caption", button._name);
                         else
                              button.LoadButton("Dispatch", "CALLTYPE." + call_type.Replace(" ", ""), "caption", button._name);
                    }
                    if (this.address_id != null)
                    {
                         Address address = new Address(this.address_id,true);
                         if (address.unit == null) address.unit = this.pickup_room;
                         address.MapIntoIOSJson("$." + Section + ".CallLocation@CallLocation");
                    }
                    if (this.facility_id != null)
                    {
                         Business facility = new Business(this.facility_id,true);
                         facility.MapIntoIOSJson("$." + Section + ".CallLocation@CallLocation");
                         string address_id = facility.GetAddressID();
                         if (address_id != null)
                         {
                              Address address = new Address(address_id, true);
                              if (address.unit == null) address.unit = this.pickup_room;
                              address.MapIntoIOSJson("$." + Section + ".CallLocation@CallLocation");
                         }
                    }
                    

                    return true;
               }
               catch (Exception ex) { Logger.LogException(ex); return false; }
          }         
     }

}