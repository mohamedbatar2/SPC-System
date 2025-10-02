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
        public static async Task<List<string>> GetClientsNamesAsync()
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                var list = new List<string>();
                string query = "select * from Client;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    using (OleDbDataReader reader =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(reader["Client"] as string);

                        }
                        return list;
                    }
                }
            }
        }
        public static async Task AddClientAsync(String clientName)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "insert into client values(?);";
                using(OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@clientname", clientName);
                    await cmd.ExecuteNonQueryAsync();   
                }
            }
        }
        public static List<string> GetClientsNames() => GetClientsNamesAsync().GetAwaiter().GetResult();
        public static void AddClient(string clientName) => AddClientAsync(clientName).GetAwaiter().GetResult();
    }
}
