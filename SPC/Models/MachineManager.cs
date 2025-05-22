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
        public static string GetLibelle(string NMachine)
        {
            if (string.IsNullOrEmpty(NMachine))
            {
                return "";
            }
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select Libelle from machines where NMachine = @NMachine;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NMachine", NMachine);
                    using (OleDbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            return rdr[0] as string;
                        }
                         return "";
                    }
                }
            }
        }
        public static string GetTypeSPC(string NMachine)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select typeSPC from machines where NMachine = @NMachine;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NMachine", NMachine);
                    using (OleDbDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) return rdr[0] as string;
                        else return "";
                    }
                }
            }
        }
    }
}
