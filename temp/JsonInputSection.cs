using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Reflection;

namespace WebApiPoster.Models
    {
    public class JsonInputSection
        {
        public JsonInputSection() { }
        public JsonInputSection(string TableName)
            { //, object PcrVar){
            this.TableName =TableName;
            //foreach (PropertyInfo prop in PcrVar.GetType ().GetProperties ()) {
            //this[prop.Name] = prop.GetValue(PcrVar,null).ToString();
            //}
        }
        string TableName;

        private Dictionary<string, string> _dynamicProperties = new Dictionary<string, string>();
         // Index the data here 
        public string this[string key]
        {
            get
            {
                if ( _dynamicProperties.ContainsKey( key ) == false )
                    _dynamicProperties.Add( key, null );
 
                return _dynamicProperties[key]==""?null:_dynamicProperties[key];
            }
            set
            {
                if ( _dynamicProperties.ContainsKey( key ) == false )
                    _dynamicProperties.Add( key, value );
                else
                {
                    _dynamicProperties[key] = value;
                }
            }
        }
       
        }
        }
    