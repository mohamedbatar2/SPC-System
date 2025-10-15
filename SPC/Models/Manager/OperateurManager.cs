using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using SPC.DB;


namespace SPC.Models
{
    [SupportedOSPlatform("windows")]

    public class OperateurManager
    {
        public static async Task<bool> CheckOpExistAsync(string NOp)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select * from Operateurs where OperationNo = ?;";

                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nop", string.IsNullOrEmpty(NOp) ? DBNull.Value : (object)NOp);

                    using (OleDbDataReader reader = (OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync();
                    }
                }
            }
        }
        public static async Task<string> GetOpNameAsync(string NOp)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion()) {
                await conn.OpenAsync();
                string query = "select Name from Operateurs where OperationNo = ?;";
                using(OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NOp", NOp);
                    using(OleDbDataReader reader = (OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if(await reader.ReadAsync())
                        {
                            return reader[0] as string;
                        }
                        return null;
                    }
                }
            }
        }
        public static bool CheckOpExist(string NOp) => CheckOpExistAsync(NOp).GetAwaiter().GetResult();
        public static string GetOpName(string NOp) => GetOpNameAsync(NOp).GetAwaiter().GetResult();
    }
}
