using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPC.DB;

namespace SPC.Models
{
    public class SPCEnregDetailManager
    {
        public static async Task< ObservableCollection<SPCEnregDetail>> GetEnregDetailsAsync()
        {
            var enregDetails = new ObservableCollection<SPCEnregDetail>();
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string querey = "select * from enregDetail";

                OleDbCommand cmd = new OleDbCommand(querey, conn);
                using (OleDbDataReader reader =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        enregDetails.Add(new SPCEnregDetail
                        {
                            Id = (int)reader["Id"],
                            IdEnrg = (int)reader["Idenrg"],
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

                            AspectCnx = reader["AspectCnx"] as string,

                            AspectCnxB = reader["AspectCnxB"] as string,
                            AspectCnxC = reader["AspectCnxC"] as string,

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
                        });
                    }
                }

            }
            return enregDetails;
        }

        public static async Task InsertNewAsync(SPCEnregDetail detail)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = @"
                    INSERT INTO enregdetail (
                        IdEnrg, DateDebut, Quantite, HA1, HA2, HA3, 
                        HI1, HI2, HI3, Traction1, Traction2, Traction3,
                        Nature, LongueurM, Repere, Claquage, Marquage,
                        ContactAspect1, ContactAspect2, ContactAspect3,
                        Denudage1, Denudage2, Denudage3,
                        Tol1, Tol2, Tol3,
                        Clip1, Clip2, Clip3, LongueurM2, AspectCnx,
                        HA1B, HA2B, HA3B, HI1B, HI2B, HI3B, Traction1B, Traction2B, Traction3B, 
                        HA1C, HA2C, HA3C, HI1C, HI2C, HI3C, Traction1C, Traction2C, Traction3C,
                        AspectCnxB, AspectCnxC
                    )
                    VALUES (
                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 
                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?,
                        ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?
                    )";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@IdEnrg", detail.IdEnrg);
                    cmd.Parameters.AddWithValue("@DateCreation", detail.DateCreation.HasValue ? detail.DateCreation.Value.ToString("yyyy-MM-dd HH:mm:ss") : (object)DBNull.Value); 
                    cmd.Parameters.AddWithValue("@Quantite", detail.Quantite ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HA1", detail.HA1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HA2", detail.HA2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HA3", detail.HA3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI1", detail.HI1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI2", detail.HI2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI3", detail.HI3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction1", detail.Traction1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction2", detail.Traction2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction3", detail.Traction3 ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@Nature", string.IsNullOrEmpty(detail.Nature) ? DBNull.Value : (object)detail.Nature);

                    cmd.Parameters.AddWithValue("@LongueurM", detail.LongueurM ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Repere", string.IsNullOrEmpty(detail.Repere) ? DBNull.Value : (object)detail.Repere);
                    cmd.Parameters.AddWithValue("@Claquage", string.IsNullOrEmpty(detail.Claquage) ? DBNull.Value : (object)detail.Claquage);
                    cmd.Parameters.AddWithValue("@Marquage", string.IsNullOrEmpty(detail.Marquage) ? DBNull.Value : (object)detail.Marquage);

                    cmd.Parameters.AddWithValue("@ContactAspect1", string.IsNullOrEmpty(detail.ContactAspect1) ? DBNull.Value : (object)detail.ContactAspect1);
                    cmd.Parameters.AddWithValue("@ContactAspect2", string.IsNullOrEmpty(detail.ContactAspect2) ? DBNull.Value : (object)detail.ContactAspect2);
                    cmd.Parameters.AddWithValue("@ContactAspect3", string.IsNullOrEmpty(detail.ContactAspect3) ? DBNull.Value : (object)detail.ContactAspect3);
                    cmd.Parameters.AddWithValue("@Denudage1", detail.Denudage1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Denudage2", detail.Denudage2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Denudage3", detail.Denudage3 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tol1", detail.Tol1 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tol2", detail.Tol2 ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Tol3", detail.Tol3 ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@Clip1", string.IsNullOrEmpty(detail.Clip1) ? DBNull.Value : (object)detail.Clip1);
                    cmd.Parameters.AddWithValue("@Clip2", string.IsNullOrEmpty(detail.Clip2) ? DBNull.Value : (object)detail.Clip2);
                    cmd.Parameters.AddWithValue("@Clip3", string.IsNullOrEmpty(detail.Clip3) ? DBNull.Value : (object)detail.Clip3);
                    cmd.Parameters.AddWithValue("@LongueurM2", detail.LongueurM2 ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@AspectCnx", string.IsNullOrEmpty(detail.AspectCnx) ? DBNull.Value : (object)detail.AspectCnx);

                    cmd.Parameters.AddWithValue("@HA1B", detail.HA1B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HA2B", detail.HA2B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HA3B", detail.HA3B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI1B", detail.HI1B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI2B", detail.HI2B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI3B", detail.HI3B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction1B", detail.Traction1B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction2B", detail.Traction2B ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction3B", detail.Traction3B ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@HA1C", detail.HA1C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HA2C", detail.HA2C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HA3C", detail.HA3C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI1C", detail.HI1C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI2C", detail.HI2C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI3C", detail.HI3C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction1C", detail.Traction1C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction2C", detail.Traction2C ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction3C", detail.Traction3C ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@AspectCnxB", string.IsNullOrEmpty(detail.AspectCnxB) ? DBNull.Value : (object)detail.AspectCnxB);
                    cmd.Parameters.AddWithValue("@AspectCnxC", string.IsNullOrEmpty(detail.AspectCnxC) ? DBNull.Value : (object)detail.AspectCnxC);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public static ObservableCollection<SPCEnregDetail> GetEnregDetails()
            => GetEnregDetailsAsync().GetAwaiter().GetResult();

        public static void InsertNew(SPCEnregDetail detail)
            => InsertNewAsync(detail).GetAwaiter().GetResult();

    }
}
