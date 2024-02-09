using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using MySql.Data.MySqlClient;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using WebApiPoster.Models;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json.Linq;
using Parse;
using System.Threading.Tasks;
using System.Text;
using System.Drawing;
using System.Web;
using System.IO;
using System.Windows.Forms;
using System.Transactions;
using System.Threading;
using System.Web.SessionState;
using System.Net.Http;
using System.Web.Http.Cors;
using ZOLL.CodeData.SDK;
using ZOLL.CodeData.Parser;


namespace ZOLL.CodeData.SDKHowTo.Parse
{


    /// <summary>
    /// Illustrates how to parse for snapshots
    /// </summary>
    /// 
    public static class Snapshots
    {
        

        /// <summary>
        /// Placeholder for license key
        /// </summary>
        public static string LicenseKey { get; set; }
        
        /// <summary>
        /// Illustrates how to parse for snapshots
        /// </summary>
        /// <param name="filePath">Path to incident or snapshot file (JSON)</param>
        /// <returns>Sample text describing snapshots</returns>
        public static string ParseSnapshots(string filePath)
        {
  
            // Set license key
            new License.License().Key = "";

            // Write results to string
            var output = new StringBuilder();

            // Create a parser for input file that contains snapshots. Snapshots can be saved individually or appear
            // as part of an incident, both of which are saved as JSON files.
            var parser = new ZOLL.CodeData.Parser.ParserFactory().CreateParser(filePath);

            // Split input file into individual incidents (this should be done even if the input file
            // only has a single incident in it)
            var incidentFiles = parser.ImageToFiles(filePath, Path.GetTempPath());

            // Important Note: snapshots can appear in files all by themselves (i.e. outside the case in which they
            // were recorded) in which case calling ImageToFiles is not necessary.

            // The IDefibParserExtended interface and ParseResults class can be used together
            // for "a la carte" parsing. Individual types of parse results can be requested
            // by initializing various members of the ParseResults class. 
            // For instance, to get events, initialize the Events property to an empty list.
            var extendedParser = (IDefibParserExtended)parser;

            foreach (var incidentFile in incidentFiles)
            {
                // Use the ParseResults class to define what data you want - the following will request
                // snapshots in addition to some data that is always available, such as defibrillator 
                // configuration, etc.
                var parseResults = new ParseResults
                {
                    Snapshots = new List<ISnapshot>()
                };

                // Parse
                extendedParser.Parse(incidentFile, ref parseResults);

                // Show results
                output.AppendLine(@"Defibrillator configuration:");
                output.AppendLine(parseResults.DefibConfig.ToString());

                output.AppendLine(@"Snapshots:");
                foreach (var datum in parseResults.Snapshots)
                {
                    output.AppendLine(datum.ToString());
                }

                // Clean up temporary file
                File.Delete(incidentFile);
            }
            return output.ToString();
        }
    }
}