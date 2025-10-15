using System;
using System.Threading.Tasks;

namespace SPC.Services
{
    public class AuthenticationService
    {
        private readonly ApiService _apiService;
        private string _currentToken;
        private DateTime _tokenExpiration;

        public AuthenticationService(ApiService apiService)
        {
            _apiService = apiService;
        }

        public bool IsAuthenticated =>
            !string.IsNullOrEmpty(_currentToken) &&
            DateTime.UtcNow < _tokenExpiration;

        /// <summary>
        /// Authenticates user and retrieves JWT token
        /// </summary>
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                var response = await _apiService.PostAsync<LoginRequest, AuthResponse>(
                    "api/Auth/login", loginRequest);

                _currentToken = response.Token;
                _tokenExpiration = response.Expiration;
                _apiService.SetAuthToken(_currentToken);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Logs out and clears authentication token
        /// </summary>
        public void Logout()
        {
            _currentToken = null;
            _tokenExpiration = DateTime.MinValue;
            _apiService.ClearAuthToken();
        }

        /// <summary>
        /// Gets current authentication token
        /// </summary>
        public string GetCurrentToken() => _currentToken;
    }

    // Supporting classes
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
