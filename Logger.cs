using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
namespace WebApiPoster.PCR
    {
    public static class Logger
        {
        public static string LogAction(string strMessage, string LogFileName = "ActionQueries")
            {
            string LogsDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(LogsDirectory)) Directory.CreateDirectory(LogsDirectory);

            if (LogFileName != "")
                LogFileName = LogsDirectory + LogFileName + ".log";
            else
                LogFileName = LogsDirectory + "Exceptions.log";

            string Outstring = System.Environment.NewLine + DateTime.Now + System.Environment.NewLine + strMessage  + System.Environment.NewLine;
            File.AppendAllText(LogFileName, Outstring);
            return Outstring;
            }
        public static string LogError(string strMessage, string LogFileName = "Errors")
            {

            string LogsDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists(LogsDirectory)) Directory.CreateDirectory(LogsDirectory);

            if (LogFileName != "")
                LogFileName = LogsDirectory + LogFileName + ".log";
            else
                LogFileName = LogsDirectory + "Exceptions.log";
            string Outstring = System.Environment.NewLine + DateTime.Now + System.Environment.NewLine + strMessage + System.Environment.NewLine;
            File.AppendAllText(LogFileName, Outstring);
            return Outstring;
            }
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
            string LogsDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            if (!Directory.Exists (LogsDirectory)) Directory.CreateDirectory (LogsDirectory);

            if (LogFileName != "") 
                LogFileName = LogsDirectory + LogFileName + ".log";
            else
                LogFileName = LogsDirectory + "Exceptions.log";
            //"Connection string: " + DbConnect.ConnectionString +  
            string Outstring = System.Environment.NewLine + DateTime.Now + System.Environment.NewLine + strMessage + ":" + System.Environment.NewLine + strStackTrace + System.Environment.NewLine + strSource + System.Environment.NewLine;
            File.AppendAllText(LogFileName, Outstring);
            return Outstring;
            }
        }
    }