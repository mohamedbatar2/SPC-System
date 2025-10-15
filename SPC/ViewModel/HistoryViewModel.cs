using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using SPC.DTOs;
using SPC.Services;

namespace SPC.ViewModel
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;

        public ObservableCollection<SPCEnregCompletDto> AllSeries { get; set; }
        public List<string> DateItems { get; set; }

        private string selectedDate;
        public string SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                if (!string.IsNullOrEmpty(value))
                {
                    var sp = value.Split('/');
                    if (sp.Length == 2)
                    {
                        month = sp[0].TrimStart('0');
                        if (string.IsNullOrEmpty(month)) month = "12"; // Handle "00" case
                        year = sp[1];
                        OnPropertyChanged(nameof(SelectedDate));
                        _ = ReloadDataAsync();
                    }
                }
            }
        }

        private string year, month;
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

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryViewModel()
        {
            try
            {
                _apiService = App.ApiService;

                DateItems = new List<string>();
                year = DateTime.Now.Year.ToString();
                month = DateTime.Now.Month.ToString();

                AllSeries = new ObservableCollection<SPCEnregCompletDto>();

                // Build date dropdown (years from current down to 2016)
                for (int y = int.Parse(year); y > 2015; y--)
                {
                    if (y == int.Parse(year))
                    {
                        // Current year: only months up to current month
                        for (int m = int.Parse(month); m > 0; m--)
                        {
                            DateItems.Add($"{m:D2}/{y}");
                        }
                    }
                    else
                    {
                        // Previous years: all 12 months
                        for (int m = 12; m > 0; m--)
                        {
                            DateItems.Add($"{m:D2}/{y}");
                        }
                    }
                }

                // Set default selected date
                if (DateItems.Count > 0)
                {
                    selectedDate = DateItems[0];
                    OnPropertyChanged(nameof(SelectedDate));
                }

                // Load initial data AFTER window is shown (prevents blocking)
                Application.Current?.Dispatcher.BeginInvoke(new Action(async () =>
                {
                    await LoadInitialDataAsync();
                }), System.Windows.Threading.DispatcherPriority.Background);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing History view: {ex.Message}\n\nPlease make sure the API is running.",
                    "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async Task LoadInitialDataAsync()
        {
            await ReloadDataAsync();
        }

        private async Task ReloadDataAsync()
        {
            IsLoading = true;

            try
            {
                // Ensure authenticated
                if (!App.AuthService.IsAuthenticated)
                {
                    var loginSuccess = await App.AuthService.LoginAsync("admin", "password123");
                    if (!loginSuccess)
                    {
                        MessageBox.Show("Authentication failed. Please check that the API is running at https://localhost:7191",
                            "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                // Call the /api/Series/history endpoint
                var endpoint = $"api/Series/history?status=F&month={month}&year={year}";
                var data = await _apiService.GetAsync<List<SPCEnregCompletDto>>(endpoint);

                // Update UI on UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AllSeries.Clear();
                    if (data != null && data.Count > 0)
                    {
                        foreach (var item in data)
                        {
                            AllSeries.Add(item);
                        }
                    }
                    else
                    {
                        // Optional: Show message if no data
                        // MessageBox.Show($"No data found for {month}/{year}");
                    }
                });
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Cannot connect to API. Make sure SPC.API is running at https://localhost:7191\n\nError: {httpEx.Message}",
                    "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading history data: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
