using SendPdfFax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using WebApiPoster.Models;
using System.Configuration;
using System.Security.Principal;

namespace WebApiPoster
{
     public class CreateFax
     {

          public static string sendEmailFax(string Filename, string PhoneNumber, string FromAgencyName)
          {
               var fromAddress = new MailAddress("Fax@creativeems.com", "From Creative Fax");
               string FaxTo = string.Format("{0}@metrofax.com", PhoneNumber);
               var toAddress = new MailAddress(FaxTo, "To Facility");
               const string fromPassword = "support4732.";
               string subject = "Run Sheet From " + FromAgencyName;// this is RE in email
               const string body = @"The document(s) accompanying this fax contain(s) confidential information which is legally privileged. The information is intended only for the use of the intended recipient/institution named above. If you are not the intended recipient you are hereby notified that any reading, disclosure, copying, distribution or taking of any action in reliance on the contents of the telecopied information except in direct delivery to the intended recipient names above is strictly prohibited. If you have received this fax in error, please notify us immediately by the above phone number to arrange proper disposal of the original documents. ";

               var smtp = new SmtpClient
               {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
               };
               var message = new MailMessage(fromAddress, toAddress);
               message.Subject = subject;
               message.Body = body;
               message.Attachments.Add(new System.Net.Mail.Attachment(Filename));
               //  message.BodyEncoding = System.Text.Encoding.UTF8;
               //  message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
               string status = "OK";
               try
               {
                    smtp.Send(message);
                    Console.WriteLine("Fax successfully sent.");
               }
               catch (Exception ex)
               {
                    File.AppendAllText(Directory.GetCurrentDirectory() + "FaxlogError.txt", DateTime.Now.ToString() + " Error sending fax for: " + HttpContext.Current.Session["pcr_id"] + "\r\n");
                    Console.WriteLine("Failure Sending Fax.");
                    status= ex.Message;
               }

               message.Dispose();
               return status;
          }

          public static void SendTestEmailFax(string Filename, string PhoneNumber, string FromAgencyName, MailAddress fromAddress, MailAddress toAddress, string fromPassword)
          {

               string subject = "Run Sheet From " + FromAgencyName;// this is RE in email
               const string body = @"The document(s) accompanying this fax contain(s) confidential information which is legally privileged. The information is intended only for the use of the intended recipient/institution named above. If you are not the intended recipient you are hereby notified that any reading, disclosure, copying, distribution or taking of any action in reliance on the contents of the telecopied information except in direct delivery to the intended recipient names above is strictly prohibited. If you have received this fax in error, please notify us immediately by the above phone number to arrange proper disposal of the original documents. ";

               var smtp = new SmtpClient
               {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
               };
               var message = new MailMessage(fromAddress, toAddress);
               message.Subject = subject;
               message.Body = body;
               message.Attachments.Add(new System.Net.Mail.Attachment(Filename));
               //  message.BodyEncoding = System.Text.Encoding.UTF8;
               //  message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
               try
               {
                    smtp.Send(message);
                    Console.WriteLine("Fax successfully sent.");
               }
               catch (Exception)
               {
                    File.AppendAllText(Directory.GetCurrentDirectory() + "FaxlogError.txt", DateTime.Now.ToString() + " Fax successfully sent. " + HttpContext.Current.Session["pcr_id"] + "\r\n");
                    Console.WriteLine("Failure Sending Fax.");
               }

               message.Dispose();
          }



          public static void SendPdfFax(string Filename, string PhoneNumber)
          {
               string[] filename = { Filename };
               PhaxioAPI phaxio = new PhaxioAPI("6f0ba776d75ab814c3db1b87bee476ad0cbeffa0", "fa24aa47aef9a7fd674a6332624fc9259a48a97a");
               Dictionary<string, string> options = new Dictionary<string, string>();
               PhaxioOperationResult result = phaxio.sendFax(new string[1] { PhoneNumber }, filename, options);

               if (result.Success)
               {
                    File.AppendAllText(Directory.GetCurrentDirectory() + "Faxlog.txt", DateTime.Now.ToString() + " Fax successfully sent. " + HttpContext.Current.Session["pcr_id"] + "\r\n");
                    Console.WriteLine("Fax successfully sent.");
               }

          }


