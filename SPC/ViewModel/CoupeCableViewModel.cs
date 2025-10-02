using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SPC.Models;
using SPC.Services;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class CoupeCableViewModel : INotifyPropertyChanged
    {
        private readonly ISpcDataService _dataService;
        public bool IsLoading { get; set; }

        private bool Cchicked;
        public bool CChicked
        {
            get { return Cchicked; }
            set
            {
                Cchicked = value;
                if (value)
                {
                    NAChicked = false;
                    NCChicked = false;
                    enregDetail.Marquage = "C";
                }

                OnPropertyChanged(nameof(CChicked));
            }
        }

        private bool nCchicked;
        public bool NCChicked
        {
            get { return nCchicked; }
            set
            {
                nCchicked = value;
                if (value)
                {
                    NAChicked = false;
                    CChicked = false;
                    enregDetail.Marquage = "NC";
                }

                OnPropertyChanged(nameof(NCChicked));
            }
        }

        private bool nAchicked;
        public bool NAChicked
        {
            get { return nAchicked; }
            set
            {
                nAchicked = value;
                if (value)
                {
                    NCChicked = false;
                    CChicked = false;
                    enregDetail.Marquage = "";
                }
                OnPropertyChanged(nameof(NAChicked));
            }
        }
        private string quantite;
        public string Quantite
        {
            get { return quantite; }
            set {
                if (quantite != value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        quantite = value;
                        enregDetail.Quantite = null;
                    }
                    else if(Regex.IsMatch(value.ToString(), @"^[0-9]+$") && int.Parse(value) <= 500)
                    {
                        quantite = value;
                        enregDetail.Quantite = Int32.Parse(value);
                        OnPropertyChanged(nameof(Quantite));
                    }
                }
            }
        }

        private string nature;
        public string Nature
        {
            get { return nature; }
            set {

                nature = value; 
                enregDetail.Nature = value;
                NatureLabel = Nature == "D" ? "Debut" : Nature == "F" ? "Fin" : Nature == "S" ? "Surveillance" : Nature == "D-F" ? "Debut/Fin" : "";
                OnPropertyChanged(nameof(NatureLabel));
            }
        }
        private string section;
        public string Section
        {
            get { return section; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    section = null;
                    enreg.Section = null;
                    OnPropertyChanged(nameof(Section));
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (section != value && Regex.IsMatch(value.ToString(), @"^([0-9]+\,?[0-9]{0,3})(\+([0-9]+\,?[0-9]{0,3})?)?$"))
                {
                    section = value;
                    enreg.Section = value;
                }
            }
        }
        private string denudage;
        public string Denudage
        {
            get { return denudage; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    denudage = value;
                    enreg.Denudage = null;
                    OnPropertyChanged(nameof(Denudage));
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (denudage != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    denudage = value;
                    enreg.Denudage = string.IsNullOrEmpty(value)? null :(decimal?)decimal.Parse(value.Replace(',','.'));
                    OnPropertyChanged(nameof(Denudage));
                }
            }
        }
        private string denudage1;
        public string Denudage1
        {
            get { return denudage1; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    denudage1 = value;
                    enregDetail.Denudage1 = null;
                    OnPropertyChanged(nameof(Denudage1));
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (denudage1 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    denudage1 = value;
                    enregDetail.Denudage1 = string.IsNullOrEmpty(value)? null :(decimal?)decimal.Parse(value.Replace(',','.'));
                    OnPropertyChanged(nameof(Denudage1));
                }
            }
        }
        private string denudage2;
        public string Denudage2
        {
            get { return denudage2; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    denudage2 = value;
                    enregDetail.Denudage2 = null;
                    OnPropertyChanged(nameof(Denudage2));
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (denudage2 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    denudage2 = value;
                    enregDetail.Denudage2 = string.IsNullOrEmpty(value)? null :(decimal?)decimal.Parse(value.Replace(',','.'));
                    OnPropertyChanged(nameof(Denudage2));
                }
            }
        }
        private string longueurD;
        public string LongueurD
        {
            get { return longueurD; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    longueurD = value;
                    enreg.LongueurD = null;
                    OnPropertyChanged(nameof(longueurD));
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (longueurD != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                     longueurD= value;
                    enreg.LongueurD = string.IsNullOrEmpty(value)? null :(decimal?)decimal.Parse(value.Replace(',','.'));
                    OnPropertyChanged(nameof(LongueurD));
                }
            }
        }
        private string longueurM;
        public string LongueurM
        {
            get { return longueurM; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    longueurM = value;
                    enregDetail.LongueurM = null;
                    OnPropertyChanged(nameof(longueurM));
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (longueurM != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                     longueurM= value;
                    enregDetail.LongueurM = string.IsNullOrEmpty(value)? null :(decimal?)decimal.Parse(value.Replace(',','.'));
                    OnPropertyChanged(nameof(LongueurM));
                }
            }
        }
        public string Name { get; set; }
        public SPCEnreg enreg { get; set; }
        public SPCEnregDetail enregDetail { get; set; }
        public DispatcherTimer WarningNotifTimer { get; private set; }
        public List<string> Clients { get; set; }
        public List<string> ItemsSourceD { get; set; }
        public Action RequestClose { get; set; }
        public ICommand SaveCommand { get; set; }
        public bool ExitLoops { get; set; }
        public string RefColor { get; private set; }
        public string RefFor { get; private set; }
        public string VisiRefWarning { get; private set; }
        public string EnregReadOnlyProp { get; set; }
        public string NatureLabel { get; private set; }
        public string ASPECTVISI { get; set; }

        public CoupeCableViewModel( ISpcDataService dataService,SPCEnreg enreg, bool Aspect)
        {
            _dataService = dataService ??throw new ArgumentNullException(nameof(dataService));

            this.enreg = enreg;

            Section = enreg.Section;
            Denudage = enreg.Denudage.ToString().Replace(".", ",");
            LongueurD = enreg.LongueurD.ToString().Replace(".", ",");

            InitializeSync();
            ItemsSourceD = new List<string>() { "S", "F" };
            EnregReadOnlyProp = "true";

            ASPECTVISI = Aspect?"Visible":"hidden";
        }
        public CoupeCableViewModel(ISpcDataService dataService, string NMachine, string NSerie, string NMatricule)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            enreg = new SPCEnreg()
            {
                NoSerie = NSerie,
                NoMachine = NMachine,
                OperationNo = NMatricule
            };
            InitializeSync(); //should be bellow enreg cause i use it in the Init

            ItemsSourceD = new List<string>() { "D", "D-F" };
            EnregReadOnlyProp = "false";

            ASPECTVISI = "visible";
            _ = LoadInitialDataAsync();
        }
        private void InitializeSync()
        {
            Name = OperateurManager.GetOpName(enreg.OperationNo);

            ViewInit();

            Clients = ClientManager.GetClientsNames();
            SaveCommand = new AsyncRelayCommand(SaveSerieAsync, CanSave);
            enregDetail = new SPCEnregDetail()
            {
                DateCreation = DateTime.Now,
            };

            WarningNotifTimer =new DispatcherTimer();
            WarningNotifTimer.Interval = TimeSpan.FromMilliseconds(1000);

            enreg.RefSizeTester += () => RefSizeAct();

            ExitLoops = false;
        }
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
            VisiRefWarning= "Hidden";
        }
        private void RefSizeAct()
        {
            if(enreg.Ref.Length < 6)
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
            OnPropertyChanged(nameof(RefColor));
            OnPropertyChanged(nameof(RefFor));
            OnPropertyChanged(nameof(VisiRefWarning));
        }
        private async Task SaveSerieAsync()
        {
            if (!SaveCheck())
                return;

            IsLoading = true;

            try
            {
                if (enregDetail.Nature.Contains("D"))
                {
                    await _dataService.InsertNewSerieAsync(enreg);
                }

                enregDetail.IdEnrg = await _dataService.GetSerieIdAsync(enreg.NoSerie);

                await _dataService.InsertNewDetailAsync(enregDetail);

                // Fermer la fenêtre après succès
                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private bool CheckFull()
        {
            List<string> enregProps = new List<string>{"Client", "Ref", "Section", "LongueurD"};
            List<string> enregDetailProps = new List<string>{"Repere", "Nature", "Quantite"
                , "Denudage1", "Denudage2", "LongueurM" 
            };
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

            // FIX: Correction du bug (= au lieu de ==)
            if (!NAChicked && !NCChicked && !CChicked)
            {
                MessageBox.Show("Sélectionner un état de marquage (C, NC ou NA).", "Validation",
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
