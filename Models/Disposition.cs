using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text; 
using System.Configuration;
using System.Globalization;

namespace WebApiPoster.Models
    {
    public class Disposition : BaseClass
        {
      
        public Disposition()
            {
                 this.TableName = "pcr_disposition";
            }
        public Disposition(string id) :base(id,"pcr_disposition")
            {
            this.out_of_area_exists = "0";
            InitObjects(); 
            //objAddress = new Address();
            //objDisposition_Facility = new Disposition_Facility();
            }
        public Disposition(string TableName, JsonInputSection PcrObj)
            {
             this.TableName = TableName;
             this.id = PcrObj["transported_from"];

             string disposition_facility_id = PcrObj["disposition_facility_id"];
             this.disposition_facility_id = String.IsNullOrEmpty(disposition_facility_id) ? null : disposition_facility_id;
            
             string address_id = PcrObj["address_id"];
             this.address_id = String.IsNullOrEmpty(address_id) ? null : address_id;

             this.out_of_area = PcrObj["out_of_area"];
             this.out_of_area_exists = PcrObj["out_of_area_exists"];

             InitObjects();
             //objAddress = new Address(this.address_id);
             //objDisposition_Facility = new Disposition_Facility(this.disposition_facility_id);
            }

        //private Address objAddress;
        //private Disposition_Facility objDisposition_Facility;
        public string disposition_facility_id { get; set; }
        public string address_id { get; set; }
        public string out_of_area { get; set; }
        public string out_of_area_exists { get; set; }
        public Address address_object { get; set; }
        public Disposition_Facility disposition_facility_object { get; set; }

        private void InitObjects()
            {
            if (!string.IsNullOrEmpty(address_id)) address_object = new Address(address_id, true);
            if (!string.IsNullOrEmpty(disposition_facility_id)) disposition_facility_object = new Disposition_Facility(disposition_facility_id);
            }
        public void HandleRecord(int InsertUpdate=0)
            {
            if (address_object != null) address_object.MakeSureItExists();
            if (disposition_facility_object != null) disposition_facility_object.HandleRecord();                
            
            this.InsertUpdateAction(InsertUpdate);

            }
        public void MapIntoIOSJson()
        {
             try
             {
                  
                  if (this.address_object != null)
                  {
                       //Address address = new Address(this.address_id);
                       address_object.MapIntoIOSJson("$.Disposition.Location@Location");
                  }
                  if (this.disposition_facility_object != null)
                  {
                       //Business disposition_facility = new Business(this.disposition_facility_id);
                       disposition_facility_object.business_object.MapIntoIOSJson("$.Disposition.Location@Location");
                  }


             }
             catch (Exception ex) { Logger.LogException(ex); }
        }  
        public void MapFromIOSJson(object JsonData)
        {
             try
             {

                  
                  Address address = new Address(JsonData, "$.Location@Location");
                  if (address.address != null)
                  {
                       address.InsertUpdateAction();
                       this.address_id = address.id;
                  }

                  Disposition_Facility disposition_facility = new Disposition_Facility(JsonData, "$.Location@Location");
                  if (disposition_facility.id != null)
                  {
                       this.disposition_facility_id = disposition_facility.id;
                  }

                  HandleRecord();
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        }
       
        }
   