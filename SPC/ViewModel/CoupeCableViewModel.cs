using SPC.Models;
using SPC.Tools;
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

namespace SPC.ViewModel
{
    public class CoupeCableViewModel : INotifyPropertyChanged
    {
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
                    else if(Regex.IsMatch(value.ToString(), @"^[0-9]+$"))
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
                    enreg.Denudage = string.IsNullOrEmpty(value) ? null : (decimal?)decimal.Parse(denudage);
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
                    enregDetail.Denudage1 = string.IsNullOrEmpty(value)?null:(decimal?)decimal.Parse(denudage1);
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
                    enregDetail.Denudage2 = string.IsNullOrEmpty(value)?null:(decimal?)decimal.Parse(denudage2);
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
                    enreg.LongueurD = string.IsNullOrEmpty(value)?null:(decimal?)decimal.Parse(longueurD);
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
                    enregDetail.LongueurM = string.IsNullOrEmpty(value)?null:(decimal?)decimal.Parse(longueurM);
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

        public CoupeCableViewModel(SPCEnreg enreg)
        {
            this.enreg = enreg;

            Section = enreg.Section;
            Denudage = enreg.Denudage.ToString().Replace(".", ",");
            LongueurD = enreg.LongueurD.ToString().Replace(".", ",");

            Init();
            ItemsSourceD = new List<string>() { "S", "F" };
            EnregReadOnlyProp = "true";
        }
        public CoupeCableViewModel(string NMachine, string NSerie, string NMatricule)
        {
            enreg = new SPCEnreg()
            {
                NoSerie = NSerie,
                NoMachine = NMachine,
                OperationNo = NMatricule
            };
            Init(); //should be bellow enreg cause i use it in the Init

            ItemsSourceD = new List<string>() { "D", "D-F" };
            EnregReadOnlyProp = "false";
        }
        private void Init()
        {
            Name = OperateurManager.GetOpName(enreg.OperationNo);

            ViewInit();

            Clients = ClientManager.GetClientsNames();
            SaveCommand = new RelayCommand(SaveSerie, parm => true);
            enregDetail = new SPCEnregDetail()
            {
                DateCreation = DateTime.Now,
            };

            WarningNotifTimer =new DispatcherTimer();
            WarningNotifTimer.Interval = TimeSpan.FromMilliseconds(1000);

            enreg.RefSizeTester += () => RefSizeAct();

            ExitLoops = false;
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
        private void SaveSerie(object obj)
        {
            if (SaveCheck())
            {
                SPCEnregManager.InsertNew(enreg);

                enregDetail.IdEnrg = SPCEnregManager.GetId(enreg.NoSerie);

                SPCEnregDetailManager.InsertNew(enregDetail);
                RequestClose?.Invoke();
            }
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
                        MessageBox.Show(prop);
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
                        MessageBox.Show(prop);
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
                MessageBox.Show("Remplire toutes les case.");
                return false;
            }

            if (NAChicked = false && NCChicked == false && Cchicked == false)
            {
                MessageBox.Show("Remplire toutes les case.");
                return false;
            }

            if (VisiRefWarning == "Visible")
            {
                MessageBox.Show("Le Ref doit surpasser 6 characters.");
                return false;
            }

            return true;
        }
        private void OnPropertyChanged(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
