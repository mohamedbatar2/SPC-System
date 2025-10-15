using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using SPC.Models;

namespace SPC.Services
{
    [SupportedOSPlatform("windows")]
    

    public class AccessDataService : ISpcDataService
    {
        // ============================================================
        // SÉRIES (SPCEnreg)
        // ============================================================

        public Task<ObservableCollection<SPCEnregComplet>> GetAllSeriesAsync(string status)
            => SPCEnregCompletManager.GetAllAsync(status);

        public Task<string> GetLastSerieAsync()
            => SPCEnregManager.GetLastSerieAsync();

        public Task<string> GetLastSerieByOpStatusAsync(string operatorId)
            => SPCEnregCompletManager.LastSerieByOpStatusAsync(operatorId);

        public Task<SPCEnreg> GetSerieAsync(string noSerie)
            => SPCEnregManager.GetSerieAsync(noSerie);

        public async Task<int> InsertNewSerieAsync(SPCEnreg serie)
        {
            await SPCEnregManager.InsertNewAsync(serie);
            return await GetSerieIdAsync(serie.NoSerie);
        }

        public Task<int> GetSerieIdAsync(string noSerie)
            => SPCEnregManager.GetIdAsync(noSerie);

        // ============================================================
        // DÉTAILS (SPCEnregDetail)
        // ============================================================

        public Task InsertNewDetailAsync(SPCEnregDetail detail)
            => SPCEnregDetailManager.InsertNewAsync(detail);

        // ============================================================
        // ORCHESTRATION (Save complet)
        // ============================================================

        public async Task SaveEnregAsync(SPCEnreg enreg, SPCEnregDetail enregDetail)
        {
            await InsertNewSerieAsync(enreg);
            enregDetail.IdEnrg = await GetSerieIdAsync(enreg.NoSerie);
            await InsertNewDetailAsync(enregDetail);
        }

        // ============================================================
        // OPÉRATEURS
        // ============================================================

        public Task<bool> CheckOpExistAsync(string operatorId)
        {
            if (string.IsNullOrEmpty(operatorId))
                return Task.FromResult(false);
            return OperateurManager.CheckOpExistAsync(operatorId);
        }

        public Task<string> GetOpNameAsync(string operatorId)
        {
            if (string.IsNullOrEmpty(operatorId))
                return Task.FromResult(string.Empty);
            return OperateurManager.GetOpNameAsync(operatorId);
        }

        // ============================================================
        // MACHINES
        // ============================================================

        public Task<string> GetMachineTypeAsync(string machineId)
        {
            if (string.IsNullOrEmpty(machineId))
                return Task.FromResult(string.Empty);
            return MachineManager.GetTypeSPCAsync(machineId);
        }

        public Task<string> GetMachineLibelleAsync(string machineId)
        {
            if (string.IsNullOrEmpty(machineId))
                return Task.FromResult(string.Empty);
            return MachineManager.GetLibelleAsync(machineId);
        }

        // ============================================================
        // OUTILS
        // ============================================================

        public async Task<ObservableCollection<Outil>> GetOutilsAsync(string cndO, string filter)
        {
            var firstTry = await OutilManager.GetOutilsByCndOAsync(cndO, filter);
            if (firstTry.Any())
                return firstTry;

            // Si aucun résultat, inverser les paramètres
            return await OutilManager.GetOutilsByCndOAsync(filter, cndO);
        }

        public async Task<List<Outil>> GetAllOutilsAsync()
        {
            var collection = await OutilManager.GetOutilsAsync();
            return collection.ToList();
        }

        public Task<ObservableCollection<Outil>> GetOutilsByConditionAsync(string noOutil, string connexion)
            => OutilManager.GetOutilsByCndOAsync(noOutil, connexion);

        // ============================================================
        // MAINTENANCE PRÉVENTIVE (OtaPrvntf)
        // ============================================================

        public Task<OtaPrvntf> GetPrvntfAsync(string noOutil)
            => OtaPrvntfManager.GetPrvntfAsync(noOutil);

        public Task UpdatePrvntfAsync(string noOutil, int quantite)
            => OtaPrvntfManager.UpdatePrvntfAsync(noOutil, quantite);

        // ✅ Alias pour compatibilité (si nécessaire)
        public Task<OtaPrvntf> GetPreventiveMaintenanceAsync(string noOutil)
            => GetPrvntfAsync(noOutil);

        public Task UpdatePreventiveMaintenanceAsync(string noOutil, int quantite)
            => UpdatePrvntfAsync(noOutil, quantite);
        public async Task ResetPrvntfAsync(string nOutil)
        {
            await Task.Run(() =>
            {
                try
                {
                    OtaPrvntfManager.ResetOtaPrvntf(nOutil);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error resetting preventif: {ex.Message}", ex);
                }
            });
        }


        // ============================================================
        // CLIENTS
        // ============================================================

        public Task<List<string>> GetClientNamesAsync()
            => ClientManager.GetClientsNamesAsync();

        // ============================================================
        // HISTORIQUE
        // ============================================================

        public async Task<List<SPCEnregComplet>> GetHistoricalSeriesAsync(string status, string month, string year)
        {
            // Récupérer l'ObservableCollection
            var collection = await SPCEnregCompletManager.GetAllAsync(status, month, year);

            // ✅ Convertir explicitement en List avec ToList()
            return collection.ToList();
        }

    }
}
