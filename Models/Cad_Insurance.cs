using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApiPoster.Models
{
     public class Cad_Insurance:BaseClass
     {
  
          public string cad_number_id { get; set; }
          public string pcr_demographic_id { get; set; }
          public string primary_policy { get; set; }
          public string secondary_policy { get; set; }
          public string terciary_insurance { get; set; }

          
          public Cad_Insurance() { TableName = "cad_insurance"; }
          public Cad_Insurance(string id)
               : base(id, "cad_insurance")
          {

          }


          public Cad_Insurance(object JsonData, string PathPrefix)
          {
               this.TableName = "cad_number";
          }
          public bool MapIntoIOSJson()
          {
               try
               {

                    if (primary_policy != null)
                    {
                         Insurance_Policy primary_policy_object = new Insurance_Policy(primary_policy);
                         primary_policy_object.MapIntoIOSJson("$.Demographics.PrimaryInsurance@PrimaryInsurance");
                    }
                    if (secondary_policy != null)
                    {
                         Insurance_Policy secondary_policy_object = new Insurance_Policy(secondary_policy);
                         secondary_policy_object.MapIntoIOSJson("$.Demographics.SecondaryInsurance@SecondaryInsurance");
                    }
                    if (terciary_insurance != null)
                    {
                         Insurance_Policy terciary_insurance_object = new Insurance_Policy(terciary_insurance);
                         terciary_insurance_object.MapIntoIOSJson("$.Demographics.TerciaryInsurance@TerciaryInsurance");
                    }
                    //if (primary_policy_object != null) primary_policy_object.MapIntoIOSJson("$.Demographics.PrimaryInsurance@PrimaryInsurance");
                    //if (secondary_policy_object != null) secondary_policy_object.MapIntoIOSJson("$.Demographics.SecondaryInsurance@SecondaryInsurance");
                    //if (terciary_insurance_object != null) terciary_insurance_object.MapIntoIOSJson("$.Demographics.TerciaryInsurance@TerciaryInsurance");



                    return true;
               }
               catch (Exception ex) { Logger.LogException(ex); return false; }
          }         

     }
}