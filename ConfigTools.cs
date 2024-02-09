using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml;

namespace WebApiPoster
{
    public class ConfigTools
    {
        private static string getConfigFilePath()
        {
            return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; // Assembly.GetExecutingAssembly().Location + ".config";
        }
        public static string ReadSetting(string key)
        {
            return ConfigurationSettings.AppSettings[key];
        }
        public static string GetConfigValue(string key, string default_value)
        {
            dynamic value = ConfigurationManager.ConnectionStrings[key];
            if (value == null) value = "";
            if (value.ToString() == "")
            {
                value = default_value;
                ConfigTools.WriteConfigValue(key, value);
            }
            return value.ToString();
        }

        private static XmlDocument loadConfigDocument()
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(getConfigFilePath());
                return doc;
            }
            catch (System.IO.FileNotFoundException e)
            {
                return null;
                //throw new Exception("No configuration file found.", e);
            }
        }

        public static void WriteConfigValue(string key, string value)
        {
            // load config document for current assembly
            XmlDocument doc = loadConfigDocument();

            // retrieve appSettings node
            XmlNode node = doc.SelectSingleNode("//connectionStrings");

            if (node == null)
                //   throw new InvalidOperationException("appSettings section not found in config file.");
                return;

            try
            {
                // select the 'add' element that contains the key
                XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

                if (elem != null)
                {
                    // add value for key
                    elem.SetAttribute("value", value);
                }
                else
                {
                    // key was not found so create the 'add' element 
                    // and set it's key/value attributes 
                    elem = doc.CreateElement("add");
                    elem.SetAttribute("name", key);
                    elem.SetAttribute("connectionString", value);
                    node.AppendChild(elem);
                }
                doc.Save(getConfigFilePath());
            }
            catch
            {
                //throw;
            }
        }
    }
}