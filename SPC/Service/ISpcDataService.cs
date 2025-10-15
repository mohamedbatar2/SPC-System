using SPC.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SPC.Services
{
    public interface ISpcDataService
    {
        // ============================================================
        // SÉRIES
        // ============================================================
        Task<ObservableCollection<SPCEnregComplet>> GetAllSeriesAsync(string status);
        Task<string> GetLastSerieAsync();
        Task<string> GetLastSerieByOpStatusAsync(string operatorId);
        Task<SPCEnreg> GetSerieAsync(string noSerie);
        Task<int> InsertNewSerieAsync(SPCEnreg serie);
        Task<int> GetSerieIdAsync(string noSerie);

        // ============================================================
        // DÉTAILS
        // ============================================================
        Task InsertNewDetailAsync(SPCEnregDetail detail);
        Task SaveEnregAsync(SPCEnreg enreg, SPCEnregDetail enregDetail);

        // ============================================================
        // OPÉRATEURS
        // ============================================================
        Task<bool> CheckOpExistAsync(string operatorId);
        Task<string> GetOpNameAsync(string operatorId);

        // ============================================================
        // MACHINES
        // ============================================================
        Task<string> GetMachineTypeAsync(string machineId);
        Task<string> GetMachineLibelleAsync(string machineId);

        // ============================================================
        // OUTILS
        // ============================================================
        Task<ObservableCollection<Outil>> GetOutilsAsync(string cndO, string filter);
        Task<List<Outil>> GetAllOutilsAsync();
        Task<ObservableCollection<Outil>> GetOutilsByConditionAsync(string noOutil, string connexion);

        // ============================================================
        // PRÉVENTIF
        // ============================================================
        Task<OtaPrvntf> GetPrvntfAsync(string noOutil);
        Task UpdatePrvntfAsync(string noOutil, int quantite);

        // Alias optionnels (pour compatibilité)
        Task<OtaPrvntf> GetPreventiveMaintenanceAsync(string noOutil);
        Task UpdatePreventiveMaintenanceAsync(string noOutil, int quantite);
        Task ResetPrvntfAsync(string nOutil);
        // ============================================================
        // CLIENTS
        // ============================================================
        Task<List<string>> GetClientNamesAsync();

        // ============================================================
        // HISTORIQUE
        // ============================================================
        Task<List<SPCEnregComplet>> GetHistoricalSeriesAsync(string status, string month, string year);
    }
}
