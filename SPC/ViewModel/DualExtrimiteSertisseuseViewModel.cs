using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SPC.Models;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class DualExtrimiteSertisseuseViewModel : INotifyPropertyChanged
    {
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
        private bool BCchicked;
        public bool BCChicked
        {
            get { return BCchicked; }
            set
            {
                BCchicked = value;
                OnPropertyChanged(nameof(BCChicked));
                enregDetail.AspectCnxB = BCchicked ? "Conform" : "Non conform";
            }
        }

        private bool BnCchicked;
        public bool BNCChicked
        {
            get { return BnCchicked; }
            set
            {
                BnCchicked = value;
                OnPropertyChanged(nameof(BNCChicked));
                enregDetail.AspectCnx = !BnCchicked ? "Conform" : "Non conform";
            }
        }
        private bool Cchicked;
        public bool CChicked
        {
            get { return Cchicked; }
            set
            {
                Cchicked = value;
                OnPropertyChanged(nameof(CChicked));
                enregDetail.AspectCnx = Cchicked ? "Conform" : "Non conform";
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
                enregDetail.AspectCnx = !nCchicked ? "Conform" : "Non conform";
            }
        }
        public bool ExitLoops { get; set; }
        private int aa;
        private int bb;
        public string ExtBEnable { get; set; }
        public string VisiExtrimiteB { get; set; }
        private string outilWarningVisibiliteB;
        public string OutilWarningVisibiliteB
        {
            get { return outilWarningVisibiliteB; }
            set
            {
                if (value != outilWarningVisibiliteB)
                {
                    outilWarningVisibiliteB = value;
                    OnPropertyChanged(nameof(OutilWarningVisibiliteB));
                    if(bb == 0)
                    {
                        prvLoopB();
                    }
                }
            }
        }
        private string outilWarningColorB;
        public string OutilWarningColorB
        {
            get { return outilWarningColorB; }
            set
            {
                if (value != outilWarningColorB)
                {
                    outilWarningColorB = value;
                    OnPropertyChanged(nameof(OutilWarningColorB));
                }
            }
        }
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
                    if(aa == 0)
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
                    else if(Regex.IsMatch(value.ToString(), @"^[0-9]+$") && int.Parse(value) <= 500)
                    {
                        quantite = value;
                        enregDetail.Quantite = Int32.Parse(value);
                        OnPropertyChanged(nameof(Quantite));
                    }
                }
            }
        }

        private string denudageB;
        public string DenudageB
        {
            get { return denudageB; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    denudageB = value;
                    enreg.DenudageB = null;
                    OnPropertyChanged(nameof(DenudageB));
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (denudageB != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    denudageB = value;
                    enreg.DenudageB = string.IsNullOrEmpty(value)?null:(decimal?)decimal.Parse(denudageB);
                    OnPropertyChanged(nameof(DenudageB));
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
                    enreg.Denudage = string.IsNullOrEmpty(value)?null:(decimal?)decimal.Parse(denudage);
                    OnPropertyChanged(nameof(Denudage));
                }
            }
        }
        public string ha1B;
        public string HA1B
        {
            get
            {
                return ha1B;
            }
            set
            {
                if (string.IsNullOrEmpty(value)){
                    ha1B = null;
                    WarningNotif(null , "HA1B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (ha1B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha1B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha1B)?(decimal?)null:decimal.Parse(ha1B.Replace(',', '.')), "HA1B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HA1B));
                    enregDetail.HA1B = decimal.Parse(ha1B.Replace(',', '.'));
                }
            }
        }
        public string ha2B;
        public string HA2B
        {
            get
            {
                return ha2B;
            }
            set
            {
                if (value == null){
                    ha2B = null;
                    WarningNotif(null , "HA2B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (ha2B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha2B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha2B)?(decimal?)null:decimal.Parse(ha2B.Replace(',', '.')), "HA2B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HA2B));
                    enregDetail.HA2B = decimal.Parse(ha2B.Replace(',', '.'));
                }
            }
        }
        public string ha3B;
        public string HA3B
        {
            get
            {
                return ha3B;
            }
            set
            {
                if (value == null){
                    ha3B = null;
                    WarningNotif(null , "HA3B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (ha3B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha3B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha3B)?(decimal?)null:decimal.Parse(ha3B.Replace(',', '.')), "HA3B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HA3B));
                    enregDetail.HA3B = decimal.Parse(ha3B.Replace(',', '.'));
                }
            }
        }

        public string hi1B;
        public string HI1B
        {
            get
            {
                return hi1B;
            }
            set
            {
                if (value == null){
                    hi1B = null;
                    WarningNotif(null , "HI1B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (hi1B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    hi1B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(hi1B) ? (decimal?)null : decimal.Parse(hi1B.Replace(',', '.')), "HI1B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HI1B));
                    enregDetail.HI1B = decimal.Parse(hi1B.Replace(',', '.'));
                }
            }
        }
        public string hi2B;
        public string HI2B
        {
            get
            {
                return hi2B;
            }
            set
            {
                if (value == null){
                    hi2B = null;
                    WarningNotif(null , "HI2B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (hi2B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    hi2B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(hi2B) ? (decimal?)null : decimal.Parse(hi2B.Replace(',', '.')), "HI2B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HI2B));
                    enregDetail.HI2B = decimal.Parse(hi2B.Replace(',', '.'));
                }
            }
        }
        public string hi3B;
        public string HI3B
        {
            get
            {
                return hi3B;
            }
            set
            {
                if (value == null){
                    hi3B = null;
                    WarningNotif(null , "HI3B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (hi3B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    hi3B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(hi3B) ? (decimal?)null : decimal.Parse(hi3B.Replace(',', '.')), "HI3B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(HI3B));
                    enregDetail.HI3B = decimal.Parse(hi3B.Replace(',', '.'));
                }
            }
        }
        public string traction1B;
        public string Traction1B
        {
            get
            {
                return traction1B;
            }
            set
            {
                if (value == null){
                    traction1B = null;
                    WarningNotif(null , "Traction1B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (traction1B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    traction1B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(traction1B) ? (decimal?)null : decimal.Parse(traction1B.Replace(',', '.')), "Traction1B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(Traction1B));
                    enregDetail.Traction1B = decimal.Parse(traction1B.Replace(',', '.'));
                }
            }
        }
        public string traction2B;
        public string Traction2B
        {
            get
            {
                return traction2B;
            }
            set
            {
                if (value == null){
                    traction2B = null;
                    WarningNotif(null , "Traction2B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (traction2B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    traction2B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(traction2B) ? (decimal?)null : decimal.Parse(traction2B.Replace(',', '.')), "Traction2B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(Traction2B));
                    enregDetail.Traction2B = decimal.Parse(traction2B.Replace(',', '.'));
                }
            }
        }
        public string traction3B;
        public string Traction3B
        {
            get
            {
                return traction3B;
            }
            set
            {
                if (value == null){
                    traction3B = null;
                    WarningNotif(null , "Traction3B");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (traction3B != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    traction3B = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(traction3B) ? (decimal?)null : decimal.Parse(traction3B.Replace(',', '.')), "Traction3B");
                        WarningNotifTimer.Stop();
                    };
                    WarningNotifTimer.Stop();
                    WarningNotifTimer.Start();

                    OnPropertyChanged(nameof(Traction3B));
                    enregDetail.Traction3B = decimal.Parse(traction3B.Replace(',', '.'));
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
                if (string.IsNullOrEmpty(value)){
                    ha1 = null;
                    WarningNotif(null , "HA1");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (ha1 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha1 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha1)?(decimal?)null:decimal.Parse(ha1.Replace(',', '.')), "HA1");
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
                if (value == null){
                    ha2 = null;
                    WarningNotif(null , "HA2");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (ha2 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha2 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha2)?(decimal?)null:decimal.Parse(ha2.Replace(',', '.')), "HA2");
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
                if (value == null){
                    ha3 = null;
                    WarningNotif(null , "HA2");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
                if (ha3 != value && Regex.IsMatch(value.ToString(), @"^[0-9]+\,?[0-9]{0,3}$"))
                {
                    ha3 = value;

                    WarningNotifTimer.Tick += (s, e) =>
                    {
                        WarningNotif(string.IsNullOrEmpty(ha3)?(decimal?)null:decimal.Parse(ha3.Replace(',', '.')), "HA3");
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
                if (value == null){
                    hi1 = null;
                    WarningNotif(null , "HI1");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
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
                if (value == null){
                    hi2 = null;
                    WarningNotif(null , "HI2");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
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
                if (value == null){
                    hi3 = null;
                    WarningNotif(null , "HI3");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
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
                if (value == null){
                    traction1 = null;
                    WarningNotif(null , "Traction1");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
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
                if (value == null){
                    traction2 = null;
                    WarningNotif(null , "Traction2");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
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
                if (value == null){
                    traction3 = null;
                    WarningNotif(null , "Traction3");
                    return;
                }
                if(value.Last() == '.') value = value.Substring(0, value.Length - 1 ) + ',';
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

        private string nOutilB;
        public string NOutilB
        {
            get { return nOutilB; }
            set {
                if (value.ToUpper() != nOutilB)
                {
                    enreg.NoOutilB = value.ToUpper();
                    nOutilB = value.ToUpper();
                    FilterOutils();
                    WarningNotifCheckingAll();
                    var prv = OtaPrvntfManager.GetPrvntf(NOutilB);
                    OutilWarningVisibiliteB = prv == null || ( prv.DtPrv > DateTime.Now.AddYears(-1) && prv.QtAct < 0.8 * prv.PrvQt ) ? "Hidden" : "Visible";
                }
            }
        }
        private string connexionB;

        public string ConnexionB
        {
            get { return connexionB; }
            set {
                if (value != connexionB)
                {
                    enreg.ConnexionB = value;
                    connexionB = value;
                    FilterOutils();
                    WarningNotifCheckingAll();
                }
            }
        }

        private string nOutil;
        public string NOutil
        {
            get { return nOutil; }
            set {
                if (value.ToUpper() != nOutil)
                {
                    enreg.NoOutil = value.ToUpper();
                    nOutil = value.ToUpper();
                    FilterOutils();
                    WarningNotifCheckingAll();
                    var prv = OtaPrvntfManager.GetPrvntf(NOutil);
                    OutilWarningVisibilite = prv == null || ( prv.DtPrv > DateTime.Now.AddYears(-1) && prv.QtAct < 0.8 * prv.PrvQt ) ? "Hidden" : "Visible";
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
                    FilterOutils();
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
                    enreg.Section = null;
                    OnPropertyChanged(nameof(Section));
                    WarningNotifCheckingAll();
                    return;
                }
                if (value.Last() == '.') value = value.Substring(0, value.Length - 1) + ',';
                if (section != value && Regex.IsMatch(value.ToString(), @"^([0-9]+\,?[0-9]{0,3})(\+([0-9]+\,?[0-9]{0,3})?)?$"))
                {
                    section = value;
                    enreg.Section = value;
                    if (!string.IsNullOrEmpty(NOutil) && !string.IsNullOrEmpty(Connexion))
                    {
                        FilterOutils();
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

        public string WarningColor { get; set; }
        public string Warning { get; set; }
        public string VisiWarning { get; set; }

        public string VisiDataGrid { get; set; }

        public string ColorHA1B { get; set; }
        public string ColorHA2B { get; set; }
        public string ColorHA3B { get; set; }
        public string ColorHI1B { get; set; }
        public string ColorHI2B { get; set; }
        public string ColorHI3B { get; set; }
        public string ColorTraction1B { get; set; }
        public string ColorTraction2B { get; set; }
        public string ColorTraction3B { get; set; }

        public string WarningColorB { get; set; }
        public string WarningB { get; set; }
        public string VisiWarningB { get; set; }

        public string VisiDataGridB { get; set; }

        public string CliColor { get; set; }
        public string CliFor { get; set; }
        public string VisiCliWarning { get; set; }


        public string VisiRefWarning { get; set; }
        public string RefColor { get; set; }
        public string RefFor { get; set; }
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

        public SPCEnregComplet enregComplet { get; set; }
        public SPCEnreg enreg { get; set; }
        public SPCEnregDetail enregDetail { get; set; }
        public ObservableCollection<Outil> Outils { get; set; }
        public ObservableCollection<Outil> NonFiltredOutils { get; set; }
        public ObservableCollection<Outil> OutilsB { get; set; }
        public ObservableCollection<Outil> NonFiltredOutilsB { get; set; }
        public List<string> Clients { get; set; }
        public List<string> ItemsSourceD { get; set; }
        public Action RequestClose { get; set; }
        public ICommand SaveCommand { get; set; }

        private List<string> warnings;
        private List<string> warningsB;

        public event PropertyChangedEventHandler PropertyChanged;
        public DualExtrimiteSertisseuseViewModel(string NMachine, string NSerie, string NMatricule)
        {
            enreg = new SPCEnreg()
            {
                NoSerie = NSerie,
                NoMachine = NMachine,
                OperationNo = NMatricule
            };
            Init(); //should be bellow enreg cause i use it in the Init

            VisiExtrimiteB = "Visible";
            ItemsSourceD = new List<string>() { "D", "D-F" };
            EnregReadOnlyProp = "false";
        }

        public DualExtrimiteSertisseuseViewModel(SPCEnreg enreg)
        {
            this.enreg = enreg;

            Init(); //should be bellow enreg cause i use it in the Init

            Section = enreg.Section;

            Connexion = enreg.Connexion;
            NOutil = enreg.NoOutil;
            Denudage = enreg.Denudage.ToString().Replace('.',',');

            if (enreg.DenudageB != null)
            {
                VisiExtrimiteB = "Visible";
                ConnexionB = enreg.ConnexionB;
                NOutilB = enreg.NoOutilB;
                DenudageB = enreg.DenudageB.ToString().Replace('.',',');
            }

            else VisiExtrimiteB = "Hidden";

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
            OutilsB = new ObservableCollection<Outil>();
            SaveCommand = new RelayCommand(SaveSerie, parm => true);
            enregDetail = new SPCEnregDetail()
            {
                DateCreation = DateTime.Now,
            };
            WarningNotifTimer =new DispatcherTimer();
            WarningNotifTimer.Interval = TimeSpan.FromMilliseconds(1000);

            enreg.RefSizeTester += () => RefSizeAct();
            enreg.CliAbsTester += () => CliAbsAct();

            ExitLoops = false;
        }
        private void ViewInit()
        {
            warnings = new List<string>();
            warningsB = new List<string>();
            Warning = "HORS TOLERANCE";
            WarningColor = "red";
            WarningColorB = "red";
            RefColor = "LightGreen";
            RefFor = "White";
            VisiRefWarning= "Hidden";
            CliColor = "LightGreen";
            CliFor = "White";
            VisiCliWarning= "Hidden";
            VisiDataGrid = "Hidden";
            VisiDataGridB = "Hidden";
            VisiWarning = "Hidden";
            VisiWarningB = "Hidden";
            OutilWarningColor = "red";
            OutilWarningColorB = "red";
            OutilWarningVisibilite = "Hidden";
            OutilWarningVisibiliteB = "Hidden";

            ExtBEnable = "false";

            InitWarningColor();
            InitWarningColorB();

        }
        private void WarningNotifCheckingAll()
        {
            WarningNotif(string.IsNullOrEmpty(HA1)?(decimal?)null:decimal.Parse(HA1.Replace(',', '.')), "HA1");
            WarningNotif(string.IsNullOrEmpty(HA2)?(decimal?)null:decimal.Parse(HA2.Replace(',', '.')), "HA2");
            WarningNotif(string.IsNullOrEmpty(HA3)?(decimal?)null:decimal.Parse(HA3.Replace(',', '.')), "HA3");

            WarningNotif(string.IsNullOrEmpty(HI1)?(decimal?)null:decimal.Parse(HI1.Replace(',', '.')), "HI1");
            WarningNotif(string.IsNullOrEmpty(HI2)?(decimal?)null:decimal.Parse(HI2.Replace(',', '.')), "HI2");
            WarningNotif(string.IsNullOrEmpty(HI3)?(decimal?)null:decimal.Parse(HI3.Replace(',', '.')), "HI3");

            WarningNotif(string.IsNullOrEmpty(Traction1)?(decimal?)null:decimal.Parse(Traction1.Replace(',', '.')), "Traction1");
            WarningNotif(string.IsNullOrEmpty(Traction2)?(decimal?)null:decimal.Parse(Traction2.Replace(',', '.')), "Traction2");
            WarningNotif(string.IsNullOrEmpty(Traction3)?(decimal?)null:decimal.Parse(Traction3.Replace(',', '.')), "Traction3");

            WarningNotif(string.IsNullOrEmpty(HA1B)?(decimal?)null:decimal.Parse(HA1B.Replace(',', '.')), "HA1B");
            WarningNotif(string.IsNullOrEmpty(HA2B)?(decimal?)null:decimal.Parse(HA2B.Replace(',', '.')), "HA2B");
            WarningNotif(string.IsNullOrEmpty(HA3B)?(decimal?)null:decimal.Parse(HA3B.Replace(',', '.')), "HA3B");

            WarningNotif(string.IsNullOrEmpty(HI1B)?(decimal?)null:decimal.Parse(HI1B.Replace(',', '.')), "HI1B");
            WarningNotif(string.IsNullOrEmpty(HI2B)?(decimal?)null:decimal.Parse(HI2B.Replace(',', '.')), "HI2B");
            WarningNotif(string.IsNullOrEmpty(HI3B)?(decimal?)null:decimal.Parse(HI3B.Replace(',', '.')), "HI3B");

            WarningNotif(string.IsNullOrEmpty(Traction1B)?(decimal?)null:decimal.Parse(Traction1B.Replace(',', '.')), "Traction1B");
            WarningNotif(string.IsNullOrEmpty(Traction2B)?(decimal?)null:decimal.Parse(Traction2B.Replace(',', '.')), "Traction2B");
            WarningNotif(string.IsNullOrEmpty(Traction3B)?(decimal?)null:decimal.Parse(Traction3B.Replace(',', '.')), "Traction3B");
        }
        public void WarningNotif(decimal? v, string p) 
        {

            var extrimiteB = p.Contains("B");

            if(!extrimiteB && (Outils.Count == 0 || (string.IsNullOrEmpty(Section) || string.IsNullOrEmpty(Connexion) || string.IsNullOrEmpty(NOutil))))    
            {
                warnings.RemoveAll(s=> !s.Contains("B"));
                InitWarningColor();
                return;
            }
            if(extrimiteB && (OutilsB.Count == 0 || (string.IsNullOrEmpty(Section) || string.IsNullOrEmpty(ConnexionB) || string.IsNullOrEmpty(NOutilB))))
            {
                warnings.RemoveAll(s=> s.Contains("B"));
                InitWarningColor();
                return;
            }

            var outil = extrimiteB?OutilsB[0]:Outils[0];

            string warn = p;

            if (p.Contains("HA"))
            {
                if (CompareBetween(v, outil.Hame, outil.TolHa))
                {
                    if (warnings.Contains(warn))
                    {
                        warnings.Remove(warn);
                    }
                }
                else
                {
                    if (!warnings.Contains(warn))
                    {
                        warnings.Add(warn);
                    }
                    if(extrimiteB && warnings.Count(s => s.Contains("B")) == 1) WarningLoopB();
                    else if (warnings.Count(s => !s.Contains("B")) == 1) WarningLoop();
                }
            }
            else if (p.Contains("HI"))
            {
                if (CompareBetween(v, outil.Hisolant, outil.TolHi))
                {
                    if (warnings.Contains(warn))
                    {
                        warnings.Remove(warn);
                    }
                }
                else
                {
                    if (!warnings.Contains(warn))
                    {
                        warnings.Add(warn);
                    }
                    if(extrimiteB && warnings.Count(s=> s.Contains("B")) == 1) WarningLoopB();
                    else if (warnings.Count(s=> !s.Contains("B")) == 1) WarningLoop();
               }
            }
            else if (p.Contains("Traction"))
            {
                if(v < outil.Trac)
                {
                    if (!warnings.Contains(warn))
                    {
                        warnings.Add(warn);
                    }
                    if(extrimiteB && warnings.Count(s=> s.Contains("B")) == 1) WarningLoopB();
                    else if (warnings.Count(s=> !s.Contains("B")) == 1) WarningLoop();
                }
                else
                {
                    if (warnings.Contains(warn))
                    {
                        warnings.Remove(warn);
                    }
                }
            }
            SetWarningColor(warn);
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
        private void InitWarningColorB()
        {
            ColorHA1B = "white";
            ColorHA2B = "white";
            ColorHA3B = "white";
            ColorHI1B = "white";
            ColorHI2B = "white";
            ColorHI3B = "white";
            ColorTraction1B = "white";
            ColorTraction2B = "white";
            ColorTraction3B = "white";
            OnPropertyChanged(nameof(ColorHA1B));
            OnPropertyChanged(nameof(ColorHA2B));
            OnPropertyChanged(nameof(ColorHA3B));
            OnPropertyChanged(nameof(ColorHI1B));
            OnPropertyChanged(nameof(ColorHI2B));
            OnPropertyChanged(nameof(ColorHI3B));
            OnPropertyChanged(nameof(ColorTraction1B));
            OnPropertyChanged(nameof(ColorTraction2B));
            OnPropertyChanged(nameof(ColorTraction3B));
        }

        public void SetWarningColor(string warning)
        {
            if (warning == "HA1")
            {
                ColorHA1 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA1));
            }
            if (warning == "HA2") { 
                ColorHA2 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA2));
            }
            if (warning == "HA3")
            {
                ColorHA3 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA3));
            }
            if (warning == "HI1")
            {
                ColorHI1 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI1));
            }
            if (warning == "HI2")
            {
                ColorHI2 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI2));
            }
            if (warning == "HI3")
            {
                ColorHI3 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI3));
            }
            if (warning == "Traction1") 
            {
                ColorTraction1 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction1));
            }
            if (warning == "Traction2")
            {
                ColorTraction2 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction2));
            }
            if (warning == "Traction3")
            {
                ColorTraction3 = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction3));
            }

            if (warning == "HA1B")
            {
                ColorHA1B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA1B));
            }
            if (warning == "HA2B") { 
                ColorHA2B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA2B));
            }
            if (warning == "HA3B")
            {
                ColorHA3B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHA3B));
            }
            if (warning == "HI1B")
            {
                ColorHI1B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI1B));
            }
            if (warning == "HI2B")
            {
                ColorHI2B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI2B));
            }
            if (warning == "HI3B")
            {
                ColorHI3B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorHI3B));
            }
            if (warning == "Traction1B") 
            {
                ColorTraction1B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction1B));
            }
            if (warning == "Traction2B")
            {
                ColorTraction2B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction2B));
            }
            if (warning == "Traction3B")
            {
                ColorTraction3B = warnings.Contains(warning) ? "PaleVioletRed" : "white";
                OnPropertyChanged(nameof(ColorTraction3B));
            }
        }
        private async void prvLoopB()
        {
            bb++;
            while (OutilWarningVisibiliteB == "Visible" && !ExitLoops)
            {
                OutilWarningColorB = "Red";
                await Task.Delay(400);
                OutilWarningColorB = "Yellow";
                await Task.Delay(400);
            }
            bb--;
        }
        private async void prvLoop()
        {
            aa++;
            while (OutilWarningVisibilite == "Visible" && !ExitLoops)
            {
                OutilWarningColor = "Red";
                await Task.Delay(400);
                OutilWarningColor = "Yellow";
                await Task.Delay(400);
            }
            aa--;
        }
        private async void WarningLoopB()
        {
            VisiWarningB = "Visible";
            OnPropertyChanged(nameof(VisiWarningB));
            while (warnings.Any(s=>s.Contains("B")) && !ExitLoops)
            {
                WarningColorB = "red";
                OnPropertyChanged(nameof(WarningColorB));
                await Task.Delay(200);
                WarningColorB = "yellow";
                OnPropertyChanged(nameof(WarningColorB));
                await Task.Delay(200);
            }
            VisiWarningB = "Hidden";
            OnPropertyChanged(nameof(VisiWarningB));
       }
        private async void WarningLoop()
        {
            VisiWarning = "Visible";
            OnPropertyChanged(nameof(VisiWarning));
            while (!warnings.All(s=>s.Contains("B")) && !ExitLoops)
            {
                WarningColor = "red";
                OnPropertyChanged(nameof(WarningColor));
                await Task.Delay(200);
                WarningColor = "yellow";
                OnPropertyChanged(nameof(WarningColor));
                await Task.Delay(200);
            }
            VisiWarning = "Hidden";
            OnPropertyChanged(nameof(VisiWarning));
       }

        private bool CompareBetween(decimal? value1, decimal? value2, decimal? tol)
        {
            if (value1 == null || value2 == null) return true;
            return !(value1 > value2+tol || value1 < value2-tol);
        }
        private void SaveSerie(object obj)
        {
            if (SaveCheck())
            {
                enreg.HA = Outils[0].Hame;
                enreg.HI = Outils[0].Hisolant;
                enreg.Traction = Outils[0].Trac;
                SPCEnregManager.InsertNew(enreg);

                enregDetail.IdEnrg = SPCEnregManager.GetId(enreg.NoSerie);
                OtaPrvntfManager.UpdatePrvntf(NOutil, (int)enregDetail.Quantite);
                if(!string.IsNullOrEmpty(NOutilB))
                    OtaPrvntfManager.UpdatePrvntf(NOutilB, (int)enregDetail.Quantite);

                SPCEnregDetailManager.InsertNew(enregDetail);
                RequestClose?.Invoke();
            }
            else
            {
                MessageBox.Show("Error"); //ToDo add sepecific ...
            }
        }
        private bool CheckFull(string Ext = "A")
        {
            List<string> enregProps = new List<string>{"Client", "Ref", "Section"};
            List<string> enregDetailProps = new List<string>{"Repere", "Nature", "Quantite"};
            if (Ext == "A") 
            {
                enregProps.AddRange(new List<string> {"Connexion", "Denudage", "NoOutil" });
                enregDetailProps.AddRange(new List<String> 
                { "HA1", "HA2", "HA3", "HI1", "HI2", "HI3"
                ,"Traction1" ,"Traction2","Traction3","NoOutil", "AspectCnx",});
            }
            else if (Ext == "B") 
            {
                if (string.IsNullOrEmpty(enreg.ConnexionB) && string.IsNullOrEmpty(enreg.NoOutilB)) return true;
                enregProps.AddRange(new List<string> {"ConnexionB", "DenudageB", "NoOutilB"});
                enregDetailProps.AddRange(new List<string> 
                { "HA1B", "HA2B", "HA3B", "HI1B", "HI2B", "HI3B"
                ,"Traction1B" ,"Traction2B","Traction3B","NoOutilB", "AspectCnxB",});
            }
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
            OnPropertyChanged(nameof(RefColor));
            OnPropertyChanged(nameof(RefFor));
            OnPropertyChanged(nameof(VisiRefWarning));
        }

        public void FilterOutils()
        {
            OutilsB.Clear();
            var outilsB = OutilManager.GetOutilsByCndO(NOutilB, ConnexionB);

            foreach (var item in outilsB) 
            {
                if (item.Sec.Equals(Section) || string.IsNullOrEmpty(Section)) OutilsB.Add(item);
            }
            VisiDataGridB = OutilsB.Count() == 0 ? "Hidden" : "Visible";
            OnPropertyChanged(nameof(VisiDataGridB));

            Outils.Clear();
            var outils = OutilManager.GetOutilsByCndO(NOutil, Connexion);

            foreach (var item in outils) 
            {
                if (item.Sec.Equals(Section) || string.IsNullOrEmpty(Section)) Outils.Add(item);
            }
            VisiDataGrid = Outils.Count() == 0 ? "Hidden" : "Visible";
            OnPropertyChanged(nameof(VisiDataGrid));

            ExtBEnable = Outils.Count() == 0 ? "false" : "true";
            OnPropertyChanged(nameof(ExtBEnable));
        }
        private bool SaveCheck()
        {

            if (!CheckFull() || !CheckFull("B"))
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

            var prv = OtaPrvntfManager.GetPrvntf(string.IsNullOrEmpty(NOutil)? "" : NOutil);
            bool prvcheck = prv == null ? false : prv.QtAct + enregDetail.Quantite < prv.PrvQt;

            if (!prvcheck)
            {
                MessageBox.Show("pas possible, fait le preventife.");
                return false;
            }

            if (!string.IsNullOrEmpty(NOutilB))
            {
                var prvB = OtaPrvntfManager.GetPrvntf(string.IsNullOrEmpty(NOutilB)? "" : NOutilB);
                bool prvcheckB = prvB == null ? false : prvB.QtAct + enregDetail.Quantite < prvB.PrvQt;

                if (!prvcheckB)
                {
                    MessageBox.Show($"pas possible, fait le preventife pour l'outil {NOutilB}.");
                    return false;
                }
            }

            return true;
        }
        private void OnPropertyChanged(string Name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name));
        }
    }
}
