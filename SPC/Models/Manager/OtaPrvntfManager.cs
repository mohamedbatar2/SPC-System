using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPC.DB;

namespace SPC.Models
{
    public class OtaPrvntfManager
    {
        public static async Task ResetOtaPrvntfAsync(string NOutil)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "update OtaPrvntf set DtPre = ?, QtAct = 0 where NOutil = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DtPrev", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public static async Task< OtaPrvntf> GetPrvntfAsync(string NOutil)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select * from OtaPrvntf where NOutil = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    using(OleDbDataReader reader =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new OtaPrvntf()
                            {
                                NOutil = reader["NOutil"] as string,
                                DtPrv = (DateTime)reader["DtPre"],
                                QtAct = (int)reader["QtAct"],
                                PrvQt = (int)reader["PrvQt"]
                            };
                        }
                        return null;
                    }
                }
            }
        }
        public static async Task UpdatePrvntfAsync(string NOutil, int Quantite)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "update OtaPrvntf set QtAct = QtAct + ? where NOutil = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Qt", Quantite);
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
             public static void ResetOtaPrvntf(string NOutil) => ResetOtaPrvntfAsync(NOutil).GetAwaiter().GetResult();
        public static OtaPrvntf GetPrvntf(string NOutil) => GetPrvntfAsync(NOutil).GetAwaiter().GetResult();
        public static void UpdatePrvntf(string NOutil, int Quantite) => UpdatePrvntfAsync(NOutil, Quantite).GetAwaiter().GetResult();
    }
    }

