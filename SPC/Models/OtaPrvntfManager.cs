using System;
using System.Collections.Generic;
using System.Data.Common;
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
            using(SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "update OtaPrvntf set DtPre = @DtPrev, QtAct = 0 where NOutil = @NOutil;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DtPrev", DateTime.Now);
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static OtaPrvntf GetPrvntf(string NOutil)
        {
            using(SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select * from OtaPrvntf where NOutil = @NOutil;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    using(SqlDataReader reader = cmd.ExecuteReader())
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
            using(SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "update OtaPrvntf set QtAct = QtAct + @Qt where NOutil = @NOutil;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Qt", Quantite);
                    cmd.Parameters.AddWithValue("@NOutil", NOutil);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
