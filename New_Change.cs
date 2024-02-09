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

     public class New_Change : PcrBase
     {
          public New_Change(string id)
               : base(id, "New_Change")
          {
          }
          public New_Change(string TableName, JsonInputSection PcrObj)
               : base(TableName, PcrObj)
          {
               
          }
               


          public string Table_name { get; set; }
          public string mypath { get; set; }
          public string device_info { get; set; }
         

          public void HandleRecord(int InsertUpdate = 0)
          {
               this.InsertUpdateAction(InsertUpdate);

          }

     }
}