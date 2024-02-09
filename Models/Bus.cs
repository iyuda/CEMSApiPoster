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
    public class Bus : BaseClass
        {
        public Bus(string id)
            : base(id, "Bus")
            {
            InitObjects();
            }
        public Bus(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                prop.SetValue(this, PcrObj[prop.Name]);
                }
            InitObjects();

            }


        public string agency_id { get; set; }
        public string bus_number { get; set; }
        public string bus_name { get; set; }
        public string bus_type { get; set; }
        public string bus_comment { get; set; }
        public string vehicle_id { get; set; }
        public string registration { get; set; }
        public string expiration { get; set; }
        public string lastInspection { get; set; }
        public string next_inspection { get; set; }
        public string active { get; set; }
        public string neighborhood { get; set; }
        public Agency agency_object { get; set; }
        public Neighborhood neighborhood_object { get; set; }

        private void InitObjects()
            {
            if (!string.IsNullOrEmpty(agency_id)) agency_object = new Agency(agency_id);
            if (!string.IsNullOrEmpty(neighborhood)) neighborhood_object = new Neighborhood(neighborhood);

            }

        public void HandleRecord(int InsertUpdate = 0)
            {
            this.ValidateFields();
            this.InsertUpdateAction(InsertUpdate);
            }

        public void ValidateFields()
            {
            if (!string.IsNullOrEmpty(agency_id))
                {
                Agency agency = new Agency(agency_id);
                agency.HandleRecord();
                }
            if (!string.IsNullOrEmpty(neighborhood))
                {
                Neighborhood neighborhoodobj = new Neighborhood(neighborhood);
                neighborhoodobj.HandleRecord();
                }
            }
        public static string GetBusIDByAgency(string agency_id, string bus_number="")
        {
             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from bus where bus_number = '" + bus_number +"' and agency_id = '" + agency_id + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  if (rv == null)
                  {
                       rv = Guid.NewGuid().ToString();
                       Bus bus = new Bus(rv);
                       bus.agency_id = agency_id;
                       bus.bus_number = "";
                       bus.HandleRecord();
                  }
                  return rv;
             }
        }
        }
    
    }