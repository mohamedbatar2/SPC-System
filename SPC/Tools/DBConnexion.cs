using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPC.DB
{
    public class DBConnexion
    {
        public static string user = Environment.UserName;
        public static string databasePath = $@"C:\Users\{user}\Desktop\SPCapplication.accdb";
        public static string connexionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};Persist Security Info=False;";
        public static OleDbConnection GetConnexion()
        {
            var conn = new OleDbConnection(connexionString);
            return conn;
        }
    }
}
