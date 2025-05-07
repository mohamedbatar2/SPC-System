using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SPC.DB;

namespace SPC.Models
{
    public class EnregCompletManager
    {
        public static ObservableCollection<EnregComplet> GetAll()
        {
            using (SqlConnection conn = DBConnexion.GetConnexion())
            {
                string query = "select * from enreg e, enregdetail d where e.id = d.idenrg;";
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        var enregComplets = new ObservableCollection<EnregComplet>();
                        while (reader.Read())
                        {
                            enregComplets.Add(new EnregComplet()
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

                                AH = reader["AH"] != DBNull.Value ? (decimal?)reader["AH"] : null,
                                FH = reader["FH"] != DBNull.Value ? (decimal?)reader["FH"] : null,
                                Traction = reader["Traction"] != DBNull.Value ? (decimal?)reader["Traction"] : null,
                                LongueurD = reader["LongueurD"] != DBNull.Value ? (decimal?)reader["LongueurD"] : null,
                                LongueurD2 = reader["LongueurD2"] != DBNull.Value ? (decimal?)reader["LongueurD2"] : null,


                                DateCreation = reader["DateDebut"] != DBNull.Value ? (DateTime?)reader["DateDebut"] : null,
                                Quantite = reader["Quantite"] != DBNull.Value ? (decimal?)reader["Quantite"] : null,
                                AH1 = reader["AH1"] != DBNull.Value ? (decimal?)reader["AH1"] : null,
                                AH2 = reader["AH2"] != DBNull.Value ? (decimal?)reader["AH2"] : null,
                                AH3 = reader["AH3"] != DBNull.Value ? (decimal?)reader["AH3"] : null,
                                FH1 = reader["FH1"] != DBNull.Value ? (decimal?)reader["FH1"] : null,
                                FH2 = reader["FH2"] != DBNull.Value ? (decimal?)reader["FH2"] : null,
                                FH3 = reader["FH3"] != DBNull.Value ? (decimal?)reader["FH3"] : null,
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

                                AspectCnx = reader["AspectCnx"] as string,
                            });
                        }
                        return enregComplets;
                    }
                }
            }
        }
        
        public static string LastSerieByOpStatus(string NOp)
        {
            using (SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = "select top 1 Nature from enreg e, enregdetail d where OperationNo = @NOp and d.idenrg = e.id order by datedebut desc;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NOp", NOp);
                    using (SqlDataReader reader = cmd.ExecuteReader())
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
