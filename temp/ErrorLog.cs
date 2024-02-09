using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace WebApiPoster.Models
    {
    public static class ErrorLog
        {

        public static string LogException(Exception e, string LogFileName = "")
            {
            string strMessage = "";
            string strStackTrace = "Stack trace: ";
            string strSource = "Source: ";
     
            if (e.InnerException != null) {
                strMessage += e.InnerException.Message.Trim();
                if (e.InnerException.StackTrace  != null) strStackTrace += e.InnerException.StackTrace.Trim();
                if (e.InnerException.Source  != null) strSource += e.InnerException.Source.Trim();
             
            }
            else {
                strMessage += e.Message.Trim();
                if (e.StackTrace  != null) strStackTrace += e.StackTrace.Trim();
                if (e.Source  != null) strSource += e.Source.Trim();
                
            }
            if (LogFileName == "") LogFileName = AppDomain.CurrentDomain.BaseDirectory + "Errors.log";
            string Outstring = System.Environment.NewLine + DateTime.Now + System.Environment.NewLine + strMessage + ":" + System.Environment.NewLine + strStackTrace + System.Environment.NewLine + strSource + System.Environment.NewLine;
            File.AppendAllText(LogFileName, Outstring);
            return Outstring;
            }
        }
    }