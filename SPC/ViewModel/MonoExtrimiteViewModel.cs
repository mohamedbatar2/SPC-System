 using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using SPC.Models;
using SPC.Services;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class MonoExtrimiteViewModel : INotifyPropertyChanged
    {
        private readonly ISpcDataService _dataService;
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
        public string NatureLabel { get; set; }

        private bool Cchicked;
        public bool CChicked
        {
            get { return Cchicked; }
            set
            {
                Cchicked = value;
                OnPropertyChanged(nameof(CChicked));
                enregDetail.AspectCnx = Cchicked ? "C" : "NC";
            }
        }

        private bool nCchicked;
        public bool NCChicked
        {
            get { return nCchicked; }
            set
            {
                nCchicked = value;
                OnPropertyChanged(nameof(NCChicked));
                enregDetail.AspectCnx = !nCchicked ? "C" : "NC";
            }
        }
        public bool ExitLoops { get; set; }
        private int cc;
        private string outilWarningVisibilite;
        public string OutilWarningVisibilite
        {
            get { return outilWarningVisibilite; }
            set
            {
                if (value != outilWarningVisibilite)
                {
                    outilWarningVisibilite = value;
                    OnPropertyChanged(nameof(OutilWarningVisibilite));
                    if (cc == 0)
                    {
                        prvLoop();
                    }
                }
            }
        }
        private string outilWarningColor;
        public string OutilWarningColor
        {
            get { return outilWarningColor; }
            set
            {
                if (value != outilWarningColor)
                {
                    outilWarningColor = value;
                    OnPropertyChanged(nameof(OutilWarningColor));
                }
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
                    else if (Regex.IsMatch(value.ToString(), @"^[0-9]+$") && int.Parse(value) <= 500)
                    {
                        quantite = value;
                        enregDetail.Quantite = Int32.Parse(value);
                        OnPropertyChanged(nameof(Quantite));
                    }
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
                    //enreg.Denudage = string.IsNullOrEmpty(value)?null:(decimal?)decimal.Parse(denudage); // this was so much wrong
                    enreg.Denudage = string.IsNullOrEmpty(value) ? null : (decimal?)decimal.Parse(denudage.Replace(',', '.'));
                    OnPropertyChanged(nameof(Denudage));
                }
            }
        }
        public string ha1;
        public string HA1
        {
            get
            {
                return ha1;
            }
            set
            {
                if (string.IsNullOrEmpty(value)) {
                    ha1 = null;
                    WarningNotif(null, "HA1");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (ha1 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha1 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha1) ? (decimal?)null : decimal.Parse(ha1.Replace(',', '.')), "HA1");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HA1));
                    enregDetail.HA1 = decimal.Parse(ha1.Replace(',', '.'));
                }
            }
        }
        public string ha2;
        public string HA2
        {
            get
            {
                return ha2;
            }
            set
            {
                if (value == null) {
                    ha2 = null;
                    WarningNotif(null, "HA2");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (ha2 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha2 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha2) ? (decimal?)null : decimal.Parse(ha2.Replace(',', '.')), "HA2");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HA2));
                    enregDetail.HA2 = decimal.Parse(ha2.Replace(',', '.'));
                }
            }
        }
        public string ha3;
        public string HA3
        {
            get
            {
                return ha3;
            }
            set
            {
                if (value == null) {
                    ha3 = null;
                    WarningNotif(null, "HA2");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (ha3 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha3 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha3) ? (decimal?)null : decimal.Parse(ha3.Replace(',', '.')), "HA3");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HA3));
                    enregDetail.HA3 = decimal.Parse(ha3.Replace(',', '.'));
                }
            }
        }

        public string hi1;
        public string HI1
        {
            get
            {
                return hi1;
            }
            set
            {
                if (value == null) {
                    hi1 = null;
                    WarningNotif(null, "HI1");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (hi1 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    hi1 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(hi1) ? (decimal?)null : decimal.Parse(hi1.Replace(',', '.')), "HI1");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HI1));
                    enregDetail.HI1 = decimal.Parse(hi1.Replace(',', '.'));
                }
            }
        }
        public string hi2;
        public string HI2
        {
            get
            {
                return hi2;
            }
            set
            {
                if (value == null) {
                    hi2 = null;
                    WarningNotif(null, "HI2");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (hi2 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    hi2 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(hi2) ? (decimal?)null : decimal.Parse(hi2.Replace(',', '.')), "HI2");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HI2));
                    enregDetail.HI2 = decimal.Parse(hi2.Replace(',', '.'));
                }
            }
        }
        public string hi3;
        public string HI3
        {
            get
            {
                return hi3;
            }
            set
            {
                if (value == null) {
                    hi3 = null;
                    WarningNotif(null, "HI3");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (hi3 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    hi3 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(hi3) ? (decimal?)null : decimal.Parse(hi3.Replace(',', '.')), "HI3");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HI3));
                    enregDetail.HI3 = decimal.Parse(hi3.Replace(',', '.'));
                }
            }
        }
        public string traction1;
        public string Traction1
        {
            get
            {
                return traction1;
            }
            set
            {
                if (value == null) {
                    traction1 = null;
                    WarningNotif(null, "Traction1");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (traction1 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    traction1 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(traction1) ? (decimal?)null : decimal.Parse(traction1.Replace(',', '.')), "Traction1");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(Traction1));
                    enregDetail.Traction1 = decimal.Parse(traction1.Replace(',', '.'));
                }
            }
        }
        public string traction2;
        public string Traction2
        {
            get
            {
                return traction2;
            }
            set
            {
                if (value == null) {
                    traction2 = null;
                    WarningNotif(null, "Traction2");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (traction2 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    traction2 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(traction2) ? (decimal?)null : decimal.Parse(traction2.Replace(',', '.')), "Traction2");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(Traction2));
                    enregDetail.Traction2 = decimal.Parse(traction2.Replace(',', '.'));
                }
            }
        }
        public string traction3;
        public string Traction3
        {
            get
            {
                return traction3;
            }
            set
            {
                if (value == null) {
                    traction3 = null;
                    WarningNotif(null, "Traction1");
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (traction3 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    traction3 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(traction3) ? (decimal?)null : decimal.Parse(traction3.Replace(',', '.')), "Traction3");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(Traction3));
                    enregDetail.Traction3 = decimal.Parse(traction3.Replace(',', '.'));
                }
            }
        }
        public string Name { get; set; }//title

        private string _nOutil;
        public string NOutil
        {
            get => _nOutil;
            set
            {
                if (value.ToUpper() != _nOutil)
                {
                    enreg.NoOutil = value.ToUpper();
                    _nOutil = value.ToUpper();
                    _ = FilterOutilsAsync();  // Fire and forget
                    WarningNotifCheckingAll();
                    _ = CheckOutilPrvntfAsync();  // Nouveau
                }
            }
        }
        private string connexion;

        public string Connexion
        {
            get { return connexion; }
            set {
                if (value != connexion)
                {
                    enreg.Connexion = value;
                    connexion = value;
                    _ = FilterOutilsAsync();
                    WarningNotifCheckingAll();
                }
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
                    OnPropertyChanged(nameof(Section));
                    enreg.Section = null;
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (section != value && Regex.IsMatch(value.ToString(), @"^([0-9]+\,?[0-9]{0,3})(\+([0-9]+\,?[0-9]{0,3})?)?$"))
                {
                    section = value;
                    enreg.Section = value;
                    if (!string.IsNullOrEmpty(NOutil) && !string.IsNullOrEmpty(Connexion))
                    {
                        _ = FilterOutilsAsync();
                        WarningNotifCheckingAll();
                        OnPropertyChanged(nameof(Section));
                    }
                }
            }
        }
        public DispatcherTimer WarningNotifTimer { get; set; }
        public string ColorHA1 { get; set; }
        public string ColorHA2 { get; set; }
        public string ColorHA3 { get; set; }
        public string ColorHI1 { get; set; }
        public string ColorHI2 { get; set; }
        public string ColorHI3 { get; set; }
        public string ColorTraction1 { get; set; }
        public string ColorTraction2 { get; set; }
        public string ColorTraction3 { get; set; }
        public string CliColor { get; set; }
        public string CliFor { get; set; }
        public string VisiCliWarning { get; set; }
        public string WarningColor { get; set; }
        public string Warning { get; set; }
        public string VisiWarning { get; set; }
        public string VisiRefWarning { get; set; }
        public string RefColor { get; set; }
        public string RefFor { get; set; }
        public string VisiDataGrid { get; set; }
        private string enregReadOnlyProp;

        public string EnregReadOnlyProp
        {
            get { return enregReadOnlyProp; }
            set {
                enregReadOnlyProp = value;
                ClientChanging = value == "false" ? "true" : "false";
            }
        }
        public string ClientChanging { get; set; }

        public SPCEnregComplet enregComplet { get; set; }
        public SPCEnreg enreg { get; set; }
        public SPCEnregDetail enregDetail { get; set; }
        public ObservableCollection<Outil> Outils { get; set; }
        public ObservableCollection<Outil> NonFiltredOutils { get; set; }
        public List<string> Clients { get; set; }
        public List<string> ItemsSourceD { get; set; }
        public Action RequestClose { get; set; }
        public ICommand SaveCommand { get; set; }

        private List<string> warnings;

        public event PropertyChangedEventHandler PropertyChanged;
        public MonoExtrimiteViewModel(ISpcDataService dataService, string NMachine, string NSerie, string NMatricule)
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
            _ = LoadInitialDataAsync();
        }

        public MonoExtrimiteViewModel(ISpcDataService dataService,SPCEnreg enreg)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            this.enreg = enreg;
            InitializeSync(); //should be bellow enreg cause i use it in the Init
            Section = enreg.Section;
            Connexion = enreg.Connexion;
            NOutil = enreg.NoOutil;
            Denudage = enreg.Denudage.ToString().Replace('.', ',');

            ItemsSourceD = new List<string>() { "S", "F" };
            EnregReadOnlyProp = "true";
        }

        private void InitializeSync()
        {
            ViewInit();

            Outils = new ObservableCollection<Outil>();
            SaveCommand = new AsyncRelayCommand(SaveSerieAsync, CanSave);
            enregDetail = new SPCEnregDetail()
            {
                DateCreation = DateTime.Now,
            };
            WarningNotifTimer = new DispatcherTimer();
            WarningNotifTimer.Interval = TimeSpan.FromMilliseconds(1000);

            enreg.RefSizeTester += () => RefSizeAct();
            enreg.CliAbsTester += () => _ = CliAbsActAsync();

            ExitLoops = false;
        }

        private async Task LoadInitialDataAsync()
        {
            IsLoading = true;
            try
            {
                Name = await _dataService.GetOpNameAsync(enreg.OperationNo);
                Clients = await _dataService.GetClientNamesAsync();

                // Convertir List en ObservableCollection
                var outilsList = await _dataService.GetAllOutilsAsync();
                NonFiltredOutils = new ObservableCollection<Outil>(outilsList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ViewInit()
        {
            OutilWarningVisibilite = "Hidden";
            warnings = new List<string>();
            Warning = "";
            WarningColor = "red";
            RefColor = "LightGreen";
            RefFor = "White";
            VisiRefWarning = "Hidden";
            CliColor = "LightGreen";
            CliFor = "White";
            VisiCliWarning = "Hidden";
            VisiDataGrid = "Hidden";
            VisiWarning = "Hidden";
            OutilWarningColor = "red";
            InitWarningColor();

        }
        private void WarningNotifCheckingAll()
        {
            WarningNotif(string.IsNullOrEmpty(HA1) ? (decimal?)null : decimal.Parse(HA1.Replace(',', '.')), "HA1");
            WarningNotif(string.IsNullOrEmpty(HA2) ? (decimal?)null : decimal.Parse(HA2.Replace(',', '.')), "HA2");
            WarningNotif(string.IsNullOrEmpty(HA3) ? (decimal?)null : decimal.Parse(HA3.Replace(',', '.')), "HA3");

            WarningNotif(string.IsNullOrEmpty(HI1) ? (decimal?)null : decimal.Parse(HI1.Replace(',', '.')), "HI1");
            WarningNotif(string.IsNullOrEmpty(HI2) ? (decimal?)null : decimal.Parse(HI2.Replace(',', '.')), "HI2");
            WarningNotif(string.IsNullOrEmpty(HI3) ? (decimal?)null : decimal.Parse(HI3.Replace(',', '.')), "HI3");

            WarningNotif(string.IsNullOrEmpty(Traction1) ? (decimal?)null : decimal.Parse(Traction1.Replace(',', '.')), "Traction1");
            WarningNotif(string.IsNullOrEmpty(Traction2) ? (decimal?)null : decimal.Parse(Traction2.Replace(',', '.')), "Traction2");
            WarningNotif(string.IsNullOrEmpty(Traction3) ? (decimal?)null : decimal.Parse(Traction3.Replace(',', '.')), "Traction3");
        }
        public void WarningNotif(decimal? v, string p)
        {
            if (Outils.Count == 0 || (string.IsNullOrEmpty(Section) && string.IsNullOrEmpty(Connexion) && string.IsNullOrEmpty(NOutil)))
            {
                warnings.Clear();
                InitWarningColor();
                return;
            }
            var outil = Outils[0];
            string warn = p;

            if (p.Contains("HA"))
            {
                if (CompareBetween(v, outil.Hame, outil.TolHa))
                {
                    if (warnings.Contains(warn))
                    {
                        warnings.Remove(warn);
                        SetWaringColor(warn);
                    }
                }
                else
                {
                    if (!warnings.Contains(warn))
                    {
                        warnings.Add(warn);
                        SetWaringColor(warn);
                    }
                    if (warnings.Count() == 1)
                    {
                        WarningLoop();
                    }
                }
            }
            else if (p.Contains("HI"))
            {
                if (CompareBetween(v, outil.Hisolant, outil.TolHi))
                {
                    if (warnings.Contains(warn))
                    {
                        warnings.Remove(warn);
                        SetWaringColor(warn);
                    }
                }
                else
                {
                    if (!warnings.Contains(warn))
                    {
                        warnings.Add(warn);
                        SetWaringColor(warn);
                    }
                    if (warnings.Count() == 1)
                    {
                        WarningLoop();
                    }
                }
            }
            else if (p.Contains("Traction"))
            {
                if (v < outil.Trac)
                {
                    if (!warnings.Contains(warn))
                    {
                        warnings.Add(warn);
                        SetWaringColor(warn);
                    }
                    if (warnings.Count() == 1)
                    {
                        WarningLoop();
                    }
                }
                else
                {
                    if (warnings.Contains(warn))
                    {
                        warnings.Remove(warn);
                        SetWaringColor(warn);
                    }
                }
            }
        }
        private void InitWarningColor()
        {
            ColorHA1 = "white";
            ColorHA2 = "white";
            ColorHA3 = "white";
            ColorHI1 = "white";
            ColorHI2 = "white";
            ColorHI3 = "white";
            ColorTraction1 = "white";
            ColorTraction2 = "white";
            ColorTraction3 = "white";
            OnPropertyChanged(nameof(ColorHA1));
            OnPropertyChanged(nameof(ColorHA2));
            OnPropertyChanged(nameof(ColorHA3));
            OnPropertyChanged(nameof(ColorHI1));
            OnPropertyChanged(nameof(ColorHI2));
            OnPropertyChanged(nameof(ColorHI3));
            OnPropertyChanged(nameof(ColorTraction1));
            OnPropertyChanged(nameof(ColorTraction2));
            OnPropertyChanged(nameof(ColorTraction3));
        }

        public void SetWaringColor(string warning)
        {
            if (warning == "HA1")
            {
                ColorHA1 = ColorHA1 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA1));
            }
            if (warning == "HA2") {
                ColorHA2 = ColorHA2 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA2));
            }
            if (warning == "HA3")
            {
                ColorHA3 = ColorHA3 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA3));
            }
            if (warning == "HI1")
            {
                ColorHI1 = ColorHI1 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI1));
            }
            if (warning == "HI2")
            {
                ColorHI2 = ColorHI2 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI2));
            }
            if (warning == "HI3")
            {
                ColorHI3 = ColorHI3 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI3));
            }
            if (warning == "Traction1")
            {
                ColorTraction1 = ColorTraction1 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction1));
            }
            if (warning == "Traction2")
            {
                ColorTraction2 = ColorTraction2 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction2));
            }
            if (warning == "Traction3")
            {
                ColorTraction3 = ColorTraction3 == "white" ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction3));
            }
        }
        private async void prvLoop()
        {
            cc++;
            while (OutilWarningVisibilite == "Visible" && !ExitLoops)
            {
                OutilWarningColor = "Red";
                await Task.Delay(400);
                OutilWarningColor = "Yellow";
                await Task.Delay(400);
            }
            cc--;
        }
        private async void WarningLoop()
        {
            VisiWarning = "Visible";
            OnPropertyChanged(nameof(VisiWarning));
            while (warnings.Count() != 0 && !ExitLoops)
            {
                Warning = "Hort tolerance";
                OnPropertyChanged(nameof(Warning));
                WarningColor = "red";
                OnPropertyChanged(nameof(WarningColor));
                await Task.Delay(200);
                WarningColor = "yellow";
                OnPropertyChanged(nameof(WarningColor));
                await Task.Delay(200);
            }
            Warning = "";
            OnPropertyChanged(nameof(Warning));
            VisiWarning = "Hidden";
            OnPropertyChanged(nameof(VisiWarning));
        }

        private bool CompareBetween(decimal? value1, decimal? value2, decimal? tol)
        {
            if (value1 == null || value2 == null) return true;
            return !(value1 > value2 + tol || value1 < value2 - tol);
        }
        private async Task SaveSerieAsync()
        {
            if (!await SaveCheckAsync())
                return;

            IsLoading = true;
            try
            {
                if (!App.AuthService.IsAuthenticated)
                {
                    await App.AuthService.LoginAsync("admin", "password123");
                }
                if (enregDetail.Nature.Contains("D"))
                {
                    enreg.HA = Outils[0].Hame;
                    enreg.HI = Outils[0].Hisolant;
                    enreg.Traction = Outils[0].Trac;
                    await _dataService.InsertNewSerieAsync(enreg);
                }

                enregDetail.IdEnrg = await _dataService.GetSerieIdAsync(enreg.NoSerie);
                await _dataService.UpdatePrvntfAsync(NOutil, (int)enregDetail.Quantite);
                await _dataService.InsertNewDetailAsync(enregDetail);

                RequestClose?.Invoke();
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show($"Erreur de connexion API: {httpEx.Message}",
                    "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
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
            List<string> enregProps = new List<string>{"Client", "Ref", "Section", "Connexion"
                , "Denudage",};
            List<string> enregDetailProps = new List<string>{"Repere", "Nature", "Quantite"
                ,"HA1", "HA2", "HA3", "HI1", "HI2", "HI3"
                ,"Traction1" ,"Traction2","Traction3","NoOutil", "AspectCnx",
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
        
        private void RefSizeAct()
        {
            if (enreg.Ref.Length < 6)
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
        private async Task CliAbsActAsync()
        {
            try
            {
                var clients = await _dataService.GetClientNamesAsync();

                if (!clients.Contains(enreg.Client))
                {
                    CliColor = "Red";
                    CliFor = "Yellow";
                    VisiCliWarning = "Visible";
                }
                else
                {
                    CliColor = "LightGreen";
                    CliFor = "White";
                    VisiCliWarning = "Hidden";
                }

                OnPropertyChanged(nameof(CliColor));
                OnPropertyChanged(nameof(CliFor));
                OnPropertyChanged(nameof(VisiCliWarning));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking client: {ex.Message}");
            }
        }
        public async Task FilterOutilsAsync()
        {
            try
            {
                Outils.Clear();
                var outils = await _dataService.GetOutilsAsync(NOutil, Connexion);

                foreach (var item in outils)
                {
                    if (item.Sec.Equals(Section) || string.IsNullOrEmpty(Section))
                        Outils.Add(item);
                }
                VisiDataGrid = Outils.Count() == 0 ? "Hidden" : "Visible";
                OnPropertyChanged(nameof(VisiDataGrid));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur filtrage outils: {ex.Message}");
            }
        }
        private async Task<bool> SaveCheckAsync()
        {
            if (!CheckFull())
            {
                MessageBox.Show("Remplire toutes les case.");
                return false;
            }

            if (VisiRefWarning == "Visible")
            {
                MessageBox.Show("Le Ref doit surpasser 6 characters.");
                return false;
            }

            if (Outils.Count() == 0)
            {
                MessageBox.Show("Verifier NOutil / Section / Connexion.");
                return false;
            }

            if (warnings.Count() != 0)
            {
                MessageBox.Show("pas possible, contacter le respensable de machine.");
                return false;
            }

            var prv = await _dataService.GetPrvntfAsync(string.IsNullOrEmpty(NOutil) ? "" : NOutil);
            bool prvcheck = prv == null ? false : prv.QtAct + enregDetail.Quantite < prv.PrvQt;

            if (!prvcheck)
            {
                MessageBox.Show("pas possible, fait le preventife.");
                return false;
            }

            return true;
        }
        private void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
    
    private async Task CheckOutilPrvntfAsync()
        {
            try
            {
                var prv = await _dataService.GetPrvntfAsync(NOutil);
                OutilWarningVisibilite = prv == null ||
                    (prv.DtPrv > DateTime.Now.AddYears(-1) && prv.QtAct < 0.8 * prv.PrvQt)
                    ? "Hidden" : "Visible";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur préventif: {ex.Message}");
            }
        }

    } 
}
