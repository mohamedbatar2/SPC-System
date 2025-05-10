using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using SPC.Models;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class MonoExtrimiteViewModel : INotifyPropertyChanged
    {
        public string Name { get; set; }//title

        private string nOutil;
        public string NOutil
        {
            get { return nOutil; }
            set {
                if (value != nOutil)
                {
                    enreg.NoOutil = value;
                    nOutil = value;
                    _dispatcherTimer.Stop();
                    _dispatcherTimer.Start();
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
                    _dispatcherTimer.Stop();
                    _dispatcherTimer.Start();
                }
            }
        }
        private string section;
        public string Section
        {
            get { return section; }
            set
            {
                if (value != section)
                {
                    section = value;
                    enreg.Section = value;
                    if (!string.IsNullOrEmpty(NOutil) && !string.IsNullOrEmpty(Connexion)) FilterOutils();
                }
            }
        }
        public DispatcherTimer _dispatcherTimer { get; set; }

        public string SaveButtonState { get; set; }
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
                ClientChanging = value=="false"? "true" : "false";
            }
        }
        public string ClientChanging { get; set; }

        public EnregComplet enregComplet { get; set; }
        public Enreg enreg { get; set; }
        public EnregDetail enregDetail { get; set; }
        public ObservableCollection<Outil> Outils { get; set; }
        public ObservableCollection<Outil> NonFiltredOutils { get; set; }
        public List<string> Clients { get; set; }
        public List<string> ItemsSourceD { get; set; }
        public Action RequestClose { get; set; }
        public ICommand SaveCommand { get; set; }

        private List<string> warnings;

        public event PropertyChangedEventHandler PropertyChanged;
        public MonoExtrimiteViewModel(string NMachine, string NSerie, string NMatricule)
        {
            enreg = new Enreg()
            {
                NoSerie = NSerie,
                NoMachine = NMachine,
                OperationNo = NMatricule
            };
            Init(); //should be bellow enreg cause i use it in the Init

            ItemsSourceD = new List<string>() { "D", "D-F" };
            EnregReadOnlyProp = "false";
        }

        public MonoExtrimiteViewModel(Enreg enreg)
        {
            this.enreg = enreg;
            Init(); //should be bellow enreg cause i use it in the Init

            Section = enreg.Section;
            Connexion = enreg.Connexion;
            NOutil = enreg.NoOutil;

            ItemsSourceD = new List<string>() { "S", "F" };
            EnregReadOnlyProp = "true";
        }

        private void Init()
        {
            Name = OperateurManager.GetOpName(enreg.OperationNo);

            ViewInit();

            NonFiltredOutils = OutilManager.GetOutils();
            Clients = ClientManager.GetClientsNames();
            Outils = new ObservableCollection<Outil>();
            SaveCommand = new RelayCommand(SaveSerie, parm => true);
            enregDetail = new EnregDetail()
            {
                DateCreation = DateTime.Now,
            };
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(400);
            _dispatcherTimer.Tick += (s, e) =>
            {
                _dispatcherTimer.Stop();
                FilterOutils();
            };

            enregDetail.ChangeColor += (v, l) => WarningNotif(v, l);
            enreg.RefSizeTester += () => RefSizeAct();
            enreg.CliAbsTester += () => CliAbsAct();
        }
        private void ViewInit()
        {
            warnings = new List<string>();
            Warning = "";
            WarningColor = "red";
            RefColor = "LightGreen";
            RefFor = "White";
            VisiRefWarning= "Hidden";
            CliColor = "LightGreen";
            CliFor = "White";
            VisiCliWarning= "Hidden";
            VisiDataGrid = "Hidden";
            VisiWarning = "Hidden";
            ColorHA1 = "white";
            ColorHA2 = "white";
            ColorHA3 = "white";
            ColorHI1 = "white";
            ColorHI2 = "white";
            ColorHI3 = "white";
            ColorTraction1 = "white";
            ColorTraction2 = "white";
            ColorTraction3 = "white";
            SaveButtonState = "false";
        }
        public void WarningNotif(decimal? v, string p) 
        {
            if(Outils.Count == 0) 
                return;
            var outil = Outils[0];
            string warn = p;

            if (p.Contains("AH"))
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
            else if (p.Contains("FH"))
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
                if(v<outil.Trac)
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

        public void SetWaringColor(string warning)
        {
            if (warning == "AH1")
            {
                ColorHA1 = ColorHA1 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorHA1)));
            }
            if (warning == "AH2") { 
                ColorHA2 = ColorHA2 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorHA2)));
            }
            if (warning == "AH3")
            {
                ColorHA3 = ColorHA3 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorHA3)));
            }
            if (warning == "FH1")
            {
                ColorHI1 = ColorHI1 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorHI1)));
            }
            if (warning == "FH2")
            {
                ColorHI2 = ColorHI2 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorHI2)));
            }
            if (warning == "FH3")
            {
                ColorHI3 = ColorHI3 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorHI3)));
            }
            if (warning == "Traction1") 
            {
                ColorTraction1 = ColorTraction1 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorTraction1)));
            }
            if (warning == "Traction2")
            {
                ColorTraction2 = ColorTraction2 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorTraction2)));
            }
            if (warning == "Traction3")
            {
                ColorTraction3 = ColorTraction3 == "white" ? "PaleVioletRed" : "white";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ColorTraction3)));
            }
        }
        private async void WarningLoop()
        {
            VisiWarning = "Visible";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisiWarning)));
            while (warnings.Count()!=0)
            {
                Warning = "Hort tolerance";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
                WarningColor = "red";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WarningColor)));
                await Task.Delay(200);
                WarningColor = "yellow";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WarningColor)));
                await Task.Delay(200);
            }
            Warning = "";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Warning)));
            VisiWarning = "Hidden";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisiWarning)));
       }

        private bool CompareBetween(decimal? value1, decimal? value2, decimal? tol)
        {
            if (value1 == null || value2 == null) return true;
            return !(value1 > value2+tol || value1 < value2-tol);
        }
        private void SaveSerie(object obj)
        {
            if (!ClientManager.GetClientsNames().Contains(enreg.Client))
            {
                MessageBox.Show($"Pas de client {enreg.Client}");
                return;
            }
            else if (checkFull())
            {
                if (Outils.Count() == 0)
                {
                    MessageBox.Show("svp selectionner des valid");
                    return;
                }
                enregDetail.Nature = enregDetail.Nature.Equals("D") ? "Debut"
                    : enregDetail.Nature.Equals("D-F") ? "Debut-Fin"
                    : enregDetail.Nature.Equals("F") ? "Fin"
                    : enregDetail.Nature.Equals("S") ? "Sourvillence"
                    : null;

                EnregManager.InsertNew(enreg);

                enregDetail.IdEnrg = EnregManager.GetId(enreg.NoSerie);

                EnregDetailManager.InsertNew(enregDetail);
                RequestClose?.Invoke();

                //if (!ClientManager.GetClientsNames().Contains(enreg.Client)) //remove the first check and uncomment this to allow clients to be added if absent
                //{
                //    ClientManager.AddClient(enreg.Client);
                //}
            }
            else MessageBox.Show("remplire tout les cas svp");
        }
        private bool checkFull()
        {
            List<string> enregProps = new List<string>{"Client", "Ref", "Section", "Connexion"
                , "Denudage",};
            List<string> enregDetailProps = new List<string>{"Repere", "Nature", "Quantite"
                ,"AH1", "AH2", "AH3", "FH1", "FH2", "FH3"
                ,"Traction1" ,"Traction2","Traction3","NoOutil", "AspectCnx",
            };
            foreach (var prop in enregProps)
            {
                var propInfo = typeof(Enreg).GetProperty(prop);
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
                var propInfo = typeof(EnregDetail).GetProperty(prop);
                if (propInfo != null)
                {
                    var value = propInfo.GetValue(enregDetail);

                    if (value == null || (value is string && string.IsNullOrEmpty((string)value)))
                    {
                        MessageBox.Show(prop);
                        return false;
                    }
                }
            }
            return true;
        }
        private void CliAbsAct()
        {
            if(!ClientManager.GetClientsNames().Contains(enreg.Client))
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CliColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CliFor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisiCliWarning)));
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RefColor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RefFor)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisiRefWarning)));
        }

        public void FilterOutils()
        {
            Outils.Clear();
            var outils = OutilManager.GetOutilsByCndO(NOutil, Connexion);

            foreach (var item in outils) 
            {
                if (item.Sec.Equals(string.IsNullOrEmpty(Section)?"":Section)) Outils.Add(item);
            }
            VisiDataGrid = Outils.Count() == 0 ? "Hidden" : "Visible";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisiDataGrid)));
        }
        public void SaveCheck()
        {
            if (warnings.Count() == 0 && checkFull() && VisiRefWarning == "False" && Outils.Count()!=0) SaveButtonState = "true";

            else SaveButtonState = "false";

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SaveButtonState)));
        }
    }
}
