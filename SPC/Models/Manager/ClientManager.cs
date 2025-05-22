using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using SPC.DB;

namespace SPC.Models
{
    public class ClientManager
    {
        public static List<string> GetClientsNames()
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                var list = new List<string>();
                string query = "select * from Client;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(reader["Client"] as string);

                        }
                        return list;
                    }
                }
            }
        }
        public static void AddClient(String clientName)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "insert into client values(?);";
                using(OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clientname", clientName);
                    cmd.ExecuteNonQuery();   
                }
            }
        }
    }
}
