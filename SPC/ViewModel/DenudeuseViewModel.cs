using SPC.Models;
using SPC.Services;
using SPC.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SPC.ViewModel
{
    public class DenudeuseViewModel : INotifyPropertyChanged
    {
        // ============================================================
        // SERVICES & DEPENDENCIES
        // ============================================================

        private readonly ISpcDataService _dataService;

        // ============================================================
        // ISLOADING PROPERTY
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

        // ============================================================
        // STRING PROPERTIES WITH VALIDATION
        // ============================================================

        private string _quantite;
        public string Quantite
        {
            get => _quantite;
            set
            {
                if (_quantite != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        _quantite = value;
                        enregDetail.Quantite = null;
                    }
                    else if (Regex.IsMatch(value, @"^[0-9]+$") && int.Parse(value) <= 500)
                    {
                        _quantite = value;
                        enregDetail.Quantite = int.Parse(value);
                        OnPropertyChanged(nameof(Quantite));
                    }
                }
            }
        }

        private string _nature;
        public string Nature
        {
            get => _nature;
            set
            {
                _nature = value;
                enregDetail.Nature = value;
                NatureLabel = value == "D" ? "Debut"
                    : value == "F" ? "Fin"
                    : value == "S" ? "Surveillance"
                    : value == "D-F" ? "Debut/Fin"
                    : "";
                OnPropertyChanged(nameof(NatureLabel));
            }
        }

        private string _section;
        public string Section
        {
            get => _section;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _section = null;
                    enreg.Section = null;
                    OnPropertyChanged(nameof(Section));
                    return;
                }

                if (value.Last() == '.')
                    value = value.Substring(0, value.Length - 1) + ',';

                if (_section != value && Regex.IsMatch(value, @"^([0-9]+\,?[0-9]{0,3})(\+([0-9]+\,?[0-9]{0,3})?)?$"))
                {
                    _section = value;
                    enreg.Section = value;
                    OnPropertyChanged(nameof(Section));
                }
            }
        }

        private string _denudage;
        public string Denudage
        {
            get => _denudage;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _denudage = value;
                    enreg.Denudage = null;
                    OnPropertyChanged(nameof(Denudage));
                    return;
                }

                if (value.Last() == '.')
                    value = value.Substring(0, value.Length - 1) + ',';

                if (_denudage != value && Regex.IsMatch(value, @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    _denudage = value;
                    enreg.Denudage = string.IsNullOrEmpty(value)
                        ? null
                        : (decimal?)decimal.Parse(_denudage.Replace(',', '.'));
                    OnPropertyChanged(nameof(Denudage));
                }
            }
        }

        private string _denudage1;
        public string Denudage1
        {
            get => _denudage1;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _denudage1 = value;
                    enregDetail.Denudage1 = null;
                    OnPropertyChanged(nameof(Denudage1));
                    return;
                }

                if (value.Last() == '.')
                    value = value.Substring(0, value.Length - 1) + ',';

                if (_denudage1 != value && Regex.IsMatch(value, @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    _denudage1 = value;
                    enregDetail.Denudage1 = string.IsNullOrEmpty(value)
                        ? null
                        : (decimal?)decimal.Parse(_denudage1.Replace(',', '.'));
                    OnPropertyChanged(nameof(Denudage1));
                }
            }
        }

        private string _denudage2;
        public string Denudage2
        {
            get => _denudage2;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _denudage2 = value;
                    enregDetail.Denudage2 = null;
                    OnPropertyChanged(nameof(Denudage2));
                    return;
                }

                if (value.Last() == '.')
                    value = value.Substring(0, value.Length - 1) + ',';

                if (_denudage2 != value && Regex.IsMatch(value, @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    _denudage2 = value;
                    enregDetail.Denudage2 = string.IsNullOrEmpty(value)
                        ? null
                        : (decimal?)decimal.Parse(_denudage2.Replace(',', '.'));
                    OnPropertyChanged(nameof(Denudage2));
                }
            }
        }

        private string _denudage3;
        public string Denudage3
        {
            get => _denudage3;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _denudage3 = value;
                    enregDetail.Denudage3 = null;
                    OnPropertyChanged(nameof(Denudage3));
                    return;
                }

                if (value.Last() == '.')
                    value = value.Substring(0, value.Length - 1) + ',';

                if (_denudage3 != value && Regex.IsMatch(value, @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    _denudage3 = value;
                    enregDetail.Denudage3 = string.IsNullOrEmpty(value)
                        ? null
                        : (decimal?)decimal.Parse(_denudage3.Replace(',', '.'));
                    OnPropertyChanged(nameof(Denudage3));
                }
            }
        }

        // ============================================================
        // PUBLIC PROPERTIES
        // ============================================================

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public SPCEnreg enreg { get; set; }
        public SPCEnregDetail enregDetail { get; set; }
        public DispatcherTimer WarningNotifTimer { get; private set; }

        private List<string> _clients;
        public List<string> Clients
        {
            get => _clients;
            set
            {
                _clients = value;
                OnPropertyChanged(nameof(Clients));
            }
        }

        public List<string> ItemsSourceD { get; set; }
        public Action RequestClose { get; set; }
        public ICommand SaveCommand { get; set; }
        public bool ExitLoops { get; set; }

        private string _refColor;
        public string RefColor
        {
            get => _refColor;
            private set
            {
                _refColor = value;
                OnPropertyChanged(nameof(RefColor));
            }
        }

        private string _refFor;
        public string RefFor
        {
            get => _refFor;
            private set
            {
                _refFor = value;
                OnPropertyChanged(nameof(RefFor));
            }
        }

        private string _visiRefWarning;
        public string VisiRefWarning
        {
            get => _visiRefWarning;
            private set
            {
                _visiRefWarning = value;
                OnPropertyChanged(nameof(VisiRefWarning));
            }
        }

        public string EnregReadOnlyProp { get; set; }

        private string _natureLabel;
        public string NatureLabel
        {
            get => _natureLabel;
            private set
            {
                _natureLabel = value;
                OnPropertyChanged(nameof(NatureLabel));
            }
        }

        // ============================================================
        // CONSTRUCTORS
        // ============================================================

        // Constructor pour mode surveillance/fin (readonly)
        public DenudeuseViewModel(ISpcDataService dataService, SPCEnreg enreg)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            this.enreg = enreg;

            Section = enreg.Section;
            Denudage = enreg.Denudage?.ToString().Replace('.', ',');

            InitializeSync();
            ItemsSourceD = new List<string>() { "S", "F" };
            EnregReadOnlyProp = "true";

            // ✅ CORRECTION: Charger les données async
            _ = LoadInitialDataAsync();
        }

        // Constructor pour nouveau (editable)
        public DenudeuseViewModel(ISpcDataService dataService, string nMachine, string nSerie, string nMatricule)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            enreg = new SPCEnreg()
            {
                NoSerie = nSerie,
                NoMachine = nMachine,
                OperationNo = nMatricule
            };

            InitializeSync();
            ItemsSourceD = new List<string>() { "D", "D-F" };
            EnregReadOnlyProp = "false";

            // ✅ CORRECTION: Charger les données async
            _ = LoadInitialDataAsync();
        }

        // ============================================================
        // INITIALIZATION
        // ============================================================

        private void InitializeSync()
        {
            // ✅ CORRECTION: Ne PAS charger le nom ici, le faire en async
            ViewInit();

            // ✅ CORRECTION: Ne PAS charger les clients ici, le faire en async
            SaveCommand = new AsyncRelayCommand(SaveSerieAsync, CanSave);

            enregDetail = new SPCEnregDetail()
            {
                DateCreation = DateTime.Now,
            };

            WarningNotifTimer = new DispatcherTimer();
            WarningNotifTimer.Interval = TimeSpan.FromMilliseconds(1000);

            enreg.RefSizeTester += () => RefSizeAct();

            ExitLoops = false;
        }

        // ✅ NOUVELLE MÉTHODE: Chargement async
        private async Task LoadInitialDataAsync()
        {
            IsLoading = true;

            try
            {
                // Charger le nom de l'opérateur
                Name = await _dataService.GetOpNameAsync(enreg.OperationNo);

                // Charger la liste des clients
                Clients = await _dataService.GetClientNamesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ViewInit()
        {
            RefColor = "LightGreen";
            RefFor = "White";
            VisiRefWarning = "Hidden";
        }

        // ============================================================
        // REF SIZE VALIDATION
        // ============================================================

        private void RefSizeAct()
        {
            if (enreg.Ref?.Length < 6)
            {
                RefColor = "Red";
                RefFor = "Yellow";
                VisiRefWarning = "Visible";
            }
            else
            {
                RefColor = "LightGreen";
                RefFor = "White";
                VisiRefWarning = "Hidden";
            }
        }

        // ============================================================
        // SAVE LOGIC (ASYNC)
        // ============================================================

        // ✅ NOUVELLE MÉTHODE ASYNC
        private async Task SaveSerieAsync()
        {
            if (!SaveCheck())
                return;

            IsLoading = true;

            try
            {
                // 1️⃣ Insertion de la série si c'est un début
                if (enregDetail.Nature.Contains("D"))
                {
                    await _dataService.InsertNewSerieAsync(enreg);
                }

                // 2️⃣ Récupération de l'ID
                enregDetail.IdEnrg = await _dataService.GetSerieIdAsync(enreg.NoSerie);

                // 3️⃣ Insertion du détail
                await _dataService.InsertNewDetailAsync(enregDetail);

                // 4️⃣ Fermer la fenêtre après succès
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur lors de l'enregistrement:\n\n{ex.Message}",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                System.Diagnostics.Debug.WriteLine($"Erreur SaveSerieAsync: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private bool CanSave()
        {
            return !IsLoading;
        }

        // ============================================================
        // VALIDATION
        // ============================================================

        private bool CheckFull()
        {
            List<string> enregProps = new List<string>
            {
                "Client", "Ref", "Section", "Denudage"
            };

            List<string> enregDetailProps = new List<string>
            {
                "Repere", "Nature", "Quantite",
                "Denudage1", "Denudage2", "Denudage3"
            };

            // Vérifier les propriétés de enreg
            foreach (var prop in enregProps)
            {
                var propInfo = typeof(SPCEnreg).GetProperty(prop);
                if (propInfo != null)
                {
                    var value = propInfo.GetValue(enreg);

                    if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                    {
                        return false;
                    }
                }
            }

            // Vérifier les propriétés de enregDetail
            foreach (var prop in enregDetailProps)
            {
                var propInfo = typeof(SPCEnregDetail).GetProperty(prop);
                if (propInfo != null)
                {
                    var value = propInfo.GetValue(enregDetail);

                    if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool SaveCheck()
        {
            if (!CheckFull())
            {
                MessageBox.Show("Remplir toutes les cases.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (VisiRefWarning == "Visible")
            {
                MessageBox.Show("Le Ref doit contenir au moins 6 caractères.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
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
