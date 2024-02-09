using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
{
	public class GetDataRow
	{
		public List<string> id { get; set; }
		public List<string> agency_id { get; set; }
		string cad_number { get; set; }
		string pcr_demographic_id { get; set; }
		string pcr_disposition_id { get; set; }
		string call_intake_id { get; set; }
		string pcr_id { get; set; }
		string bus_id { get; set; }
		string date { get; set; }
		string schedule_return { get; set; }
		string downloaded { get; set; }
		string downloaded_time { get; set; }
		string cancelled { get; set; }
		string firstCrewMember { get; set; }
		string secondCrewMember { get; set; }
		int is_schedule_return { get; set; }
		int is_cancelled { get; set; }
		public List<string> utc_insert { get; set; }
		public List<string> utc_update { get; set; }
		string user_login_id { get; set; }
		string schedule_cad_id { get; set; }
		string caller_name { get; set; }
		string caller_phone { get; set; }
		string transfer_care { get; set; }
		int is_transfer_care { get; set; }
		int active { get; set; }
		int is_dry_run { get; set; }


		//string id { get; set; }
		string call_type { get; set; }
		string call_info { get; set; }
		string urgency { get; set; }
		string reason_for_appointment { get; set; }
		string reason_for_ambulance { get; set; }
		string pick_up { get; set; }
		string appointment { get; set; }
		string requested { get; set; }
		string comments { get; set; }
		string come_back { get; set; }
		string facility_id { get; set; }
		string address_id { get; set; }
		string trip_type { get; set; }
		string assigned_to { get; set; }
		string assigned { get; set; }
		string en_route_63 { get; set; }
		string on_scene_84 { get; set; }
		string pt_contact { get; set; }
		string from_scene_82 { get; set; }
		string at_destination_81 { get; set; }
		string in_service { get; set; }
		int reason_for_ambulance_index { get; set; }
		string latest_time { get; set; }
		string latest_status { get; set; }
		string trip_notes { get; set; }
		string current_location { get; set; }
		string additional_crew_members { get; set; }
		string fd_showed_up { get; set; }
		string driver_name { get; set; }
		string pickup_room { get; set; }
		string dropoff_room { get; set; }
		string logisticare { get; set; }
		string authorization { get; set; }
		string pickup_phone { get; set; }
		string dropoff_phone { get; set; }
		string validation_status { get; set; }
		string insurance_id { get; set; }
		string insurance_other { get; set; }
		string pt_address_id { get; set; }
		string eligibility { get; set; }
		string eligibility_timeset { get; set; }
		string mileage_distance { get; set; }
		string requesting_business_id { get; set; }



		string pt_person { get; set; }
		string emr_contact_person { get; set; }
		string phy_contact_person { get; set; }
		string primary_policy { get; set; }
		string secondary_policy { get; set; }
		string terciary_insurance { get; set; }
		public List<string> insert_id { get; set; }
		public List<string> update_id { get; set; }


		public List<string> version_id { get; set; }
		public List<string> billing_category_id { get; set; }
		public List<string> multi_id { get; set; }
		public List<string> dynamic_button_id { get; set; }
		public string Name { get; set; }
		public string billing_code { get; set; }
		public string button_type { get; set; }
		public string diagnosis_code { get; set; }
		public string section_name { get; set; }
		public string section_label { get; set; }
		public string button_label_break { get; set; }
		public string button_class { get; set; }
		public string multi { get; set; }
		public string multi_button_type { get; set; }
	}
}