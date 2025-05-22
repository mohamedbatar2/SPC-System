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
        public static void ResetOtaPrvntf(string NOutil)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "update OtaPrvntf set DtPre = ?, QtAct = 0 where NOutil = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DtPrev", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static OtaPrvntf GetPrvntf(string NOutil)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select * from OtaPrvntf where NOutil = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    using(OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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
        public static void UpdatePrvntf(string NOutil, int Quantite)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "update OtaPrvntf set QtAct = QtAct + ? where NOutil = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Qt", Quantite);
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
