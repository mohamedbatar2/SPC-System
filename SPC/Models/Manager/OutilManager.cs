using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPC.DB;

namespace SPC.Models
{
    public class OutilManager
    {
        public static ObservableCollection<Outil> GetOutilsByCndO(string nOutil, string cnx)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query;
                if (string.IsNullOrEmpty(nOutil) && string.IsNullOrEmpty(cnx)) return new ObservableCollection<Outil>();
                else if(!string.IsNullOrEmpty(cnx) && string.IsNullOrEmpty(nOutil)) query = "select * from outil where Connexion = ?;";
                else if(!string.IsNullOrEmpty(nOutil) && string.IsNullOrEmpty(cnx)) query = "select * from outil where NOutil = ?;";
                else query = "select * from outil where NOutil = ? and Connexion = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    if (!(string.IsNullOrEmpty(nOutil) || string.IsNullOrEmpty(cnx))) 
                    {
                        cmd.Parameters.AddWithValue("@nOutil", nOutil);
                        cmd.Parameters.AddWithValue("@cnx", cnx);
                    }
                    else if (!string.IsNullOrEmpty(cnx)) cmd.Parameters.AddWithValue("@cnx", cnx);
                    else if (!string.IsNullOrEmpty(nOutil)) cmd.Parameters.AddWithValue("@nOutil", nOutil);

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        var outils = new ObservableCollection<Outil>();
                        while (reader.Read())
                        {
                            outils.Add(new Outil()
                            {
                                code = Convert.ToInt32(reader["code"]),
                                NOutil = reader["NOutil"] as string,
                                Connexion = reader["Connexion"] as string,
                                Sec = reader["sec"] as string,
                                Hame = reader["Hame"] == DBNull.Value ? null : (decimal?)reader["Hame"],
                                TolHa = reader["tolHa"] == DBNull.Value ? null : (decimal?)reader["tolHa"],
                                Hisolant = reader["Hisolant"] == DBNull.Value ? null : (decimal?)reader["Hisolant"],
                                TolHi = reader["TolHi"] == DBNull.Value ? null : (decimal?)reader["TolHi"],
                                TypeFil = reader["TypFil"] as string,
                                Denu = reader["denu"] == DBNull.Value ? null : (decimal?)reader["denu"],
                                Vld = reader["Vld"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Vld"]),
                                Lame = reader["Lame"] == DBNull.Value ? null : (decimal?)reader["Lame"],
                                Lisolant = reader["Lisolant"] == DBNull.Value ? null : (decimal?)reader["Lisolant"],
                                Produit = reader["Produit"] as string,
                                DateFC = reader["DateFC"] != DBNull.Value ? Convert.ToDateTime(reader["DateFC"]) : DateTime.MinValue,
                                Cla = reader["Cla"] == DBNull.Value ? null : (decimal?)reader["Cla"],
                                Bat = reader["Bat"] == DBNull.Value ? null : (decimal?)reader["Bat"],
                                Empl = reader["empl"] as string,
                                Reg = reader["reg"] as string,
                                Ema = reader["rema"] as string,
                                Ph = reader["ph"] as string,
                                Emplcnx = reader["emplcnx"] as string,
                                Clip = reader["clip"] as string,
                                DateSi = reader["DtSai"] != DBNull.Value ? Convert.ToDateTime(reader["DtSai"]) : DateTime.MinValue,
                                Trac = reader["Trac"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Trac"]),
                                UAP = reader["uap"] as string,
                                Zone = reader["zone"] as string
                            });
                        }
                        return outils;
                    }
                }
            }
        }
        public static ObservableCollection<Outil> GetOutils()
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select * from outil;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        var outils = new ObservableCollection<Outil>();
                        while (reader.Read())
                        {
                            outils.Add(new Outil()
                            {
                                code = Convert.ToInt32(reader["code"]),
                                NOutil = reader["NOutil"] as string,
                                Connexion = reader["Connexion"] as string,
                                Sec = reader["sec"] as string,
                                Hame = reader["Hame"] == DBNull.Value ? null : (decimal?)reader["Hame"],
                                TolHa = reader["tolHa"] == DBNull.Value ? null : (decimal?)reader["tolHa"],
                                Hisolant = reader["Hisolant"] == DBNull.Value ? null : (decimal?)reader["Hisolant"],
                                TolHi = reader["TolHi"] == DBNull.Value ? null : (decimal?)reader["TolHi"],
                                TypeFil = reader["TypFil"] as string,
                                Denu = reader["denu"] == DBNull.Value ? null : (decimal?)reader["denu"],
                                Vld = reader["Vld"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Vld"]),
                                Lame = reader["Lame"] == DBNull.Value ? null : (decimal?)reader["Lame"],
                                Lisolant = reader["Lisolant"] == DBNull.Value ? null : (decimal?)reader["Lisolant"],
                                Produit = reader["Produit"] as string,
                                DateFC = reader["DateFC"] != DBNull.Value ? Convert.ToDateTime(reader["DateFC"]) : DateTime.MinValue,
                                Cla = reader["Cla"] == DBNull.Value ? null : (decimal?)reader["Cla"],
                                Bat = reader["Bat"] == DBNull.Value ? null : (decimal?)reader["Bat"],
                                Empl = reader["empl"] as string,
                                Reg = reader["reg"] as string,
                                Ema = reader["rema"] as string,
                                Ph = reader["ph"] as string,
                                Emplcnx = reader["emplcnx"] as string,
                                Clip = reader["clip"] as string,
                                DateSi = reader["DtSai"] != DBNull.Value ? Convert.ToDateTime(reader["DtSai"]) : DateTime.MinValue,
                                Trac = reader["Trac"] == DBNull.Value ? null : (int?)Convert.ToInt32(reader["Trac"]),
                                UAP = reader["uap"] as string,
                                Zone = reader["zone"] as string
                            });
                        }
                        return outils;
                    }
                }
            }
        }
    }
}
