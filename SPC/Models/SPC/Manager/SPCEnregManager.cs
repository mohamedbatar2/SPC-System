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
    public class SPCEnregManager
    {
        public static async Task<ObservableCollection<SPCEnreg>> GetEnregsAsync()
        {
            var Enregistrements = new ObservableCollection<SPCEnreg>();
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select * from Enreg;";
                OleDbCommand comm = new OleDbCommand(query, conn);

                using (OleDbDataReader reader =(OleDbDataReader)await comm.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {

                        Enregistrements.Add(new SPCEnreg()
                        {
                            Id = (int)reader["ID"],

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
                            NoMachine = reader["NoMachine"] as string,

                            HA = reader["HA"] !=DBNull.Value? (decimal?)reader["HA"] : null,
                            HI = reader["HI"]!= DBNull.Value? (decimal?)reader["HI"] :null,
                            Traction = reader["Traction"]!= DBNull.Value? (decimal?)reader["Traction"] :null,
                            LongueurD = reader["LongueurD"]!= DBNull.Value? (decimal?)reader["LongueurD"] :null,
                            LongueurD2 = reader["LongueurD2"]!= DBNull.Value? (decimal?)reader["LongueurD2"] :null,

                            NoOutilB = reader["NoOutilB"] as string,
                            NoOutilC = reader["NoOutilC"] as string,

                            Connexion = reader["Connexion"] as string,
                            ConnexionB = reader["ConnexionB"] as string,
                            ConnexionC = reader["ConnexionC"] as string,

                            Denudage = reader["Denudage"]!= DBNull.Value? (decimal?)reader["Denudage"] :null,
                            DenudageB = reader["DenudageB"]!= DBNull.Value? (decimal?)reader["DenudageB"] :null,
                            DenudageC = reader["DenudageC"]!= DBNull.Value? (decimal?)reader["DenudageC"] :null,
                        });
                    }

                }
            }
            return Enregistrements;
        }

        public static async Task<string> GetLastSerieAsync()
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = @"
                                SELECT TOP 1 NoSerie 
                                FROM enreg 
                                ORDER BY CLng(Mid(NoSerie, 3, Len(NoSerie))) DESC;
                                ";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    using (OleDbDataReader reader =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return reader.GetString(0);
                        }
                        return "SN00";
                    }
                }
            }
        }
        public static async Task<SPCEnreg> GetLastSerieAsync(string NMachine, string NMatricule)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select top 1 * from enreg where NoMachine = @NMachine and OperationNo = @NOpr order by id desc;";
                using(OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NMachine", NMachine);
                    cmd.Parameters.AddWithValue("@NOpr", NMatricule);
                    using(OleDbDataReader reader =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                         
                        var enreg = new SPCEnreg()
                        {
                            Id = (int)reader["ID"],

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
                            NoMachine = reader["NoMachine"] as string,

                            HA = reader["HA"] != DBNull.Value ? (decimal?)reader["HA"] : null,
                            HI = reader["HI"] != DBNull.Value ? (decimal?)reader["HI"] : null,
                            Traction = reader["Traction"] != DBNull.Value ? (decimal?)reader["Traction"] : null,
                            LongueurD = reader["LongueurD"] != DBNull.Value ? (decimal?)reader["LongueurD"] : null,
                            LongueurD2 = reader["LongueurD2"] != DBNull.Value ? (decimal?)reader["LongueurD2"] : null,

                            Connexion = reader["Connexion"] as string,
                            Ref = reader["Ref"] as string,

                            Denudage = reader["Denudage"] != DBNull.Value ? (decimal?)reader["Denudage"] : null,

                            NoOutilB = reader["NoOutilB"] as string,
                            NoOutilC = reader["NoOutilC"] as string,

                            ConnexionB = reader["ConnexionB"] as string,
                            ConnexionC = reader["ConnexionC"] as string,

                            DenudageB = reader["DenudageB"]!= DBNull.Value? (decimal?)reader["DenudageB"] :null,
                            DenudageC = reader["DenudageC"]!= DBNull.Value? (decimal?)reader["DenudageC"] :null,
                        };
                        return enreg;
                    }
                }
            }
        }
        public static async Task<SPCEnreg> GetSerieAsync(string NSerie)
        {
            using(OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select * from enreg where NoSerie = @NoSerie;";
                using(OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Noserie", NSerie);
                    using (OleDbDataReader reader =(OleDbDataReader)await  cmd.ExecuteReaderAsync())
                    {
                        await reader.ReadAsync();
                         
                        var enreg = new SPCEnreg()
                        {
                            Id = (int)reader["ID"],

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
                            NoMachine = reader["NoMachine"] as string,

                            HA = reader["HA"] != DBNull.Value ? (decimal?)reader["HA"] : null,
                            HI = reader["HI"] != DBNull.Value ? (decimal?)reader["HI"] : null,
                            Traction = reader["Traction"] != DBNull.Value ? (decimal?)reader["Traction"] : null,
                            LongueurD = reader["LongueurD"] != DBNull.Value ? (decimal?)reader["LongueurD"] : null,
                            LongueurD2 = reader["LongueurD2"] != DBNull.Value ? (decimal?)reader["LongueurD2"] : null,

                            Connexion = reader["Connexion"] as string,
                            Ref = reader["Ref"] as string,

                            Denudage = reader["Denudage"] != DBNull.Value ? (decimal?)reader["Denudage"] : null,

                            NoOutilB = reader["NoOutilB"] as string,
                            NoOutilC = reader["NoOutilC"] as string,

                            ConnexionB = reader["ConnexionB"] as string,
                            ConnexionC = reader["ConnexionC"] as string,

                            DenudageB = reader["DenudageB"]!= DBNull.Value? (decimal?)reader["DenudageB"] :null,
                            DenudageC = reader["DenudageC"]!= DBNull.Value? (decimal?)reader["DenudageC"] :null,
                        };
                        return enreg;
                    }
                }
            }
        }
        public static async Task InsertNewAsync(SPCEnreg enreg)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = @"
                    INSERT INTO enreg (
                        NoSerie, ResourceNo, OperationNo, Client, NoEquipement, [Section],
                        NoContact, NoOutil, NoContact2, NoOutil2, HA, HI, Traction,
                        LongueurD, UAP, LongueurD2, NoMachine, Connexion, Ref, Denudage,
                        ConnexionB, NoOutilB, DenudageB,
                        ConnexionC, NoOutilC, DenudageC
                    )
                    VALUES (
                    ?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?
                    )";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@NoSerie", string.IsNullOrEmpty(enreg.NoSerie) ? DBNull.Value : (object)enreg.NoSerie);
                    cmd.Parameters.AddWithValue("@ResourceNo", string.IsNullOrEmpty(enreg.RessourceNo) ? DBNull.Value : (object)enreg.RessourceNo);
                    cmd.Parameters.AddWithValue("@OperationNo", string.IsNullOrEmpty(enreg.OperationNo) ? DBNull.Value : (object)enreg.OperationNo);
                    cmd.Parameters.AddWithValue("@Client", string.IsNullOrEmpty(enreg.Client) ? DBNull.Value : (object)enreg.Client);
                    cmd.Parameters.AddWithValue("@NoEquipement", string.IsNullOrEmpty(enreg.NoEquipement) ? DBNull.Value : (object)enreg.NoEquipement);
                    cmd.Parameters.AddWithValue("@Section", string.IsNullOrEmpty(enreg.Section) ? DBNull.Value : (object)enreg.Section);
                    cmd.Parameters.AddWithValue("@NoContact", string.IsNullOrEmpty(enreg.NoContact) ? DBNull.Value : (object)enreg.NoContact);
                    cmd.Parameters.AddWithValue("@NoOutil", string.IsNullOrEmpty(enreg.NoOutil) ? DBNull.Value : (object)enreg.NoOutil);
                    cmd.Parameters.AddWithValue("@NoContact2", string.IsNullOrEmpty(enreg.NoContact2) ? DBNull.Value : (object)enreg.NoContact2);
                    cmd.Parameters.AddWithValue("@NoOutil2", string.IsNullOrEmpty(enreg.NoOutil2) ? DBNull.Value : (object)enreg.NoOutil2);

                    cmd.Parameters.AddWithValue("@HA", enreg.HA ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@HI", enreg.HI ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Traction", enreg.Traction ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LongueurD", enreg.LongueurD ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UAP", string.IsNullOrEmpty(enreg.UAP) ? DBNull.Value : (object)enreg.UAP);
                    cmd.Parameters.AddWithValue("@LongueurD2", enreg.LongueurD2 ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@NoMachine", (object)enreg.NoMachine ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Connexion", (object)enreg.Connexion ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Ref", (object)enreg.Ref ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@Denudage", enreg.Denudage ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@ConnexionB", (object)enreg.ConnexionB ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoOutilB", string.IsNullOrEmpty(enreg.NoOutilB) ? DBNull.Value : (object)enreg.NoOutilB);
                    cmd.Parameters.AddWithValue("@DenudageB", enreg.DenudageB ?? (object)DBNull.Value);

                    cmd.Parameters.AddWithValue("@ConnexionC", (object)enreg.ConnexionC ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@NoOutilC", string.IsNullOrEmpty(enreg.NoOutilC) ? DBNull.Value : (object)enreg.NoOutilC);
                    cmd.Parameters.AddWithValue("@DenudageC", enreg.DenudageC ?? (object)DBNull.Value);

                   await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<int> GetIdAsync(string NSerie)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select Id from enreg where NoSerie = @NSerie;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NSerie", NSerie);
                    using (OleDbDataReader reader =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        reader.Read();
                        return (int)reader[0];
                    }
                }
            }
        }
        public static async Task<bool> IsSerieThereAsync(string NSerie)
        {
            using (OleDbConnection conn = DBConnexion.GetConnexion())
            {
                await conn.OpenAsync();
                string query = "select NoSerie from enreg where NoSerie = ?;";
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NSerie", NSerie);
                    using (OleDbDataReader reader =(OleDbDataReader)await cmd.ExecuteReaderAsync())
                    {
                        return await reader.ReadAsync();
                    }
                }
            }
        }
        // Add these at the end of the SPCEnregManager class, before the closing brace

        // ============================================================
        // Backward Compatibility Wrappers (Synchronous versions)
        // ============================================================

        public static ObservableCollection<SPCEnreg> GetEnregs()
            => GetEnregsAsync().GetAwaiter().GetResult();

        public static string GetLastSerie()
            => GetLastSerieAsync().GetAwaiter().GetResult();

        public static SPCEnreg GetLastSerie(string NMachine, string NMatricule)
            => GetLastSerieAsync(NMachine, NMatricule).GetAwaiter().GetResult();

        public static SPCEnreg GetSerie(string NSerie)
            => GetSerieAsync(NSerie).GetAwaiter().GetResult();

        public static void InsertNew(SPCEnreg enreg)
            => InsertNewAsync(enreg).GetAwaiter().GetResult();

        public static int GetId(string NSerie)
            => GetIdAsync(NSerie).GetAwaiter().GetResult();

        public static bool IsSerieThere(string NSerie)
            => IsSerieThereAsync(NSerie).GetAwaiter().GetResult();

    }
}
