using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApiPoster.Models
{
     public class Cad_Number:BaseClass
     {
          public string cad_number { get; set; }
          public string agency_id { get; set; }
          public string pcr_demographic_id { get; set; }
          public string pcr_disposition_id { get; set; }
          public string call_intake_id { get; set; }
          public string pcr_id { get; set; }
          public string bus_id { get; set; }
          public string firstCrewMember { get; set; }
          public string secondCrewMember { get; set; }
          
          public Cad_Number() { TableName = "cad_number"; }
          public Cad_Number(string id)
               : base(id, "cad_number")
          {

          }

        public bool MapFromPcrJson(string json)
        {
            try
            {

        //         public string cad_number { get; set; }
        //public string agency_id { get; set; }
        //public string pcr_demographic_id { get; set; }
        //public string pcr_disposition_id { get; set; }
        //public string call_intake_id { get; set; }
        //public string pcr_id { get; set; }
        //public string bus_id { get; set; }
        //public string firstCrewMember { get; set; }
        //public string secondCrewMember { get; set; }

//        pcr_id= JsonMaker.GetIOSJsonExtract("$.Demographics");
//cad_number= JsonMaker.GetIOSJsonExtract("$.CAD.cad_id");
                JsonMaker.UpdateJsonValue("$.CAD.cad_id", this.id);
                JsonMaker.UpdateJsonValue("$.CAD.pcr_id", this.pcr_id);
                JsonMaker.UpdateJsonValue("$.CAD.cad_number", this.cad_number);

                Demographic demographic = new Demographic(this.pcr_demographic_id);
                demographic.MapIntoIOSJson();

                Cad_Insurance cad_insurance = new Cad_Insurance();
                cad_insurance.Retrieve(new string[] { "cad_number_id" }, new string[] { this.id });
                cad_insurance.MapIntoIOSJson();

                Disposition disposition = new Disposition(this.pcr_disposition_id);
                disposition.MapIntoIOSJson();

                JsonMaker.UpdateJsonValue("$.Dispatch.DispatchInfo@DispatchInfo_CAD", cad_number);

                StringBuilder Crews = new StringBuilder();
                if (!String.IsNullOrEmpty(firstCrewMember))
                    Crews.Append("'" + firstCrewMember + "'");
                if (!String.IsNullOrEmpty(secondCrewMember))
                {
                    if (Crews.Length > 0) Crews.Append(",");
                    Crews.Append("'" + secondCrewMember + "'");
                }

                if (Crews.Length > 0)
                {
                    string SelectQuery = "(select p.* from users u inner join person p on u.person_id=p.id  where u.id in (" + Crews.ToString() + "))";
                    List<Person> PersonList = Utilities.GetClassList<Person>(SelectQuery);
                    int DriverNo = 1;
                    foreach (Person person in PersonList)
                    {
                        JsonMaker.UpdateJsonValue("$.Dispatch.Members@Members_M" + DriverNo.ToString(), person.last_name + ", " + person.first_name);
                        DriverNo += 1;

                    }
                }
                Call_Intake call_intake = new Call_Intake(this.call_intake_id);
                call_intake.MapIntoDispatchJson();
               


                return true;
            }
            catch (Exception ex) { Logger.LogException(ex); return false; }
        }
        public Cad_Number(object JsonData, string PathPrefix)
          {
               this.TableName = "cad_number";
          }
          public bool MapIntoIOSJson()
          {
               try
               {

                    if (!this.Exists())
                         return false;

                    JsonMaker.UpdateJsonValue("$.CAD.cad_id", this.id);
                    JsonMaker.UpdateJsonValue("$.CAD.pcr_id", this.pcr_id);
                    JsonMaker.UpdateJsonValue("$.CAD.cad_number", this.cad_number);

                    Demographic demographic = new Demographic(this.pcr_demographic_id);
                    demographic.MapIntoIOSJson();

                    Cad_Insurance cad_insurance = new Cad_Insurance();
                    cad_insurance.Retrieve(new string[] { "cad_number_id" }, new string[] { this.id });
                    cad_insurance.MapIntoIOSJson();

                    Disposition disposition = new Disposition(this.pcr_disposition_id);
                    disposition.MapIntoIOSJson();

                    JsonMaker.UpdateJsonValue("$.Dispatch.DispatchInfo@DispatchInfo_CAD", cad_number);

                    StringBuilder Crews = new StringBuilder();
                    if (!String.IsNullOrEmpty(firstCrewMember))  
                         Crews.Append("'" + firstCrewMember + "'");
                    if (!String.IsNullOrEmpty(secondCrewMember)) {
                         if (Crews.Length > 0)  Crews.Append(",");
                         Crews.Append("'" + secondCrewMember + "'");
                    }

                    if (Crews.Length > 0)
                    {
                         string SelectQuery = "(select p.* from users u inner join person p on u.person_id=p.id  where u.id in (" + Crews.ToString() + "))";
                         List<Person> PersonList = Utilities.GetClassList<Person>(SelectQuery);
                         int DriverNo = 1;
                         foreach (Person person in PersonList)
                         {
                              JsonMaker.UpdateJsonValue("$.Dispatch.Members@Members_M" + DriverNo.ToString(), person.last_name + ", " + person.first_name);
                              DriverNo += 1;

                         }
                    }
                    Call_Intake call_intake = new Call_Intake(this.call_intake_id);
                    call_intake.MapIntoDispatchJson();
                 //   call_intake.MapIntoDispositionJson();



                    return true;
               }
               catch (Exception ex) { Logger.LogException(ex); return false; }
          }         

     }
}