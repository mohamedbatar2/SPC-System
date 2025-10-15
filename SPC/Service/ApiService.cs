using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SPC.Services
{
    public class ApiService : IDisposable
    {
        private readonly HttpClient _httpClient;
        private string _jwtToken;
        private bool _disposed = false;

        public ApiService(string baseUrl)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };

            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Sets the JWT Bearer token for all subsequent API calls
        /// </summary>
        public void SetAuthToken(string token)
        {
            _jwtToken = token;
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Clears the authentication token
        /// </summary>
        public void ClearAuthToken()
        {
            _jwtToken = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        /// <summary>
        /// Generic GET request
        /// </summary>
        /// <typeparam name="T">Expected response type</typeparam>
        /// <param name="endpoint">API endpoint (e.g., "api/Clients")</param>
        public async Task<T> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<T>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling GET {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generic GET request for a single item by ID
        /// </summary>
        public async Task<T> GetByIdAsync<T>(string endpoint, int id)
        {
            return await GetAsync<T>($"{endpoint}/{id}");
        }

        /// <summary>
        /// Generic POST request
        /// </summary>
        /// <typeparam name="TRequest">Request body type</typeparam>
        /// <typeparam name="TResponse">Response type</typeparam>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string endpoint, TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling POST {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// POST without expecting a response body
        /// </summary>
        public async Task PostAsync<TRequest>(string endpoint, TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(endpoint, data);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling POST {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generic PUT request
        /// </summary>
        public async Task PutAsync<T>(string endpoint, T data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(endpoint, data);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling PUT {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generic DELETE request
        /// </summary>
        public async Task DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error calling DELETE {endpoint}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// DELETE by ID
        /// </summary>
        public async Task DeleteAsync(string endpoint, int id)
        {
            await DeleteAsync($"{endpoint}/{id}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _httpClient?.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
