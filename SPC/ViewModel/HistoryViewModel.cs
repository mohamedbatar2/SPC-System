using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SPC.Models;
using SPC.Services;

namespace SPC.ViewModel
{
    public class HistoryViewModel : INotifyPropertyChanged
    {
        // ============================================================
        // SERVICES & DEPENDENCIES
        // ============================================================

        private readonly ISpcDataService _dataService;

        // ============================================================
        // PROPERTIES
        // ============================================================

        private ObservableCollection<SPCEnregComplet> _allSeries;
        public ObservableCollection<SPCEnregComplet> AllSeries
        {
            get => _allSeries;
            set
            {
                _allSeries = value;
                OnPropertyChanged(nameof(AllSeries));
            }
        }

        private List<string> _dateItems;
        public List<string> DateItems
        {
            get => _dateItems;
            set
            {
                _dateItems = value;
                OnPropertyChanged(nameof(DateItems));
            }
        }

        private string _selectedDate;
        public string SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));

                if (!string.IsNullOrEmpty(value))
                {
                    var sp = value.Split('/');
                    _month = sp[0].TrimStart('0');
                    _year = sp[1];
                    _ = ReloadDataAsync(); // Fire and forget
                }
            }
        }

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

        private string _year;
        private string _month;

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public HistoryViewModel(ISpcDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialiser les collections
            AllSeries = new ObservableCollection<SPCEnregComplet>();
            DateItems = new List<string>();

            // Date actuelle
            _year = DateTime.Now.Year.ToString();
            _month = DateTime.Now.Month.ToString();

            // Générer la liste des dates
            GenerateDateItems();

            // Charger les données async
            _ = LoadInitialDataAsync();
        }

        // ============================================================
        // INITIALIZATION
        // ============================================================

        private void GenerateDateItems()
        {
            var items = new List<string>();
            int currentYear = int.Parse(_year);
            int currentMonth = int.Parse(_month);

            for (int y = currentYear; y > 2015; y--)
            {
                if (y == currentYear)
                {
                    // Année en cours: du mois actuel jusqu'à janvier
                    for (int m = currentMonth; m > 0; m--)
                    {
                        items.Add($"{m:D2}/{y}");
                    }
                }
                else
                {
                    // Années précédentes: tous les mois
                    for (int m = 12; m > 0; m--)
                    {
                        items.Add($"{m:D2}/{y}");
                    }
                }
            }

            DateItems = items;
        }

        private async Task LoadInitialDataAsync()
        {
            IsLoading = true;

            try
            {
                var series = await _dataService.GetHistoricalSeriesAsync("F", _month, _year);
                AllSeries = new ObservableCollection<SPCEnregComplet>(series);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur de chargement de l'historique:\n\n{ex.Message}",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ============================================================
        // DATA RELOAD (ASYNC)
        // ============================================================

        private async Task ReloadDataAsync()
        {
            IsLoading = true;

            try
            {
                AllSeries.Clear();

                var series = await _dataService.GetHistoricalSeriesAsync("F", _month, _year);

                foreach (var item in series)
                {
                    AllSeries.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur de rechargement des données:\n\n{ex.Message}",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ============================================================
        // INOTIFYPROPERTYCHANGED
        // ============================================================

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
