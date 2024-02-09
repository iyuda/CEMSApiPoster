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
    public class Disposition : PcrBase
        {
      
        public Disposition()
            {

            }
        public Disposition(string TableName, string id) :base(TableName, id)
            {
            this.Retrieve ();
            this.out_of_area_exists = "0";
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
             //objAddress = new Address(this.address_id);
             //objDisposition_Facility = new Disposition_Facility(this.disposition_facility_id);
            }

        //private Address objAddress;
        //private Disposition_Facility objDisposition_Facility;
        public string disposition_facility_id { get; set; }
        public string address_id { get; set; }
        public string out_of_area { get; set; }
        public string out_of_area_exists { get; set; }

        public void HandleRecord(int InsertUpdate=0)
            {
            if (!string.IsNullOrEmpty(address_id))
                {
                Address address = new Address(address_id);
                address.MakeSureItExists();
                }
            if (!string.IsNullOrEmpty(disposition_facility_id))
                {
                Disposition_Facility objDisposition_Facility = new Disposition_Facility(disposition_facility_id);
                objDisposition_Facility.MakeSureItExists();
                }
           

            //objAddress.MakeSureItExists();
            //if (objAddress.id + "" == "")
            //    this.address_id = objAddress.GetAddressIdByAddress();
            //else
            //    this.address_id = objAddress.id; 
            
            //objDisposition_Facility.MakeSureItExists();
            ////this.facility_id = PcrSection.GetFacilityByName(objFacility.name);
            
            this.InsertUpdateAction(InsertUpdate);

            }

        }
        }
   