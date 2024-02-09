using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.PCR
{

    public class PostedDataRow
    {
	    public string id { get; set; }
	    public string cad_number { get; set; }
	    public string agency_id { get; set; }
	    public string pcr_demographic_id { get; set; }
	    public string pcr_disposition_id { get; set; }
	    public string call_intake_id { get; set; }
	    public object pcr_id { get; set; }
	    public object bus_id { get; set; }
	    public string date { get; set; }
	    public object schedule_return { get; set; }
	    public object downloaded { get; set; }
	    public string downloaded_time { get; set; }
	    public string cancelled { get; set; }
	    public object firstCrewMember { get; set; }
	    public object secondCrewMember { get; set; }
	    public int is_schedule_return { get; set; }
	    public int is_cancelled { get; set; }
	    public string utc_insert { get; set; }
	    public string utc_update { get; set; }
	    public object user_login_id { get; set; }
	    public object schedule_cad_id { get; set; }
	    public object caller_name { get; set; }
	    public object caller_phone { get; set; }
	    public object transfer_care { get; set; }
	    public int is_transfer_care { get; set; }
	    public int active { get; set; }
	    public int is_dry_run { get; set; }
	    public string first_name { get; set; }
	    public string last_name { get; set; }
	   //public List<string> agency_id { get; set; }
	   //public List<string> insert_id { get; set; }
	   //public List<string> update_id { get; set; }
	   //public List<string> version_id { get; set; }
	   //public List<string> billing_category_id { get; set; }
	   //public List<string> multi_id { get; set; }
	   //public List<string> dynamic_button_id { get; set; }
	   //public string Name { get; set; }
	   //public string billing_code { get; set; }
	   //public string button_type { get; set; }
	   //public string diagnosis_code { get; set; }
	   //public string section_name { get; set; }
	   //public string section_label { get; set; }
	   //public string button_label_break { get; set; }
	   //public string button_class { get; set; }
	   //public string multi { get; set; }
	   //public string multi_button_type { get; set; }

    }
}
