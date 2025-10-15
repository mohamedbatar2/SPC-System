using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SPC.Models;
using SPC.Services;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class ResetOutilViewModel : INotifyPropertyChanged
    {
        // ============================================================
        // SERVICES & DEPENDENCIES
        // ============================================================

        private readonly ISpcDataService _dataService;

        // ============================================================
        // PROPERTIES
        // ============================================================

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string _nOutil;
        public string NOutil
        {
            get => _nOutil;
            set
            {
                _nOutil = value;
                OnPropertyChanged(nameof(NOutil));
            }
        }

        // ============================================================
        // COMMANDS
        // ============================================================

        public ICommand ResetCommand { get; set; }

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public ResetOutilViewModel()
        {
            // Use dependency injection in production
            _dataService = new AccessDataService();

            ResetCommand = new AsyncRelayCommand(ResetAsync, CanReset);
        }

        // Constructor with dependency injection (recommended)
        public ResetOutilViewModel(ISpcDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            ResetCommand = new AsyncRelayCommand(ResetAsync, CanReset);
        }

      

        private async Task ResetAsync()
        {
            if (string.IsNullOrEmpty(NOutil))
            {
                MessageBox.Show("Veuillez entrer un numéro d'outil.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;

            try
            {
                
                if (!App.AuthService.IsAuthenticated)
                {
                    await App.AuthService.LoginAsync("admin", "password123");
                }

               
                var prv = await _dataService.GetPrvntfAsync(NOutil);

                if (prv == null)
                {
                    MessageBox.Show($"Outil '{NOutil}' non trouvé.", "Information",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                   
                    await _dataService.ResetPrvntfAsync(NOutil);

                    MessageBox.Show($"Outil '{NOutil}' réinitialisé avec succès!", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Clear the input
                    NOutil = string.Empty;
                }
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Erreur de connexion API: {httpEx.Message}\n\nAssurez-vous que l'API est en cours d'exécution.",
                    "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la réinitialisation: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);

                System.Diagnostics.Debug.WriteLine($"Reset error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanReset()
        {
            return !IsLoading;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
