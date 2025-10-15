using System;
using System.Data.OleDb;
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace SPC.DB
{
    [SupportedOSPlatform("windows")]

    public class DBConnexion
    {
        private static readonly string user = Environment.UserName;
        private static readonly string databasePath = $@"C:\Users\{user}\Desktop\SPCapplication.accdb";
        private static readonly string connexionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={databasePath};Persist Security Info=False;";

        /// <summary>
        /// Creates a new OleDbConnection instance.
        /// Connection must be opened by the caller.
        /// </summary>
        public static OleDbConnection GetConnexion()
        {
            if (!File.Exists(databasePath))
            {
                throw new FileNotFoundException($"Database file not found at: {databasePath}");
            }

            return new OleDbConnection(connexionString);
        }

        /// <summary>
        /// Gets the connection string for the Access database.
        /// </summary>
        public static string GetConnectionString() => connexionString;

        /// <summary>
        /// Gets the database file path.
        /// </summary>
        public static string GetDatabasePath() => databasePath;

        /// <summary>
        /// Checks if the database file exists.
        /// </summary>
        public static bool DatabaseExists() => File.Exists(databasePath);

        /// <summary>
        /// Validates that the database connection can be established.
        /// </summary>
        public static async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var conn = GetConnexion();
                await conn.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