          protected void Capture(object sender, EventArgs e)
          {
               
          }

          private static void DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
          {
            Logger.LogAction("Document Completed", "Activity");
            browse_done = true;
            //WebBrowser browser = sender as WebBrowser;
            //using (Bitmap bitmap = new Bitmap(browser.Width, browser.Height))
            //{
            //     browser.DrawToBitmap(bitmap, new Rectangle(0, 0, browser.Width, browser.Height));
            //     using (MemoryStream stream = new MemoryStream())
            //     {
            //          bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            //          byte[] bytes = stream.ToArray();
            //          imgScreenShot.Visible = true;
            //          imgScreenShot.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(bytes);
            //     }
            //}
        }
       private void WaitForFile(string SourceFileName, int iMilliSeconds)
       {
             int iHalfSeconds = iMilliSeconds / 500;
             for (int i=1; i<= iHalfSeconds; i++) {
                 System.Threading.Thread.Sleep(500);
                 System.Windows.Forms.Application.DoEvents();
                 if (File.Exists(SourceFileName))
                      break;
             }
       }
        static bool browse_done = false;
       
        static void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {

            Logger.LogAction("Document Navigated", "Activity");
            browse_done = true;
        }
        public static void StartBrowser(object UrlOfReport)
       {

            using (WebBrowser browser = new WebBrowser())
            {
                browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
                browser.Navigated += new WebBrowserNavigatedEventHandler(webBrowser_Navigated);
                browse_done = false;
                Logger.LogAction("Start Navigate Thread", "Activity");
                browser.Visible = true;
                 browser.ScrollBarsEnabled = false;
                 browser.AllowNavigation = true;
                 browser.Navigate(UrlOfReport.ToString());
                 browser.Width = 1024;
                 browser.Height = 768;
                browser.ScriptErrorsSuppressed = true;
//                browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(DocumentCompleted);
                System.Threading.Thread.Sleep(5000);
                Logger.LogAction("browse_done: " + browse_done.ToString(), "Activity");
                System.Diagnostics.Stopwatch stopwatch = Stopwatch.StartNew();
                 while (!browse_done)
                 {
                    System.Windows.Forms.Application.DoEvents();
                    System.Threading.Thread.Sleep(1000);
                    if (stopwatch.Elapsed.TotalSeconds > 20)
                    {
                        Logger.LogAction("More than 20 seconds", "Activity");
                        break;
                    }
                 }
                Logger.LogAction("loop done", "Activity");
                Logger.LogAction("browse_done: " + browse_done.ToString(), "Activity");
                stopwatch.Stop();
                 
            }
       }



