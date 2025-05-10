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
        public ICommand SurrFinCommand { get; set; }
        public ObservableCollection<EnregComplet> FiltredSeries { get; set; }
        public ObservableCollection<EnregComplet> AllSeries { get; set; }
        public string UAP { get; set; }
        private string nMachine;
        public string NMachine
        {
            get { return nMachine; }
            set
            {
                if (nMachine != value)
                {
                    nMachine = value;
                    _timerDispatcher.Stop();
                    _timerDispatcher.Start();
                }
            }
        }
        private string nMatricule;
        public string NMatricule
        {
            get { return nMatricule; }
            set
            {
                if (nMatricule != value)
                {
                    nMatricule = value;
                    _timerDispatcher.Stop();
                    _timerDispatcher.Start();
                }
            }
        }

        private string machineLabel;
        public string MachineLabel
        {
            get { return machineLabel; }
            set
            {
                if (value != machineLabel)
                {
                    opLabel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MachineLabel)));
                }
            }
        }
        private string opLabel;
        public string OperateurLabel
        {
            get { return opLabel; }
            set
            {
                if (value != opLabel)
                {
                    opLabel = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OperateurLabel)));
                }
            }
        }


        public DispatcherTimer _timerDispatcher { get; set; }

        public MainWindowViewModel()
        {
            AllSeries = EnregCompletManager.GetAll();
            FiltredSeries = EnregCompletManager.GetAll();
            NewSeriesCommand = new RelayCommand(obj => OpenNewSeries("new"), parm => true);
            SurrFinCommand = new RelayCommand(obj => OpenNewSeries("surr"), parm => true);

            _timerDispatcher = new DispatcherTimer();
            _timerDispatcher.Interval = TimeSpan.FromMilliseconds(1000);
            _timerDispatcher.Tick += (s, e) =>
            {
                _timerDispatcher.Stop();
                ReloadFilter();
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OpenNewSeries(object obj)
        {
            if (string.IsNullOrEmpty(NMachine)) MessageBox.Show("verifier le machine");
            else if (string.IsNullOrEmpty(NMatricule) || !OperateurManager.CheckOpExist(NMatricule)) MessageBox.Show("verifier le matricule");
            else if (string.Equals("M1", NMachine, StringComparison.OrdinalIgnoreCase))
            {
                MonoExtrimiteViewModel viewModel;
                var laststatus = EnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new") && laststatus.Contains("Fin")) viewModel = new MonoExtrimiteViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule); //Todo selecting with last serie of the op | should the op start a new series when the old one didn't finish
                else if (obj.Equals("new"))
                {
                    //todo here to check if the series ended if you don't wan't to check you could remove last...conaitns("fin") from the previeus if
                    MessageBox.Show("Finir premierement le dernier serie");
                    return;
                } 
                else if (obj.Equals("surr") && (string.IsNullOrEmpty(laststatus) || !laststatus.Contains("Fin"))) viewModel = new MonoExtrimiteViewModel(EnregManager.GetLastSerie(NMachine, NMatricule)); //if there is no prev one it would arise error solve this
                else
                {
                    MessageBox.Show("creer nouveux serie");
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

            if (string.IsNullOrEmpty(NMatricule) || !OperateurManager.CheckOpExist(NMatricule))
            {
                foreach (var item in AllSeries) 
                {
                    FiltredSeries.Add(item);
                }
                OperateurLabel = "";
                return;
            }
            foreach (var item in AllSeries) 
            {
                if (item.OperationNo.Equals(NMatricule))
                {
                    FiltredSeries.Add(item);
                }
            }
            OperateurLabel = OperateurManager.GetOpName(NMatricule);
        }

        public string GenerateNSerie()
        {
            var lSerie = EnregManager.GetLastSerie();
            int n = int.Parse(lSerie.Substring(2, lSerie.Length-2)) + 1;
            return "SN"+n.ToString("D4");
        }
    }
}
