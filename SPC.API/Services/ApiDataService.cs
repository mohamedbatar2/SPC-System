using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SPC.Models;

namespace SPC.Services
{
    /// <summary>
    /// Implémentation de ISpcDataService qui communique avec l'API REST
    /// Utilisé par le WPF pour accéder aux données via HTTP
    /// </summary>
    public class ApiDataService : ISpcDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private string _token;

        public ApiDataService(string baseUrl)
        {
            _baseUrl = baseUrl;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        // ============================================================
        // HELPER METHODS
        // ============================================================

        private void AddAuthHeader()
        {
            if (!string.IsNullOrEmpty(_token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            }
        }

        private async Task<T?> GetAsync<T>(string endpoint)
        {
            AddAuthHeader();
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private async Task<TResult?> PostAsync<TRequest, TResult>(string endpoint, TRequest data)
        {
            AddAuthHeader();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResult>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private async Task PostAsync<TRequest>(string endpoint, TRequest data)
        {
            AddAuthHeader();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
        }

        private async Task PutAsync<TRequest>(string endpoint, TRequest data)
        {
            AddAuthHeader();
            var response = await _httpClient.PutAsJsonAsync(endpoint, data);
            response.EnsureSuccessStatusCode();
        }

        // SÉRIES (SPCEnreg)

        public async Task<ObservableCollection<SPCEnregComplet>> GetAllSeriesAsync(string status)
        {
            try
            {
                var series = await GetAsync<List<SPCEnregComplet>>($"api/series?status={status}");
                return new ObservableCollection<SPCEnregComplet>(series);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des séries: {ex.Message}", ex);
            }
        }

        public async Task<string?> GetLastSerieAsync()
        {
            try
            {
                return await GetAsync<string>("api/series/last");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de la dernière série: {ex.Message}", ex);
            }
        }

        public async Task<string?> GetLastSerieByOpStatusAsync(string operatorId)
        {
            try
            {
                return await GetAsync<string>($"api/series/last/{operatorId}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de la série par opérateur: {ex.Message}", ex);
            }
        }

        public async Task<SPCEnreg> GetSerieAsync(string noSerie)
        {
            try
            {
                return await GetAsync<SPCEnreg>($"api/series/{noSerie}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de la série {noSerie}: {ex.Message}", ex);
            }
        }

        public async Task<int> InsertNewSerieAsync(SPCEnreg serie)
        {
            try
            {
                return await PostAsync<SPCEnreg, int>("api/series", serie);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'insertion de la série: {ex.Message}", ex);
            }
        }

        public async Task<int> GetSerieIdAsync(string noSerie)
        {
            try
            {
                return await GetAsync<int>($"api/series/{noSerie}/id");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de l'ID de la série: {ex.Message}", ex);
            }
        }

        // DÉTAILS (SPCEnregDetail)
        
        public async Task? InsertNewDetailAsync(SPCEnregDetail detail)
        {
            try
            {
                await PostAsync("api/details", detail);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de l'insertion du détail: {ex.Message}", ex);
            }
        }

        public async Task? SaveEnregAsync(SPCEnreg enreg, SPCEnregDetail enregDetail)
        {
            try
            {
                var request = new
                {
                    Serie = enreg,
                    Detail = enregDetail
                };

                await PostAsync("api/series/complete", request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la sauvegarde de l'enregistrement: {ex.Message}", ex);
            }
        }

        // OPÉRATEURS
       

        public async Task<bool> CheckOpExistAsync(string operatorId)
        {
            try
            {
                if (string.IsNullOrEmpty(operatorId))
                    return false;

                return await GetAsync<bool>($"api/operateurs/{operatorId}/exists");
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> GetOpNameAsync(string operatorId)
        {
            try
            {
                if (string.IsNullOrEmpty(operatorId))
                    return string.Empty;

                return await GetAsync<string>($"api/operateurs/{operatorId}/name");
            }
            catch
            {
                return string.Empty;
            }
        }

        // MACHINES
  

        public async Task<string> GetMachineTypeAsync(string machineId)
        {
            try
            {
                if (string.IsNullOrEmpty(machineId))
                    return string.Empty;

                var machine = await GetAsync<Machine>($"api/machines/{machineId}");
                return machine?.TypeSPC ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<string> GetMachineLibelleAsync(string machineId)
        {
            try
            {
                if (string.IsNullOrEmpty(machineId))
                    return string.Empty;

                var machine = await GetAsync<Machine>($"api/machines/{machineId}");
                return machine?.Libelle ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

            
        // OUTILS
      
        public async Task<ObservableCollection<Outil>> GetOutilsAsync(string cndO, string filter)
        {
            try
            {
                var outils = await GetAsync<List<Outil>>($"api/outils/search?noOutil={cndO}&connexion={filter}");
                return new ObservableCollection<Outil>(outils);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des outils: {ex.Message}", ex);
            }
        }

        public async Task<List<Outil>?> GetAllOutilsAsync()
        {
            try
            {
                return await GetAsync<List<Outil>>("api/outils");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de tous les outils: {ex.Message}", ex);
            }
        }

        public async Task<ObservableCollection<Outil>?> GetOutilsByConditionAsync(string noOutil, string connexion)
        {
            try
            {
                var outils = await GetAsync<List<Outil>>($"api/outils/search?noOutil={noOutil}&connexion={connexion}");
                return new ObservableCollection<Outil>(outils);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la recherche des outils: {ex.Message}", ex);
            }
        }

       
        // PRÉVENTIF (OtaPrvntf)
      

        public async Task<OtaPrvntf?> GetPrvntfAsync(string noOutil)
        {
            try
            {
                return await GetAsync<OtaPrvntf>($"api/preventif/{noOutil}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération du préventif: {ex.Message}", ex);
            }
        }

        public async Task UpdatePrvntfAsync(string noOutil, int quantite)
        {
            try
            {
                var request = new
                {
                    NoOutil = noOutil,
                    Quantite = quantite
                };

                await PutAsync("api/preventif", request);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la mise à jour du préventif: {ex.Message}", ex);
            }
        }

        // Alias pour compatibilité
        public Task<OtaPrvntf> GetPreventiveMaintenanceAsync(string noOutil)
            => GetPrvntfAsync(noOutil);

        public Task UpdatePreventiveMaintenanceAsync(string noOutil, int quantite)
            => UpdatePrvntfAsync(noOutil, quantite);

        
        // CLIENTS
       
        public async Task<List<string>?> GetClientNamesAsync()
        {
            try
            {
                return await GetAsync<List<string>>("api/clients");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération des clients: {ex.Message}", ex);
            }
        }

        // HISTORIQUE

        public async Task<List<SPCEnregComplet>?> GetHistoricalSeriesAsync(string status, string month, string year)
        {
            try
            {
                return await GetAsync<List<SPCEnregComplet>>($"api/series/history?status={status}&month={month}&year={year}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de l'historique: {ex.Message}", ex);
            }
        }

      

        
        /// Définit le token JWT pour l'authentification
      
        public void SetAuthToken(string token)
        {
            _token = token;
        }

        
        /// Authentifie l'utilisateur et récupère un token
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var response = await PostAsync<object, dynamic>("api/auth/login", new
                {
                    Username = username,
                    Password = password
                });

                _token = response.token;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
