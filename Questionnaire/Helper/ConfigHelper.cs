using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Questionnaire.Helper
{
    public class ConfigHelper
    {
        private const string _DataBase = "QuerySystem";

        public static string GetConnectionString()
        {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString;
            return conn;
            //return GetConnectionString(_DataBase);
        }
        public static string GetConnectionString(string database)
        {
            string connString = $"Server=localhost;Database={database};Integrated Security=True";
            return connString;
        }
    }
}