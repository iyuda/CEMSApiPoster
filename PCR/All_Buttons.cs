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
    public class All_Buttons : PcrBase
        {
        public All_Buttons(string id)
            : base(id, "All_Buttons")
            {
            InitObjects();
            }
        public All_Buttons(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where(prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }
            InitObjects();
            }

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
        public Agency agency_object { get; set; }
        public Billing_Category billing_category_object { get; set; }
        public dynamic_button_types dynamic_button_types_object { get; set; }
        public multi_buttons multi_buttons_object { get; set; }
        public Sections  sections_object { get; set; }

        private void InitObjects()
            {
            if (!string.IsNullOrEmpty(agency_id)) agency_object = new Agency(agency_id);
            if (!string.IsNullOrEmpty(billing_category_id)) billing_category_object = new Billing_Category(billing_category_id);
            if (!string.IsNullOrEmpty(dynamic_button_id)) dynamic_button_types_object = new dynamic_button_types(dynamic_button_id);
            if (!string.IsNullOrEmpty(multi_id)) multi_buttons_object = new multi_buttons(multi_id);
            if (!string.IsNullOrEmpty(section_id)) sections_object = new Sections(section_id);
            
            }

        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }

        public void ValidateFields()
            {

            if (agency_object != null) agency_object.HandleRecord();
            if (billing_category_object != null) billing_category_object.HandleRecord();
            if (dynamic_button_types_object != null) dynamic_button_types_object.HandleRecord();
            if (multi_buttons_object != null) multi_buttons_object.HandleRecord();
            if (sections_object != null) sections_object.HandleRecord();

            
            }

        }
    }