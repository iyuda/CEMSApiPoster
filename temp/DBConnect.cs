using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace MysqlFunctions
{
    public class DbConnect
    {

        public static string DatabaseNewcems = "newcems";
	   public static string Server = "localhost";
	   public static uint Port = 3306;
	   public static string Username = "root";
	   public static string Pass = "";

		
        public static MySqlConnection PreConnect()
        {
            return connection(Server, Port, DatabaseNewcems, Username, Pass);
        }

        public static string GetFixedConnectionString(string ConnectionString)
        {

            //Server, Port, DatabaseNewcems, Username, Pass
            MySqlConnection Conn = new MySqlConnection();
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(ConnectionString);
            builder.OldGuids = true;
            builder.AllowZeroDateTime = false;
            builder.ConvertZeroDateTime = true;
            builder.ConnectionTimeout = 72000;
            builder.DefaultCommandTimeout = 72000;
            return builder.GetConnectionString(true);
        }

        public static MySqlConnection connection(string server, uint Port, string database, string UserID, string Pass)
        {
            //Server, Port, DatabaseNewcems, Username, Pass
            MySqlConnection Conn = new MySqlConnection();
            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.UserID = UserID;
            builder.Password = Pass;
            builder.Port = Port;
            builder.Database = database;
            builder.OldGuids = true;
            builder.AllowZeroDateTime = false;
            builder.ConvertZeroDateTime = true;
            builder.ConnectionTimeout = 72000;
            builder.DefaultCommandTimeout = 72000;
            Conn.ConnectionString = builder.GetConnectionString(true);
            return Conn;
        }

        public static void Select(string SelectQuery)
        {

            DataTable dt = new DataTable();
            MySqlDataAdapter Adapter;
            Adapter = new MySqlDataAdapter(SelectQuery, PreConnect());
            MySqlCommandBuilder cb = new MySqlCommandBuilder(Adapter);
            Adapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {

                var name = dt.Rows[0].Field<string>("Name");
                Console.WriteLine(name);
                // Agency_id = dt.Rows[0].Field<string>("agency_id");
            }
            dt.Dispose();
            Adapter.Dispose();

        }
    }
}
