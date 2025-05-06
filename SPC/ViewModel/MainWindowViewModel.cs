using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SPC.Models;
using SPC.Tools;
using SPC.Views;

namespace SPC.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand NewSeriesCommand { get; set; }
        public bool NMachineDisp { get; set; }
        public ObservableCollection<EnregComplet> FiltredSeries { get; set; }
        public ObservableCollection<EnregComplet> AllSeries { get; set; }
        public string NMachine { get; set; }
        public string UAP { get; set; }

        private string nSerie;
        private string Operateur;
        private string NOperateur;
        public string NouvelleLabel { get; set; }
        public string NouvelleColor { get; set; }

        public DispatcherTimer _timerDispatcher { get; set; }   
        public string NSerie
        {
            get { return nSerie; }
            set {
                if (nSerie != value)
                {
                    nSerie = value;
                    _timerDispatcher.Stop();
                    _timerDispatcher.Start();

                }
            }
        }

        public MainWindowViewModel()
        {
            AllSeries = EnregCompletManager.GetAll();
            FiltredSeries = EnregCompletManager.GetAll();
            NewSeriesCommand = new RelayCommand(OpenNewSeries, parm => true);

            _timerDispatcher = new DispatcherTimer();
            _timerDispatcher.Interval = TimeSpan.FromMilliseconds(400);
            _timerDispatcher.Tick += (s, e) =>
            {
                _timerDispatcher.Stop();
                ReloadFilter();

                if (UniqueSerie())
                {
                    NSerie = FiltredSeries[0].NoSerie;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NSerie)));
                }

                NMachineManager();
            };

            NMachineDisp = false;
            NouvelleLabel = "Nouvelle Serie";
            NouvelleColor = "LightBlue";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OpenNewSeries(object obj)
        {
            if (string.IsNullOrEmpty(NMachine)) MessageBox.Show("specifie le machine");
            else if (string.Equals("M1", NMachine, StringComparison.OrdinalIgnoreCase))
            {
                MonoExtrimiteViewModel viewModel;

                if (string.IsNullOrEmpty(NSerie)) viewModel = new MonoExtrimiteViewModel(NMachine.ToUpper(), GenerateNSerie(), UAP);
                else if (EnregManager.IsSerieThere(NSerie)) viewModel = new MonoExtrimiteViewModel(EnregManager.GetSerie(NSerie));
                else
                {
                    MessageBox.Show("il n'ya pas une serie avec cet reference!!");
                    return;
                }

                var view = new MonoExtrimite
                {
                    DataContext = viewModel
                };

                viewModel.RequestClose = () => view.Close();

                view.ShowDialog();
                ReloadAllSeries();
                ReloadFilter();
            }

            else if (string.Equals("M3", NMachine, StringComparison.OrdinalIgnoreCase))
            {
                var viewModel = new MultipleExtrimiteViewModel();
                var view = new MultipleExtrimite
                {
                    DataContext = viewModel
                };
                view.Show();
            }
            else MessageBox.Show($"no machine {NMachine}");
        }
        public void ReloadAllSeries()
        {
            AllSeries = EnregCompletManager.GetAll();
        }
        public void ReloadFilter()
        {
            FiltredSeries.Clear();

            if (string.IsNullOrEmpty(NSerie))
            {
                foreach (var item in AllSeries) 
                {
                    FiltredSeries.Add(item);
                }
            return;
            }
            foreach (var item in AllSeries) 
            {
                if (item.NoSerie.Contains(NSerie))
                {
                    FiltredSeries.Add(item);
                }
            }
        }

        private void NMachineManager()
        {
            if (EnregManager.IsSerieThere(NSerie))
            {
                NMachine = EnregManager.GetSerie(NSerie).NoMachine;
                NMachineDisp = true;
                NouvelleColor = "YellowGreen";
                NouvelleLabel = "Surveillance";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NouvelleColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NouvelleLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NMachine)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NMachineDisp)));
            }
            else
            {
                NMachine = null;
                NMachineDisp = false;
                NouvelleColor = "LightBlue";
                NouvelleLabel = "Nouvelle Serie";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NouvelleColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NouvelleLabel)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NMachine)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NMachineDisp)));
            }
        }

        private bool UniqueSerie()
        {
            string nserie="";
            int run=0;
            if (FiltredSeries.Count <= 0) return false;
            foreach(var ec in FiltredSeries)
            {
                if (run == 0)
                {
                    nserie = ec.NoSerie;
                    run++;
                }
                else if(nserie != ec.NoSerie)
                {
                    return false;
                }
            }
            return true;
        }
        public string GenerateNSerie()
        {
            var lSerie = EnregManager.GetLastSerie();
            int n = int.Parse(lSerie.Substring(2, lSerie.Length-2)) + 1;
            return "SN"+n.ToString("D4");
        }
    }
}
