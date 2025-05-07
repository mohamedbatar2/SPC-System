using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPC.DB;
using SPC.Tools;

namespace SPC.Models
{
    public class OperateurManager
    {
        public static bool CheckOpExist(string NOp)
        {
            using(SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select * from Operateurs where OperationNo = @NOp;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nop", string.IsNullOrEmpty(NOp)? DBNull.Value : (object)NOp);

                    using (SqlDataReader reader = cmd.ExecuteReader()) 
                    {
                        return reader.Read();
                    }
                }
            }
        }
        public static string GetOpName(string NOp)
        {
            using(SqlConnection conn = DBConnexion.GetConnexion()) {
                conn.Open();
                string query = "select Name from Operateurs where OperationNo = @NOp;";
                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NOp", NOp);
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            return reader[0] as string;
                        }
                        return null;
                    }
                }
            }
        }
    }
}
