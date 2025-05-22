using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPC.DB;

namespace SPC.Models
{
    public class SPCEnregCompletManager
    {
        public static ObservableCollection<SPCEnregComplet> GetAll(string stat, string month = "", string year = "")//not Finished == nf fo
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                string query;
                if(string.Equals(stat, "NF")) query = "select * from enreg e, enregdetail d where e.id = d.idenrg and e.NoSerie not in (select se.NoSerie from enreg se, enregdetail sd where se.id = sd.idenrg and Right(sd.Nature, 1 ) = 'F' ) order by NoSerie;";
                else query = "select * from enreg e, enregdetail d where e.id = d.idenrg and year(DateDebut) = ? and month(DateDebut) = ? e.id in (select idenrg from enregDetail where right(Nature, 1) = 'F') order by NoSerie;";
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    if (stat != "NF")
                    {
                        cmd.Parameters.AddWithValue("@year", year);
                        cmd.Parameters.AddWithValue("@month", month);
                    }
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        var enregComplets = new ObservableCollection<SPCEnregComplet>();
                        while (reader.Read())
                        {
                            enregComplets.Add(new SPCEnregComplet()
                            {
                                NoSerie = reader["NoSerie"] as string,
                                RessourceNo = reader["ResourceNo"] as string,
                                OperationNo = reader["OperationNo"] as string,
                                Client = reader["Client"] as string,
                                NoEquipement = reader["NoEquipement"] as string,
                                Section = reader["Section"] as string,
                                NoContact = reader["NoContact"] as string,
                                NoOutil = reader["NoOutil"] as string,
                                NoContact2 = reader["NoContact2"] as string,
                                NoOutil2 = reader["NoOutil2"] as string,
                                UAP = reader["UAP"] as string,

                                HA = reader["HA"] != DBNull.Value ? (decimal?)reader["HA"] : null,
                                HI = reader["HI"] != DBNull.Value ? (decimal?)reader["HI"] : null,
                                Traction = reader["Traction"] != DBNull.Value ? (decimal?)reader["Traction"] : null,
                                LongueurD = reader["LongueurD"] != DBNull.Value ? (decimal?)reader["LongueurD"] : null,
                                LongueurD2 = reader["LongueurD2"] != DBNull.Value ? (decimal?)reader["LongueurD2"] : null,


                                DateCreation = reader["DateDebut"] != DBNull.Value ? (DateTime?)reader["DateDebut"] : null,
                                Quantite = reader["Quantite"] != DBNull.Value ? (decimal?)reader["Quantite"] : null,
                                HA1 = reader["HA1"] != DBNull.Value ? (decimal?)reader["HA1"] : null,
                                HA2 = reader["HA2"] != DBNull.Value ? (decimal?)reader["HA2"] : null,
                                HA3 = reader["HA3"] != DBNull.Value ? (decimal?)reader["HA3"] : null,
                                HI1 = reader["HI1"] != DBNull.Value ? (decimal?)reader["HI1"] : null,
                                HI2 = reader["HI2"] != DBNull.Value ? (decimal?)reader["HI2"] : null,
                                HI3 = reader["HI3"] != DBNull.Value ? (decimal?)reader["HI3"] : null,
                                Traction1 = reader["Traction1"] != DBNull.Value ? (decimal?)reader["Traction1"] : null,
                                Traction2 = reader["Traction2"] != DBNull.Value ? (decimal?)reader["Traction2"] : null,
                                Traction3 = reader["Traction3"] != DBNull.Value ? (decimal?)reader["Traction3"] : null,
                                LongueurM = reader["LongueurM"] != DBNull.Value ? (decimal?)reader["LongueurM"] : null,
                                Denudage1 = reader["Denudage1"] != DBNull.Value ? (decimal?)reader["Denudage1"] : null,
                                Denudage2 = reader["Denudage2"] != DBNull.Value ? (decimal?)reader["Denudage2"] : null,
                                Denudage3 = reader["Denudage3"] != DBNull.Value ? (decimal?)reader["Denudage3"] : null,
                                Tol1 = reader["Tol1"] != DBNull.Value ? (decimal?)reader["Tol1"] : null,
                                Tol2 = reader["Tol2"] != DBNull.Value ? (decimal?)reader["Tol2"] : null,
                                Tol3 = reader["Tol3"] != DBNull.Value ? (decimal?)reader["Tol3"] : null,
                                LongueurM2 = reader["LongueurM2"] != DBNull.Value ? (decimal?)reader["LongueurM2"] : null,
                                Clip1 = reader["Clip1"] as string,
                                Clip2 = reader["Clip2"] as string,
                                Clip3 = reader["Clip3"] as string,
                                Repere = reader["Repere"] as string,
                                Claquage = reader["Claquage"] as string,
                                Marquage = reader["Marquage"] as string,
                                ContactAspect1 = reader["ContactAspect1"] as string,
                                ContactAspect2 = reader["ContactAspect2"] as string,
                                ContactAspect3 = reader["ContactAspect3"] as string,
                                Nature = reader["Nature"] as string,

                                Connexion = reader["Connexion"] as string,
                                Ref = reader["Ref"] as string,
                                NoMachine = reader["NoMachine"] as string,

                                Denudage = reader["Denudage"] != DBNull.Value ? (decimal?)reader["Denudage"] : null,

                                NoOutilB = reader["NoOutilB"] as string,
                                NoOutilC = reader["NoOutilC"] as string,

                                ConnexionB = reader["ConnexionB"] as string,
                                ConnexionC = reader["ConnexionC"] as string,

                                DenudageB = reader["DenudageB"]!= DBNull.Value? (decimal?)reader["DenudageB"] :null,
                                DenudageC = reader["DenudageC"]!= DBNull.Value? (decimal?)reader["DenudageC"] :null,

                                HA1B = reader["HA1B"] != DBNull.Value ? (decimal?)reader["HA1B"] : null,
                                HA2B = reader["HA2B"] != DBNull.Value ? (decimal?)reader["HA2B"] : null,
                                HA3B = reader["HA3B"] != DBNull.Value ? (decimal?)reader["HA3B"] : null,
                                HI1B = reader["HI1B"] != DBNull.Value ? (decimal?)reader["HI1B"] : null,
                                HI2B = reader["HI2B"] != DBNull.Value ? (decimal?)reader["HI2B"] : null,
                                HI3B = reader["HI3B"] != DBNull.Value ? (decimal?)reader["HI3B"] : null,
                                Traction1B = reader["Traction1B"] != DBNull.Value ? (decimal?)reader["Traction1B"] : null,
                                Traction2B = reader["Traction2B"] != DBNull.Value ? (decimal?)reader["Traction2B"] : null,
                                Traction3B = reader["Traction3B"] != DBNull.Value ? (decimal?)reader["Traction3B"] : null,
                                HA1C = reader["HA1C"] != DBNull.Value ? (decimal?)reader["HA1C"] : null,
                                HA2C = reader["HA2C"] != DBNull.Value ? (decimal?)reader["HA2C"] : null,
                                HA3C = reader["HA3C"] != DBNull.Value ? (decimal?)reader["HA3C"] : null,
                                HI1C = reader["HI1C"] != DBNull.Value ? (decimal?)reader["HI1C"] : null,
                                HI2C = reader["HI2C"] != DBNull.Value ? (decimal?)reader["HI2C"] : null,
                                HI3C = reader["HI3C"] != DBNull.Value ? (decimal?)reader["HI3C"] : null,
                                Traction1C = reader["Traction1C"] != DBNull.Value ? (decimal?)reader["Traction1C"] : null,
                                Traction2C = reader["Traction2C"] != DBNull.Value ? (decimal?)reader["Traction2C"] : null,
                                Traction3C = reader["Traction3C"] != DBNull.Value ? (decimal?)reader["Traction3C"] : null,

                                AspectCnx = reader["AspectCnx"] as string,
                                AspectCnxB = reader["AspectCnxB"] as string,
                                AspectCnxC = reader["AspectCnxC"] as string,
                            });
                        }
                        return enregComplets;
                    }
                }
            }
        }
        
        public static string LastSerieByOpStatus(string NOp)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select top 1 Nature from enreg e, enregdetail d where OperationNo = ? and d.idenrg = e.id order by datedebut desc;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NOp", NOp);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string Nature;
                            Nature = reader["Nature"] as string;
                            return Nature;
                        }
                        else return "NO DATA(Fin)";
                    }
                }
            }
        }
    }
}
