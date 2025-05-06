using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC.DB
{
    public class DBConnexion
    {
        private static string dbname = "SPCApplication";
        private static string host = "localhost";
        public static SqlConnection GetConnexion()
        {
            var conn = new SqlConnection("Server=" + host + "\\SQLEXPRESS;Database=" + dbname + ";Integrated Security=True;");
            return conn;
        }
    }
}