          public string CreateFile(string pcr_id, string UrlOfReport)
          {

            dynamic OnlinePath = ConfigurationManager.ConnectionStrings["GetReportUrl"];
            if (OnlinePath == null) OnlinePath = "";
            if (OnlinePath.ToString()=="")
                OnlinePath = "http://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/Report";
            dynamic SourceFileName = ConfigurationManager.ConnectionStrings["PDFReportsFilePath"];
            if (SourceFileName == null) SourceFileName = "";
            if (SourceFileName.ToString() == "")
                SourceFileName = "http://www.creativeems.com/cems/cemslocal/yiicems/Reports/ReportFiles/";
            SourceFileName +="/PcrReport_" + pcr_id + ".pdf";
            Logger.LogAction("Breakpoint_a", "Breakpoints");
            Logger.LogAction("Running url " + UrlOfReport, "Activity");

            string BrowserPath = ConfigTools.GetConfigValue("BrowserPath", "c:\\Program Files\\Internet Explorer\\iexplore.exe");
            
           
            Logger.LogAction("breakpoint_b", "Breakpoints");


            Process proc = new Process();

            Logger.LogAction(BrowserPath+ UrlOfReport, "Breakpoints");
            proc.StartInfo = new ProcessStartInfo(BrowserPath, UrlOfReport);
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.CreateNoWindow = false;
            proc.StartInfo.UseShellExecute = true;
          
            Logger.LogAction("breakpoint_c", "Breakpoints");
           
            proc.Start();
            Logger.LogAction("breakpoint_d", "Breakpoints");
            Logger.LogAction(proc.Id.ToString(), "Breakpoints");
            Logger.LogAction("breakpoint_e", "Breakpoints");
            
            Logger.LogAction("Waiting for file " + SourceFileName, "Activity");
            WaitForFile(SourceFileName, 20000);
            
               Logger.LogAction("Breakpoint_d", "Breakpoints");
               string FaxDirectory = "c:\\Creativeems\\Fax\\";
               if (!Directory.Exists(FaxDirectory)) Directory.CreateDirectory(FaxDirectory);
               string DestFileName = FaxDirectory + "\\PcrReport_" + pcr_id + ".pdf";
               if (File.Exists(DestFileName)) File.Delete(DestFileName);
               try
               {
                    Logger.LogAction("Breakpoint_e", "Breakpoints");
                    WebClient client = new WebClient();
                    Logger.LogAction("Breakpoint_f", "Breakpoints");
                    Uri uri = new Uri(SourceFileName);
                    Logger.LogAction("Breakpoint_g", "Breakpoints");
                    client.DownloadFile(uri, DestFileName);
                    Logger.LogAction("Breakpoint_h", "Breakpoints");
                
                }
               catch { }
                Logger.LogAction("Breakpoint_i", "Breakpoints");
                try
                {
                    proc.Kill();
                }
            catch { }
            Logger.LogAction("Breakpoint_j", "Breakpoints");
            if (File.Exists(DestFileName)) {
                    //Logger.LogAction("Created File " + DestFileName, "Activity");
                    return (DestFileName);
               }
               else
               {
                    //Logger.LogAction("Created File " + DestFileName, "Activity");
                    return (null);
               }
               //if (!UseIReport)
               //{
               //string PageSize = "7in*8in";
               //string argument1 = "rasterize.js";
               //string Path = @"phantomjs.exe";
               //string argument = string.Format(@"{0} ""{1}"" ""{2}""", argument1, UrlOfReport, PdfFilename);
               //Process p = new Process();
               //p.StartInfo = new ProcessStartInfo(Path, " --ignore-ssl-errors=yes " + argument);
               //p.StartInfo.CreateNoWindow = true;
               //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
               //p.StartInfo.UseShellExecute = false;
               //p.Start();
               //p.WaitForExit();
               //}
               //else
               //{
               //     string Path = @"Reports.exe";
               //     string argument = string.Format(@"{0} ""{1}"" ""{2}""", argument1, UrlOfReport, PdfFilename);
               //}

              

          }
          public static string Geturl_test(string ReportType)
          {
               if (ReportType == "pcr" || ReportType == "pcr_Addendum")
               {
                    string Onlinepath = @"https://www.creativeemstest.com/cems/cemslocal/yiicems/index.php?r=Central/Report&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "pcs" || ReportType == "pcs_Addendum")
               {
                    string Onlinepath = @"https://www.creativeemstest.com/cems/cemslocal/yiicems/index.php?r=Central/ApcfReport&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "aob" || ReportType == "aob_Addendum")
               {
                    string Onlinepath = @"https://www.creativeemstest.com/cems/cemslocal/yiicems/index.php?r=Central/HipaaReport&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "rma" || ReportType == "rma_Addendum")
               {
                    string Onlinepath = @"https://www.creativeemstest.com/cems/cemslocal/yiicems/index.php?r=Central/RmaReport&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "total")
               {
                    string Onlinepath = @"https://www.creativeemstest.com/cems/cemslocal/yiicems/index.php?r=Central/RmaReport&pcrid=";
                    return Onlinepath;
               }

               return "";
          }

          public static string Geturl(string ReportType)
          {
               if (ReportType == "pcr" || ReportType == "pcr_Addendum")
               {
                    string Onlinepath = @"https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/Report&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "pcs" || ReportType == "pcs_Addendum")
               {
                    string Onlinepath = @"https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/ApcfReport&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "aob" || ReportType == "aob_Addendum")
               {
                    string Onlinepath = @"https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/HipaaReport&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "rma" || ReportType == "rma_Addendum")
               {
                    string Onlinepath = @"https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/RmaReport&pcrid=";
                    return Onlinepath;
               }
               if (ReportType == "total")
               {
                    string Onlinepath = @"https://www.creativeems.com/cems/cemslocal/yiicems/index.php?r=Central/RmaReport&pcrid=";
                    return Onlinepath;
               }

               return "";
          }


     }
}