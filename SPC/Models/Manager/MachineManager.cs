using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPC.DB;

namespace SPC.Models
{
    public class MachineManager
    {
        public static async Task<string> GetLibelleAsync(string NMachine)
        {
            if (string.IsNullOrEmpty(NMachine))
            {
                return "";
            }
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select Libelle from machines where NMachine = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NMachine", NMachine);
                    using (OleDbDataReader rdr =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await rdr.ReadAsync())
                        {
                            return rdr[0] as string;
                        }
                         return "";
                    }
                }
            }
        }
        public static async Task<string> GetTypeSPCAsync(string NMachine)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select typeSPC from machines where NMachine = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NMachine", NMachine);
                    using (OleDbDataReader rdr =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await rdr.ReadAsync()) return rdr[0] as string;
                        else return "";
                    }
                }
            }
        }
        public static string GetLibelle(string NMachine) => GetLibelleAsync(NMachine).GetAwaiter().GetResult();
        public static string GetTypeSPC(string NMachine) => GetTypeSPCAsync(NMachine).GetAwaiter().GetResult();
    }
}
