using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPC.DB;

namespace SPC.Models
{
    public class EnregDetailManager
    {
        public static ObservableCollection<EnregDetail> GetEnregDetails()
        {
            var enregDetails = new ObservableCollection<EnregDetail>();
            using (SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string querey = "select * from enregDetail";

                SqlCommand cmd = new SqlCommand(querey, conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        enregDetails.Add(new EnregDetail
                        {
                            Id = (int)reader["Id"],
                            IdEnrg = (int)reader["Idenrg"],
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

                            AspectCnx = reader["AspectCnx"] as string,
                        });
                    }
                }

            }
            return enregDetails;
        }

        public static void InsertNew(EnregDetail detail)
        {
            using (SqlConnection conn = DBConnexion.GetConnexion())
            {
                conn.Open();
                string query = @"
                    INSERT INTO enregdetail (
                        IdEnrg, DateDebut, Quantite, AH1, AH2, AH3, 
                        FH1, FH2, FH3, Traction1, Traction2, Traction3,
                        Nature, LongueurM, Repere, Claquage, Marquage,
                        ContactAspect1, ContactAspect2, ContactAspect3,
                        Denudage1, Denudage2, Denudage3,
                        Tol1, Tol2, Tol3,
                        Clip1, Clip2, Clip3, LongueurM2, AspectCnx
                    )
                    VALUES (
                        @IdEnrg, @DateDebut, @Quantite, @AH1, @AH2, @AH3, 
                        @FH1, @FH2, @FH3, @Traction1, @Traction2, @Traction3,
                        @Nature, @LongueurM, @Repere, @Claquage, @Marquage,
                        @ContactAspect1, @ContactAspect2, @ContactAspect3,
                        @Denudage1, @Denudage2, @Denudage3,
                        @Tol1, @Tol2, @Tol3,
                        @Clip1, @Clip2, @Clip3, @LongueurM2, @AspectCnx
                    )";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@IdEnrg", detail.IdEnrg);
                    cmd.Parameters.AddWithValue("@DateDebut", detail.DateCreation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Quantite", detail.Quantite ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AH1", detail.AH1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AH2", detail.AH2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AH3", detail.AH3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FH1", detail.FH1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FH2", detail.FH2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FH3", detail.FH3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction1", detail.Traction1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction2", detail.Traction2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction3", detail.Traction3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LongueurM", detail.LongueurM ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Denudage1", detail.Denudage1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Denudage2", detail.Denudage2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Denudage3", detail.Denudage3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tol1", detail.Tol1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tol2", detail.Tol2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tol3", detail.Tol3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LongueurM2", detail.LongueurM2 ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@Repere", string.IsNullOrEmpty(detail.Repere) ? DBNull.Value : (object)detail.Repere);
                    cmd.Parameters.AddWithValue("@Claquage", string.IsNullOrEmpty(detail.Claquage) ? DBNull.Value : (object)detail.Claquage);
                    cmd.Parameters.AddWithValue("@Marquage", string.IsNullOrEmpty(detail.Marquage) ? DBNull.Value : (object)detail.Marquage);
                    cmd.Parameters.AddWithValue("@ContactAspect1", string.IsNullOrEmpty(detail.ContactAspect1) ? DBNull.Value : (object)detail.ContactAspect1);
                    cmd.Parameters.AddWithValue("@ContactAspect2", string.IsNullOrEmpty(detail.ContactAspect2) ? DBNull.Value : (object)detail.ContactAspect2);
                    cmd.Parameters.AddWithValue("@ContactAspect3", string.IsNullOrEmpty(detail.ContactAspect3) ? DBNull.Value : (object)detail.ContactAspect3);
                    cmd.Parameters.AddWithValue("@Nature", string.IsNullOrEmpty(detail.Nature) ? DBNull.Value : (object)detail.Nature);

                    cmd.Parameters.AddWithValue("@Clip1", string.IsNullOrEmpty(detail.Clip1) ? DBNull.Value : (object)detail.Clip1);
                    cmd.Parameters.AddWithValue("@Clip2", string.IsNullOrEmpty(detail.Clip2) ? DBNull.Value : (object)detail.Clip2);
                    cmd.Parameters.AddWithValue("@Clip3", string.IsNullOrEmpty(detail.Clip3) ? DBNull.Value : (object)detail.Clip3);

                    cmd.Parameters.AddWithValue("@AspectCnx", string.IsNullOrEmpty(detail.AspectCnx) ? DBNull.Value : (object)detail.AspectCnx);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
