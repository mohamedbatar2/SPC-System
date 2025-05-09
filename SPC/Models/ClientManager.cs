using System;
using System.Collections.Generic;
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
            using (SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                var list = new List<string>();
                string query = "select * from Client;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
            using (SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "insert into client values(@clientname);";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clientname", clientName);
                    cmd.ExecuteNonQuery();   
                }
            }
        }
    }
}
