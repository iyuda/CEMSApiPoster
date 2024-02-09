using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json.Linq;
namespace WebApiPoster.Models
    {
    
    public class Assessment:BaseClass 
        {
        public Assessment(string id): base(id,"pcr_Assessment")
            {

            }
        public Assessment()
        {
             this.TableName = "pcr_Assessment";

        }
        public Assessment(string TableName, JsonInputSection PcrObj)
            {
            this.TableName = TableName;
            this.PcrSection = PcrObj;
            foreach (var prop in this.GetType().GetProperties().Where (prop => prop.PropertyType.FullName.StartsWith("System.")))
                {
                string OutValue;
                switch (prop.Name)
                    {
                    case "burn_1st":
                    case "burn_2nd":
                    case "burn_3rd":
                    case "total_burn":
                        if (!Utilities.IsNumeric(PcrObj[prop.Name])) OutValue = null; else OutValue = PcrObj[prop.Name];
                        break;
                    default:
                        OutValue=PcrObj[prop.Name];
                        break;
                    }
                prop.SetValue(this, OutValue);
                }
            //this.Assessment_facility_id = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            //this.address_id = objAssessment.cad;
            //this.out_of_area = objAssessment.transported_from;
            //this.out_of_area_exists = null;
           
            }

        public string chief_complaint { get; set; }
        public string medications { get; set; }
        public string burn_1st { get; set; }
        public string burn_2nd { get; set; }
        public string burn_3rd { get; set; }
        public string total_burn { get; set; }
        public string onset { get; set; }
        public string minutes { get; set; }
        public string hours { get; set; }
        public string days { get; set; }
        public string weeks { get; set; }
        public string months { get; set; }
        public string neckScribble { get; set; }
        public string abdomenScribble { get; set; }
        public string chestScribble { get; set; }
        public string rightHandScribble { get; set; }
        public string leftHandScribble { get; set; }
        public string rightFootScribble { get; set; }
        public string leftFootScribble { get; set; }
        public string backScribble { get; set; }
        public string groinScribble { get; set; }
        private static Dictionary<string, string> ChiefComplaints = new Dictionary<string, string>();

        static Assessment()
        {
             InitChiefComplaints();
        }
        public static void InitChiefComplaints()
        {
             ChiefComplaints.Clear();

             ChiefComplaints.Add("Abdomen (blunt)", "BA");
             ChiefComplaints.Add("Abdomen (penetration)","PA");
             ChiefComplaints.Add("Abdominal/Pelvic Pain","AP");
             ChiefComplaints.Add("Agitated Delirium","AD");
             ChiefComplaints.Add("Allergic Reaction","AR");
             ChiefComplaints.Add("Altered LOC","AL");
             ChiefComplaints.Add("Amp. ↑ wrist/ankle (blunt)","BI");
             ChiefComplaints.Add("Amp. ↑ wrist/ankle (penetration)","PI");
             ChiefComplaints.Add("Apneic Episode","AE");
             ChiefComplaints.Add("Apparent Life Threatening Event (A.L.T.E)","TE");
             ChiefComplaints.Add("Back (blunt)","BB");
             ChiefComplaints.Add("Back (penetration)","PB");
             ChiefComplaints.Add("Behavioral","EH");
             ChiefComplaints.Add("Bleeding Other Site","OS");
             ChiefComplaints.Add("Blunt Head","BH");
             ChiefComplaints.Add("Blunt Head Injury w/GCS <= 14","14");
             ChiefComplaints.Add("Burns/Shock","BU");
             ChiefComplaints.Add("Buttocks (blunt)","BK");
             ChiefComplaints.Add("Buttocks (penetration)","PK");
             ChiefComplaints.Add("Cardiac Arrest","CA");
             ChiefComplaints.Add("Chest (blunt)","BC");
             ChiefComplaints.Add("Chest (penetration)","PC");
             ChiefComplaints.Add("Chest Pain","CP");
             ChiefComplaints.Add("Choking/Airway Obstruction","CH");
             ChiefComplaints.Add("Cough/Congestion","CC");
             ChiefComplaints.Add("DOA (Dead On Arrival)","DO");
             ChiefComplaints.Add("Diffuse Abdominal Tenderness","BD");
             ChiefComplaints.Add("Dizzy","DI");
             ChiefComplaints.Add("Dysrhythmia","DY");
             ChiefComplaints.Add("Ext. ↑ knee/elbow","PX");
             ChiefComplaints.Add("Extremities (blunt)","BE");
             ChiefComplaints.Add("Extremities (penetration)","PE");
             ChiefComplaints.Add("Face/Mouth (blunt)","BF");
             ChiefComplaints.Add("Face/Mouth (penetration)","PF");
             ChiefComplaints.Add("Fever","FE");
             ChiefComplaints.Add("Flail Chest","FC");
             ChiefComplaints.Add("Foreign Body","FB");
             ChiefComplaints.Add("Fractures ≥ 2 long","BR");
             ChiefComplaints.Add("GastroIntestinal Bleed","GI");
             ChiefComplaints.Add("Genitals (blunt)","BG");
             ChiefComplaints.Add("Genitals (penetration)","PG");
             ChiefComplaints.Add("Head Pain","HP");
             ChiefComplaints.Add("Hypoglycemia","HY");
             ChiefComplaints.Add("Inpatient Medical","IM");
             ChiefComplaints.Add("Inpatient Trauma", "IT");
             ChiefComplaints.Add("Labor","LA");
             ChiefComplaints.Add("Local Neurological Signs","LN");
             ChiefComplaints.Add("Medical Device Complaint","DC");
             ChiefComplaints.Add("Minor Laceration/Contusion/Abrasion (blunt)","BL");
             ChiefComplaints.Add("Minor Laceration/Contusion/Abrasion (penetration)","PL");
             ChiefComplaints.Add("Nausea/Vomiting","NV");
             ChiefComplaints.Add("Near Drowning","ND");
             ChiefComplaints.Add("Neck (blunt)","BN");
             ChiefComplaints.Add("Neck (penetration)","PN");
             ChiefComplaints.Add("Neck/Back Pain","NB");
             ChiefComplaints.Add("Neuro/Vasc./Mangled (blunt)","BV");
             ChiefComplaints.Add("Neuro/Vasc./Mangled (penetration)","PV");
             ChiefComplaints.Add("Newborn","NW");
             ChiefComplaints.Add("No Apparent Injuries","NA");
             ChiefComplaints.Add("No Medical Complaint","NC");
             ChiefComplaints.Add("Nose Bleed","NO");
             ChiefComplaints.Add("Obstetrics","OB");
             ChiefComplaints.Add("Other","OT");
             ChiefComplaints.Add("Other Pain","OP");
             ChiefComplaints.Add("Overdose","OD");
             ChiefComplaints.Add("Palpitations","PS");
             ChiefComplaints.Add("Penetrating Head","PH");
             ChiefComplaints.Add("Poisoning","PO");
             ChiefComplaints.Add("Respiratory Arrest","RA");
             ChiefComplaints.Add("Respiratory rate <10/>29","RR");
             ChiefComplaints.Add("SBP <90, <70 (<1 yr)","90");
             ChiefComplaints.Add("Seizure","SE");
             ChiefComplaints.Add("Shortness of Breath","SB");
             ChiefComplaints.Add("Spinal Cord with Deficit","SC");
             ChiefComplaints.Add("Suspected Pelvic Fracture","SX");
             ChiefComplaints.Add("Syncope","SY");
             ChiefComplaints.Add("Tension Pneumo. (blunt)","BP");
             ChiefComplaints.Add("Tension Pneumo. (penetration)","PP");
             ChiefComplaints.Add("Traumatic Arrest (blunt)","BT");
             ChiefComplaints.Add("Traumatic Arrest (penetration)","PT");
             ChiefComplaints.Add("Vaginal Bleed","VA");
             ChiefComplaints.Add("Weak","WE");
        }
        public void HandleRecord(int InsertUpdate = 0)
            {
            this.InsertUpdateAction(InsertUpdate);

            }
                                    
        public void MapIntoIOSJson()
        {
             try
             {
                  if (!String.IsNullOrEmpty(this.medications)) {
                       int i = 1;
                       foreach (string medication in medications.Split(','))
                       {
                            string[] medication_array = medication.Split(' ');
                            JsonMaker.UpdateJsonValue("$.GEN:assessmentmedications.AssessmentMedication_" + i.ToString() + "@AssessmentMedication_" + i.ToString() + "_Medication", medication_array[0]);
                            if (medication_array.Length > 1)
                            {
                                 JsonMaker.UpdateJsonValue("$.GEN:assessmentmedications.AssessmentMedication_" + i.ToString() + "@AssessmentMedication_" + i.ToString() + "_Dose",  medication_array[1]);
                            }
                            if (medication_array.Length > 2)
                            {
                                 JsonMaker.UpdateJsonValue("$.GEN:assessmentmedications.AssessmentMedication_" + i.ToString() + "@AssessmentMedication_" + i.ToString() + "_Frequency", medication_array[2]);
                            }
                      
                       }
                  }
                  if (!String.IsNullOrEmpty(this.chief_complaint))
                  {
                       JsonMaker.UpdateJsonValue("$.GEN:divChiefComplaintEmergency.ChiefComplaint@ChiefComplaint_ChiefComplaint", this.chief_complaint);
                  }


             }
             catch (Exception ex) { Logger.LogException(ex); }
        }
        public void MapFromIOSJson(object JsonData)
        {
             try
             {
                 
                 // StringBuilder Assessments= new StringBuilder();
                  string medication;
                  string dose;
                  string frequency;
                  string assessment;
                  List<string> Assessments = new List<string>();
                  for (int i=1; i<20; i++) {
                       assessment="";
                       medication = JsonMaker.GetIOSJsonExtract("$.GEN:assessmentmedications.AssessmentMedication_" + i.ToString() + "@AssessmentMedication_" + i.ToString() + "_Medication", JsonData);
                       if (medication!=null)
                            assessment+=medication+" ";
                       if (assessment.Trim()!="") {
                            dose = JsonMaker.GetIOSJsonExtract("$.GEN:assessmentmedications.AssessmentMedication_" + i.ToString() + "@AssessmentMedication_" + i.ToString() + "_Dose", JsonData);
                            if (dose!=null)
                                 assessment+=dose.ToLower().Replace("mg","")+" ";
                            frequency = JsonMaker.GetIOSJsonExtract("$.GEN:assessmentmedications.AssessmentMedication_" + i.ToString() + "@AssessmentMedication_" + i.ToString() + "_Frequency", JsonData);
                            if (frequency!=null)
                                 assessment+=frequency+" ";
                            assessment=assessment.Trim();
                       }
                       if (assessment!="") Assessments.Add (assessment);
                  }
                  this.medications=string.Join(",", Assessments.ToArray());
                  this.chief_complaint = JsonMaker.GetIOSJsonExtract("$.GEN:divChiefComplaintEmergency.ChiefComplaint@ChiefComplaint_ChiefComplaint", HttpContext.Current.Session["json_in"]);

                  //foreach (JToken token in ((IEnumerable<JToken>)JsonData).Children().Where(t => t.Path.StartsWith("AssessmentMedication")))
                  //{
                  //     if (token.Path.Split('_').Length == 0) continue;
                       
                  //     AssessmentNo = token.Path.Split('_')[1].Split('.')[0];
                  //     if (token.Path.Contains("."))
                  //     {
                  //          foreach (JToken sub_token in ((IEnumerable<JToken>)token).Children())
                  //          {
                  //               var ButtonsArray = (IEnumerable<JToken>)JsonMaker.GetIOSJsonExtract("$.model.buttons", sub_token.ToString());
                  //               if (ButtonsArray != null)
                  //                    foreach (JToken button in ButtonsArray)
                  //                    {
                  //                         Assessments.Append(JsonMaker.GetIOSJsonExtract("$.str", button.ToString()).ToString());
                  //                    }
                  //               }
                  //          }
                  //     }
                  //}
                
                  HandleRecord();
             }
             catch (Exception ex) { Logger.LogException(ex); }
        }



        }


    }