using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
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
                    _dispatcherTimer.Stop();
                    _dispatcherTimer.Start();
                }
            }
        }
        public DispatcherTimer _dispatcherTimer { get; set; }

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
            Init();

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
        }
        private void ViewInit()
        {
            warnings = new List<string>();
            Warning = "";
            WarningColor = "red";
            RefColor = "LightGreen";
            RefFor = "White";
            VisiRefWarning= "Hidden";
            VisiDataGrid = "Hidden";
            VisiWarning = "Hidden";
        }
        public void WarningNotif(decimal? v, string p) 
        {
            if(Outils.Count == 0) 
                return;
            var outil = Outils.Last();
            string warn = $"hors tolerance dans {p}";

            if (p.Contains("AH"))
            {
                if(CompareBetween(v, outil.Hame, outil.TolHa))
                {
                    if(warnings.Contains(warn))
                        warnings.Remove(warn);
                }
                else
                {
                    if(!warnings.Contains(warn))
                        warnings.Add(warn);
                    if (warnings.Count() == 1)
                    {
                        WarningLoop();
                    }
                }
            }
            else if (p.Contains("FH"))
            {
                if(CompareBetween(v, outil.Hisolant, outil.TolHi))
                {
                    if(warnings.Contains(warn))
                        warnings.Remove(warn);
                }
                else
                {
                    if(!warnings.Contains(warn))
                        warnings.Add(warn);
                    if (warnings.Count() == 1)
                    {
                        WarningLoop();
                    }
                }
            }
            else if (p.Contains("Traction"))
            {
                if(v < outil.Trac)
                {
                    if(warnings.Contains(warn))
                        warnings.Remove(warn);
                }
                else
                {
                    if(!warnings.Contains(warn))
                        warnings.Add(warn);
                    if (warnings.Count() == 1)
                    {
                        WarningLoop();
                    }
                }
            }
        }

        private async void WarningLoop()
        {
            VisiWarning = "Visible";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisiWarning)));
            while (warnings.Count()!=0)
            {
                Warning = warnings.Last();
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
            if (checkFull())
            {
                enregDetail.Nature = enregDetail.Nature.Equals("D") ? "Debut"
                    : enregDetail.Nature.Equals("D-F") ? "Debut-Fin"
                    : enregDetail.Nature.Equals("F") ? "Fin"
                    : enregDetail.Nature.Equals("S") ? "Sourvillence"
                    : null;

                EnregManager.InsertNew(enreg);

                enregDetail.IdEnrg = EnregManager.GetId(enreg.NoSerie);

                EnregDetailManager.InsertNew(enregDetail);
                RequestClose?.Invoke();
                if (!ClientManager.GetClientsNames().Contains(enreg.Client))
                {
                    ClientManager.AddClient(enreg.Client);
                }
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

            foreach (var item in NonFiltredOutils) 
            {
                if (item.Sec.Contains(string.IsNullOrEmpty(Section)?"":Section) && item.NOutil.Contains(string.IsNullOrEmpty(NOutil)?"":NOutil) && item.Connexion.Contains(string.IsNullOrEmpty(Connexion)?"":Connexion))
                {
                    Outils.Add(item);
                }
            }
            VisiDataGrid = Outils.Count() == 0 ? "Hidden" : "Visible";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisiDataGrid)));
        }
    }
}
