using Microsoft.VisualBasic;
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

    public class Dispatch : PcrBase
        {

        public Dispatch()
            {
            }
        public Dispatch(string TableName, string id)
            : base(TableName, id)
            {
            this.Retrieve();
            this.objAddress = new Address(address_id);
            this.objFacility = new Facility(facility_id);
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

            string state_id = this.objAddress.GetCityIdByName(PcrObj["state"]);
            this.objAddress.state_id = String.IsNullOrEmpty(state_id) ? null : state_id;

            string zip_id = this.objAddress.GetCityIdByName(PcrObj["zip"]);
            this.objAddress.zip_id = String.IsNullOrEmpty(zip_id) ? null : zip_id;

            this.objAddress.country_id = this.objAddress.GetCountryIdByName("United States Of America");
            //this.objAddress = new Address(PcrObj["address"], PcrObj["city"], PcrObj["state"], PcrObj["zip"]);
            //if (address_id!="") objAddress.id=address_id;
            this.objFacility = new Facility(facility_id);
            this.objFacility.name = PcrObj["facility_name"];
            //if (facility_id != "") objFacility.id = facility_id;
            }

        private Address objAddress;
        private Facility objFacility;
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

        public void HandleRecord(int InsertUpdate = 0)
            {
            ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
            {
            this.objAddress.MakeSureItExists();
            //if (objAddress.id + "" == "")
            //    this.address_id = objAddress.GetAddressIdByAddress();
            //else
            this.address_id = objAddress.id;
            this.objFacility.MakeSureItExists();
            this.facility_id = objFacility.id;
            //if (objFacility.id + "" == "")
            //    {
            //    this.objFacility.MakeSureItExists();
            //    this.facility_id = objFacility.GetFacilityByName();
            //    }
            }
        }
    }