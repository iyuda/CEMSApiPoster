using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApiPoster.PCR
    {
    public class Person:PcrBase 
        {
        public Person() { TableName = "person"; }
        public Person(string id, string SearchField="id")
            : base(id, "person", SearchField)
            {
                 active = "1";
            }
        public Person(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                    prop.SetValue(this, PcrObj[prop.Name]);
                }

            }
        public void MapIntoIOSJson(string PathPrefix, bool HasDetail = false)
        {
             JsonMaker.UpdateJsonValue(PathPrefix + "_FirstName", first_name);
             JsonMaker.UpdateJsonValue(PathPrefix + "_MiddleInitial", mi);
             JsonMaker.UpdateJsonValue(PathPrefix + "_LastName", last_name);
             if (HasDetail)
             {
                  string DetailPrefix = PathPrefix.Replace("Info", "Detail");
                  JsonMaker.UpdateJsonValue(DetailPrefix + "_DOB", dob);
                  JsonMaker.UpdateJsonValue(DetailPrefix + "_WeightLbs", weight);
                  JsonMaker.UpdateJsonValue(DetailPrefix + "_Gender", is_male+"" ==""?null: is_male+""=="1"?"Male":"Female");
                  JsonMaker.UpdateJsonValue(DetailPrefix + "_Age", age);
                  JsonMaker.UpdateJsonValue(DetailPrefix + "_SSN", ss);

                  string SelectQuery = "(select pn.* from person p inner join person_phones pp on p.id=pp.person_id inner join phone_number pn on pn.id=pp.phone_id where person_id = '" + this.id + "')";
                  List<phone_number> PhonesList = Utilities.GetClassList<phone_number>(SelectQuery, this.id);
                  for (int i = 1; i <= 2 && i <= PhonesList.Count; i++)
                  {
                       JsonMaker.UpdateJsonValue(DetailPrefix + "_Phone" + i.ToString(), PhonesList[i-1].number);
                  }

                  SelectQuery = "(select a.* from person p inner join personal_addresses pa on pa.person=p.id inner join address a on a.id=pa.address where person = '" + this.id + "')";
                  List<Address> AddressList = Utilities.GetClassList<Address>(SelectQuery, this.id);
                  foreach(Address address in AddressList)
                  {
                       address.MapIntoIOSJson(PathPrefix);
                  }

                  SelectQuery = "(select pr.* from person p inner join person_relations pr on pr.related_to_id=p.id where related_to_id  = '" + this.id + "')";
                  List<person_relations> RelationList = Utilities.GetClassList<person_relations>(SelectQuery);
                  foreach (person_relations relation in RelationList)
                  {
                       relation.MapIntoIOSJson(PathPrefix);
                  }
                 
             }

        }
        public Person(object JsonData,string PathPrefix, bool HasDetail=false)  
        {
          this.TableName = "person";
          active = "1";
          first_name = JsonMaker.GetIOSJsonExtract("$."+ PathPrefix +"_FirstName", JsonData);
          mi = JsonMaker.GetIOSJsonExtract("$."+ PathPrefix +"_MiddleInitial", JsonData);
          last_name = JsonMaker.GetIOSJsonExtract("$."+ PathPrefix +"_LastName", JsonData);
          if (HasDetail)
          {
               string DetailPrefix = PathPrefix.Replace("Info", "Detail");
               dob = JsonMaker.GetIOSJsonExtract("$." + DetailPrefix + "_DOB", JsonData);
               weight = JsonMaker.GetIOSJsonExtract("$." + DetailPrefix + "_WeightLbs", JsonData);
               is_male = JsonMaker.GetIOSJsonExtract("$." + DetailPrefix + "_Gender", JsonData) == "Male" ? "1" : "0";
               int age;
               int.TryParse(JsonMaker.GetIOSJsonExtract("$." + DetailPrefix + "_Age", JsonData), out age);
               this.age = age.ToString();
               ss = JsonMaker.GetIOSJsonExtract("$." + DetailPrefix + "_SSN", JsonData);
          }
          HandleRecord();
          person_phones phone = new person_phones(JsonData, PathPrefix+ "_Phone1", "1");
          if (phone.phone_id != null)
          {
               phone.person_id = id;
               phone.HandleRecord();
          }

          phone = new person_phones(JsonData, PathPrefix+ "_Phone2", "0");
          if (phone.phone_id != null)
          {
               phone.person_id = id;
               phone.HandleRecord();
          }

          personal_addresses personal_address = new personal_addresses(JsonData, PathPrefix);
          if (personal_address.address != null)
          {
               personal_address.person = id;
               personal_address.HandleRecord();
          }
          person_relations person_relation = new person_relations(JsonData, PathPrefix);
          if (person_relation.relation_type  != null)
          {
               person_relation.related_to_id  = id;
               person_relation.HandleRecord();
          }
        }
        public string first_name { get; set; }
        public string mi { get; set; }
        public string last_name { get; set; }
        public string dob { get; set; }
        public string weight { get; set; }
        public string is_male { get; set; }
        public string age { get; set; }
        public string ss { get; set; }
        public string email { get; set; }
        public string marital_status { get; set; }
        public string active { get; set; }


        public void HandleRecord(int InsertUpdate = 0)
            {
                this.InsertUpdateAction(InsertUpdate);
            }
        }
    }