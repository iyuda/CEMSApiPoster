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
    public class person_relations:BaseClass 
        {
        public person_relations() { TableName = "person_relations"; }
        public person_relations(string id, string SearchField="id")
            : base(id, "person_phones", SearchField)
            {
            }
        public person_relations(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public void MapIntoIOSJson(string PathPrefix)
        {
             JsonMaker.UpdateJsonValue(PathPrefix + "_Relationship", GetTypeNameById(relation_type));

        }
        public person_relations(object JsonData, string PathPrefix, string is_primary = "1")
  
        {
             TableName = "person_relations";
             person_id = Guid.NewGuid().ToString();
             string relation_type_name = JsonMaker.GetIOSJsonExtract("$." + PathPrefix + "_Relationship", JsonData);
             if (relation_type_name!=null)
               relation_type = GetTypeIdByName(relation_type_name);
            

           }


        public string person_id { get; set; }
        public string related_to_id { get; set; }
        public string relation_type { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
                this.ValidateFields();
                this.InsertUpdateAction(InsertUpdate);
            }
        public void ValidateFields()
        {

             if (!string.IsNullOrEmpty(person_id))
             {
                  Person person = new Person(person_id);
                  person.HandleRecord();
             }
            


        }
        public string GetTypeNameById(string id)
        {


             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select relation_type from person_relation_type where id = '" + id + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  return rv;
             }
        }
        public string GetTypeIdByName(string RelationShipType)
        {


             using (MySqlConnection cn = new MySqlConnection(DbConnect.ConnectionString))
             {
                  cn.Open();
                  string SqlString = "select id from person_relation_type where relation_type = '" + RelationShipType + "'";
                  MySqlCommand cmd = new MySqlCommand(SqlString, cn);
                  string rv = cmd.ExecuteScalar()+"";
                  if (rv == "")
                  {
                       SqlString = "select id from person_relation_type where relation_type = 'Other'";
                       cmd.CommandText = SqlString;
                       rv = cmd.ExecuteScalar().ToString();
                  }
                  //rv = rv == null ? "" : rv.ToString();
                  return rv;
             }
        }
        }
     
    }