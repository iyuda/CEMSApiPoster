using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiPoster.Models
{
     public class message_inbox: BaseClass
        {

          public string message_id { get; set; }
          public string from_content { get; set; }
          public string message_title { get; set; }
          public string message_content { get; set; }
          public string from_user_id { get; set; }
          public string to_agency_id { get; set; }
          public string to_user_id { get; set; }
          public string to_unit_id { get; set; }
          public string to_bus_id { get; set; }
          public string user_login_id { get; set; }

          public message_inbox() { TableName = "message_inbox"; }
          public message_inbox(string id)
               : base(id, "message_inbox")
               {

               }
          public message_inbox(string TableName, JsonInputSection PcrObj)
               : base(TableName, PcrObj)
               {

               }
          public void MapFromJson(object JsonData)
          {
               try
               {

                    message_id = JsonMaker.GetIOSJsonExtract("$.message_id", JsonData);
                    from_content = JsonMaker.GetIOSJsonExtract("$.from_content", JsonData);
                    message_title = JsonMaker.GetIOSJsonExtract("$.message_title", JsonData);
                    message_content = JsonMaker.GetIOSJsonExtract("$.message_content", JsonData);
                    from_user_id = JsonMaker.GetIOSJsonExtract("$.from_user_id", JsonData);
                    to_agency_id = JsonMaker.GetIOSJsonExtract("$.to_agency_id", JsonData);
                    to_user_id = JsonMaker.GetIOSJsonExtract("$.to_user_id", JsonData);
                    to_unit_id = JsonMaker.GetIOSJsonExtract("$.to_unit_id", JsonData);
                    to_bus_id = JsonMaker.GetIOSJsonExtract("$.to_bus_id", JsonData);

                    HandleRecord();
               }
               catch (Exception ex) { Logger.LogException(ex); }
          }
          public void HandleRecord(int InsertUpdate = 0)
               {
               this.InsertUpdateAction(InsertUpdate);
               }
          }
    }