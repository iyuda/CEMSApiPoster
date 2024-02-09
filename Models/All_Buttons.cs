using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
    public class All_Buttons : BaseClass
    {
        public string ButtonID { get; set; }
        public string Name { get; set; }
        public string billing_code { get; set; }
        public string billing_category_id { get; set; }
        public string button_type { get; set; }
        public string version_id { get; set; }
        public string multi { get; set; }
        public string multi_id { get; set; }
        public string section_id { get; set; }
        public string numeric_order { get; set; }
        public string dynamic_button_id { get; set; }
        public string agency_id { get; set; }
        public string _map_name { get; set; }
        public string _section_name { get; set; }
        public string _section_label { get; set; }

        public All_Buttons() { TableName = "all_buttons"; }
        public All_Buttons(string id, string SearchField = "id")
            : base(id, "all_buttons", SearchField)
            {
            
        }
    }
}