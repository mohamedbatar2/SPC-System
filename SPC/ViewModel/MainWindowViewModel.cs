using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SPC.Models;
using SPC.Services;
using SPC.Tools;
using SPC.Views;

namespace SPC.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // ============================================================
        // SERVICES & DEPENDENCIES
        // ============================================================

        private readonly ISpcDataService _dataService;

        // ============================================================
        // COMMANDS
        // ============================================================

        public ICommand HistoryCommand { get; set; }
        public ICommand NewSeriesCommand { get; set; }
        public ICommand SurrFinCommand { get; set; }

        // ============================================================
        // COLLECTIONS
        // ============================================================

        private ObservableCollection<SPCEnregComplet> _filtredSeries;
        public ObservableCollection<SPCEnregComplet> FiltredSeries
        {
            get => _filtredSeries;
            set
            {
                _filtredSeries = value;
                OnPropertyChanger(nameof(FiltredSeries));
            }
        }

        private ObservableCollection<SPCEnregComplet> _allSeries;
        public ObservableCollection<SPCEnregComplet> AllSeries
        {
            get => _allSeries;
            set
            {
                _allSeries = value;
                OnPropertyChanger(nameof(AllSeries));
            }
        }

        private ObservableCollection<Outil> _filtredOutils;
        public ObservableCollection<Outil> FiltredOutils
        {
            get => _filtredOutils;
            set
            {
                _filtredOutils = value;
                OnPropertyChanger(nameof(FiltredOutils));
            }
        }

        // ============================================================
        // PROPERTIES
        // ============================================================

        private SPCEnregComplet _serieSelected;
        public SPCEnregComplet SerieSelected
        {
            get => _serieSelected;
            set
            {
                if (_serieSelected != value)
                {
                    _serieSelected = value;
                    OnPropertyChanger(nameof(SerieSelected));
                }
            }
        }

        private string _uap;
        public string UAP
        {
            get => _uap;
            set
            {
                _uap = value;
                OnPropertyChanger(nameof(UAP));
            }
        }

        private string _nMachine;
        public string NMachine
        {
            get => _nMachine;
            set
            {
                OutilFilter = "";
                InfoBar.RestartTimer();
                if (_nMachine != value)
                {
                    _nMachine = value;
                    _timerDispatcher.Stop();
                    _timerDispatcher.Start();
                    OnPropertyChanger(nameof(NMachine));
                }
            }
        }

        private string _nMatricule;
        public string NMatricule
        {
            get => _nMatricule;
            set
            {
                InfoBar.RestartTimer();
                OutilFilter = "";
                if (_nMatricule != value)
                {
                    _nMatricule = value;
                    _timerDispatcher.Stop();
                    _timerDispatcher.Start();
                    OnPropertyChanger(nameof(NMatricule));
                }
            }
        }

        private string _machineLabel;
        public string MachineLabel
        {
            get => _machineLabel;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _machineLabel = "Machine:";
                }
                else if (value != _machineLabel)
                {
                    _machineLabel = value;
                }
                OnPropertyChanger(nameof(MachineLabel));
            }
        }

        private string _operateurLabel;
        public string OperateurLabel
        {
            get => _operateurLabel;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _operateurLabel = "Matricule:";
                }
                else if (value != _operateurLabel)
                {
                    _operateurLabel = value;
                }
                OnPropertyChanger(nameof(OperateurLabel));
            }
        }

        private Outil _selectedOutil;
        public Outil SelectedOutil
        {
            get => _selectedOutil;
            set
            {
                _selectedOutil = value;
                VisiOutilInfor = (SelectedOutil == null) ? "Hidden" : "Visible";
                OnPropertyChanger(nameof(SelectedOutil));
            }
        }

        private string _visiOutilInfor;
        public string VisiOutilInfor
        {
            get => _visiOutilInfor;
            set
            {
                _visiOutilInfor = value;
                OnPropertyChanger(nameof(VisiOutilInfor));
            }
        }

        private string _outilFilter;
        public string OutilFilter
        {
            get => _outilFilter;
            set
            {
                _outilFilter = value;
                _ = ReloadOutilsAsync(); // Fire and forget
                OnPropertyChanger(nameof(OutilFilter));
            }
        }

        private string _visiOutilGrid;
        public string VisiOutilGrid
        {
            get => _visiOutilGrid;
            set
            {
                _visiOutilGrid = value;
                OnPropertyChanger(nameof(VisiOutilGrid));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanger(nameof(IsLoading));
            }
        }

        public InfoBarHelper InfoBar { get; set; }
        public DispatcherTimer _timerDispatcher { get; set; }
        public DispatcherTimer _reloadTimer { get; set; }

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public MainWindowViewModel(ISpcDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize collections
            AllSeries = new ObservableCollection<SPCEnregComplet>();
            FiltredSeries = new ObservableCollection<SPCEnregComplet>();
            FiltredOutils = new ObservableCollection<Outil>();

            // Initialize commands
            NewSeriesCommand = new AsyncRelayCommand(async () => await OpenNewSeriesAsync("new"));
            SurrFinCommand = new AsyncRelayCommand(async () => await OpenNewSeriesAsync("surr"));
            HistoryCommand = new RelayCommand(OpenHistory, parm => true);

            // Setup timers
            SetupTimers();

            // Initialize other properties
            InfoBar = new InfoBarHelper();
            VisiOutilGrid = "hidden";
            VisiOutilInfor = "Hidden";

            // Show reset outil view
            var resetOutilView = new ResetOutil();
            resetOutilView.Show();

            // Load initial data async
            _ = InitializeAsync();
        }

        // ============================================================
        // INITIALIZATION
        // ============================================================

        private void SetupTimers()
        {
            // Timer pour le filtrage différé
            _timerDispatcher = new DispatcherTimer();
            _timerDispatcher.Interval = TimeSpan.FromMilliseconds(1000);
            _timerDispatcher.Tick += async (s, e) =>
            {
                _timerDispatcher.Stop();
                await ReloadFilterAsync();
            };

            // Timer pour le rechargement automatique
            _reloadTimer = new DispatcherTimer();
            _reloadTimer.Interval = TimeSpan.FromMinutes(1);
            _reloadTimer.Tick += async (s, e) =>
            {
                await ReloadAllSeriesAsync();
            };
            _reloadTimer.Start();
        }

        private async Task InitializeAsync()
        {
            IsLoading = true;
            try
            {
                await ReloadAllSeriesAsync();
                await ReloadFilterAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur d'initialisation: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ============================================================
        // ASYNC METHODS - DATA LOADING
        // ============================================================

        public async Task ReloadAllSeriesAsync()
        {
            try
            {
                var series = await _dataService.GetAllSeriesAsync("NF");
                AllSeries = series;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de rechargement des séries: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public async Task ReloadFilterAsync()
        {
            IsLoading = true;
            try
            {
                // Filtrer les séries
                var filtered = new ObservableCollection<SPCEnregComplet>();

                if (string.IsNullOrEmpty(NMatricule))
                {
                    foreach (var item in AllSeries)
                    {
                        filtered.Add(item);
                    }
                }
                else
                {
                    foreach (var item in AllSeries)
                    {
                        if (item.OperationNo.Equals(NMatricule))
                        {
                            if (string.IsNullOrEmpty(NMachine))
                            {
                                filtered.Add(item);
                            }
                            else if (item.NoMachine.Contains(NMachine.ToUpper()))
                            {
                                filtered.Add(item);
                            }
                        }
                    }
                }

                FiltredSeries = filtered;

                // Charger les labels async
                await LoadLabelsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de filtrage: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadLabelsAsync()
        {
            try
            {
                // Charger le nom de l'opérateur
                if (!string.IsNullOrEmpty(NMatricule))
                {
                    bool exists = await _dataService.CheckOpExistAsync(NMatricule);
                    if (exists)
                    {
                        var opName = await _dataService.GetOpNameAsync(NMatricule.ToUpper());
                        OperateurLabel = opName;
                    }
                    else
                    {
                        OperateurLabel = "";
                    }
                }
                else
                {
                    OperateurLabel = "";
                }

                // Charger le libellé de la machine
                if (!string.IsNullOrEmpty(NMachine))
                {
                    var machineLib = await _dataService.GetMachineLibelleAsync(NMachine);
                    MachineLabel = machineLib;
                }
                else
                {
                    MachineLabel = "";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur chargement labels: {ex.Message}");
            }
        }

        private async Task ReloadOutilsAsync()
        {
            try
            {
                FiltredOutils.Clear();

                var outils = await _dataService.GetOutilsAsync("", OutilFilter);

                if (outils == null || outils.Count == 0)
                {
                    outils = await _dataService.GetOutilsAsync(OutilFilter, "");
                }

                foreach (var item in outils)
                {
                    FiltredOutils.Add(item);
                }

                VisiOutilGrid = (FiltredOutils.Count > 0) ? "Visible" : "Hidden";

                if (FiltredOutils.Count == 0)
                {
                    SelectedOutil = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur rechargement outils: {ex.Message}");
            }
        }

        // ============================================================
        // ASYNC METHODS - NAVIGATION
        // ============================================================

        private void OpenHistory(object obj)
        {
            InfoBar.RestartTimer();
            var historyView = new SerieHistory
            {
                DataContext = new HistoryViewModel()
            };
            historyView.ShowDialog();
        }

        private async Task OpenNewSeriesAsync(string mode)
        {
            InfoBar.RestartTimer();

            // Validation
            if (!await ValidateInputsAsync())
                return;

            try
            {
                IsLoading = true;

                // Récupérer le type de machine
                var machineType = await _dataService.GetMachineTypeAsync(NMachine);

                // Créer le ViewModel approprié selon le type de machine
                switch (machineType)
                {
                    case "CRIP1":
                        await OpenMonoExtrimiteAsync(mode);
                        break;
                    case "CRIP2":
                        await OpenDualExtrimiteAsync(mode);
                        break;
                    case "CRIP3":
                        await OpenTripleExtrimiteAsync(mode);
                        break;
                    case "DENUD":
                        await OpenDenudeuseAsync(mode);
                        break;
                    case "CCD":
                        await OpenCoupeCableAsync(mode);
                        break;
                    default:
                        MessageBox.Show($"Type de machine inconnu: {machineType}", "Erreur",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur d'ouverture: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task<bool> ValidateInputsAsync()
        {
            if (string.IsNullOrEmpty(NMachine))
            {
                MessageBox.Show("Vérifier le numéro de machine", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var machineType = await _dataService.GetMachineTypeAsync(NMachine);
            if (string.IsNullOrEmpty(machineType))
            {
                MessageBox.Show("Machine non trouvée", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(NMatricule))
            {
                MessageBox.Show("Vérifier le matricule", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            bool opExists = await _dataService.CheckOpExistAsync(NMatricule);
            if (!opExists)
            {
                MessageBox.Show("Opérateur non trouvé", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        // ============================================================
        // MACHINE-SPECIFIC VIEWS
        // ============================================================

        private async Task OpenCoupeCableAsync(string mode)
        {
            CoupeCableViewModel viewModel;

            if (mode == "new")
            {
                var nSerie = await GenerateNSerieAsync();
                viewModel = new CoupeCableViewModel(_dataService, NMachine.ToUpper(), nSerie, NMatricule);
            }
            else if (mode == "surr" && SerieSelected != null)
            {
                var serie = await _dataService.GetSerieAsync(SerieSelected.NoSerie);
                bool hasMarquage = FiltredSeries
                    .Where(s => SerieSelected.NoSerie.Equals(s.NoSerie))
                    .All(s => !string.IsNullOrEmpty(s.Marquage));

                viewModel = new CoupeCableViewModel(_dataService, serie, hasMarquage);
            }
            else
            {
                MessageBox.Show("Sélectionner une série!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var view = new CoupeCableView { DataContext = viewModel };
            viewModel.RequestClose = () => view.Close();
            view.ShowDialog();

            viewModel.ExitLoops = true;
            await ReloadAllSeriesAsync();
            await ReloadFilterAsync();
        }

        private async Task OpenMonoExtrimiteAsync(string mode)
        {
            MonoExtrimiteViewModel viewModel;

            if (mode == "new")
            {
                var nSerie = await GenerateNSerieAsync();
                viewModel = new MonoExtrimiteViewModel(_dataService, NMachine.ToUpper(), nSerie, NMatricule);
            }
            else if (mode == "surr" && SerieSelected != null)
            {
                var serie = await _dataService.GetSerieAsync(SerieSelected.NoSerie);
                viewModel = new MonoExtrimiteViewModel(_dataService, serie);
            }
            else
            {
                MessageBox.Show("Sélectionner une série!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var view = new MonoExtrimite { DataContext = viewModel };
            viewModel.RequestClose = () => view.Close();
            view.ShowDialog();

            viewModel.ExitLoops = true;
            await ReloadAllSeriesAsync();
            await ReloadFilterAsync();
        }

        private async Task OpenDualExtrimiteAsync(string mode)
        {
            DualExtrimiteSertisseuseViewModel viewModel;

            if (mode == "new")
            {
                var nSerie = await GenerateNSerieAsync();
                viewModel = new DualExtrimiteSertisseuseViewModel(_dataService, NMachine.ToUpper(), nSerie, NMatricule);
            }
            else if (mode == "surr" && SerieSelected != null)
            {
                var serie = await _dataService.GetSerieAsync(SerieSelected.NoSerie);
                viewModel = new DualExtrimiteSertisseuseViewModel(_dataService, serie);
            }
            else
            {
                MessageBox.Show("Sélectionner une série!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var view = new DualExtrimiteSertisseuse { DataContext = viewModel };
            viewModel.RequestClose = () => view.Close();
            view.ShowDialog();

            viewModel.ExitLoops = true;
            await ReloadAllSeriesAsync();
            await ReloadFilterAsync();
        }

        private async Task OpenTripleExtrimiteAsync(string mode)
        {
            TripleExtrimiteSetisseuseViewModel viewModel;

            if (mode == "new")
            {
                var nSerie = await GenerateNSerieAsync();
                viewModel = new TripleExtrimiteSetisseuseViewModel(_dataService, NMachine.ToUpper(), nSerie, NMatricule);
            }
            else if (mode == "surr" && SerieSelected != null)
            {
                var serie = await _dataService.GetSerieAsync(SerieSelected.NoSerie);
                viewModel = new TripleExtrimiteSetisseuseViewModel(_dataService, serie);
            }
            else
            {
                MessageBox.Show("Sélectionner une série!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var view = new TripleExtrimiteSertisseuse { DataContext = viewModel };
            viewModel.RequestClose = () => view.Close();
            view.ShowDialog();

            viewModel.ExitLoops = true;
            await ReloadAllSeriesAsync();
            await ReloadFilterAsync();
        }

        private async Task OpenDenudeuseAsync(string mode)
        {
            DenudeuseViewModel viewModel;

            if (mode == "new")
            {
                var nSerie = await GenerateNSerieAsync();
                viewModel = new DenudeuseViewModel(_dataService, NMachine.ToUpper(), nSerie, NMatricule);
            }
            else if (mode == "surr" && SerieSelected != null)
            {
                var serie = await _dataService.GetSerieAsync(SerieSelected.NoSerie);
                viewModel = new DenudeuseViewModel(_dataService, serie);
            }
            else
            {
                MessageBox.Show("Sélectionner une série!", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var view = new DenudeuseView { DataContext = viewModel };
            viewModel.RequestClose = () => view.Close();
            view.ShowDialog();

            viewModel.ExitLoops = true;
            await ReloadAllSeriesAsync();
            await ReloadFilterAsync();
        }

        // ============================================================
        // HELPER METHODS
        // ============================================================

        public async Task<string> GenerateNSerieAsync()
        {
            var lSerie = await _dataService.GetLastSerieAsync();
            int n = int.Parse(lSerie.Substring(2, lSerie.Length - 2)) + 1;
            return "SN" + n.ToString("D4");
        }

        private void OnPropertyChanger(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
