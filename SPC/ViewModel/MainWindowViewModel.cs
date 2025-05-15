using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
        public ICommand HistoryCommand { get; set; }
        public ICommand NewSeriesCommand { get; set; }
        public ICommand SurrFinCommand { get; set; }
        public ObservableCollection<EnregComplet> FiltredSeries { get; set; }
        public ObservableCollection<EnregComplet> AllSeries { get; set; }
        private EnregComplet serieSelected;

        public EnregComplet SerieSelected
        {
            get { return serieSelected; }
            set {
                if( value == null)
                {
                    serieSelected = value;
                }
                else if ( serieSelected != value) 
                {
                    serieSelected = value;
                    NMachine = value.NoMachine;
                    NMatricule = value.OperationNo;
                }
            }
        }

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
                    OnPropertyChanger(nameof(NMachine));
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
                    OnPropertyChanger(nameof(NMatricule));
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
                    machineLabel = value;
                    OnPropertyChanger(nameof(MachineLabel));
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
                    OnPropertyChanger(nameof(OperateurLabel));
                }
            }
        }

        public DispatcherTimer _timerDispatcher { get; set; }

        public MainWindowViewModel()
        {
            AllSeries = EnregCompletManager.GetAll("NF");
            FiltredSeries = EnregCompletManager.GetAll("NF");
            NewSeriesCommand = new RelayCommand(obj => OpenNewSeries("new"), parm => true);
            SurrFinCommand = new RelayCommand(obj => OpenNewSeries("surr"), parm => true);
            HistoryCommand = new RelayCommand(OpenHistory, parm => true);

            _timerDispatcher = new DispatcherTimer();
            _timerDispatcher.Interval = TimeSpan.FromMilliseconds(1000);
            _timerDispatcher.Tick += (s, e) =>
            {
                _timerDispatcher.Stop();
                ReloadFilter();
            };

            var resetOutilView = new ResetOutil();
            resetOutilView.Show();
        }

        private void OpenHistory(object obj)
        {
            var historyView = new SerieHistory
            {
                DataContext = new HistoryViewModel()
            };
            historyView.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OpenNewSeries(object obj)
        {
            if (string.IsNullOrEmpty(NMachine) || string.IsNullOrEmpty(MachineManager.GetTypeSPC(NMachine))) MessageBox.Show("verifier le machine");
            else if (string.IsNullOrEmpty(NMatricule) || !OperateurManager.CheckOpExist(NMatricule)) MessageBox.Show("verifier le matricule");
            else if (string.Equals(MachineManager.GetTypeSPC(NMachine), "CRIP1"))
            {
                MonoExtrimiteViewModel viewModel;
                var laststatus = EnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new")) viewModel = new MonoExtrimiteViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule);
                else if (obj.Equals("surr") && SerieSelected != null) viewModel = new MonoExtrimiteViewModel(EnregManager.GetSerie(SerieSelected.NoSerie)); //if there is no prev one it would arise error solve this
                else
                {
                    MessageBox.Show("Selectionner une serie!!!!");
                    return;
                }

                var view = new MonoExtrimite
                {
                    DataContext = viewModel
                };

                viewModel.RequestClose = () => view.Close();

                view.ShowDialog();

                viewModel.ExitLoops = true;
                ReloadAllSeries();
                ReloadFilter();
            }

            else if (string.Equals(MachineManager.GetTypeSPC(NMachine), "CRIP2"))
            {

                DualExtrimiteSertisseuseViewModel viewModel;
                var laststatus = EnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new")) viewModel = new DualExtrimiteSertisseuseViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule);
                else if (obj.Equals("surr") && SerieSelected != null) viewModel = new DualExtrimiteSertisseuseViewModel(EnregManager.GetSerie(SerieSelected.NoSerie));                 else
                {
                    MessageBox.Show("Selectionner une serie!!!!");
                    return;
                }

                var view = new DualExtrimiteSertisseuse
                {
                    DataContext = viewModel
                };

                viewModel.RequestClose = () => view.Close();

                view.ShowDialog();

                viewModel.ExitLoops = true;
                ReloadAllSeries();
                ReloadFilter();
            }
        }
        public void ReloadAllSeries()
        {
            AllSeries = EnregCompletManager.GetAll("NF");
        }
        public void ReloadFilter()
        {
            FiltredSeries.Clear();

            if (string.IsNullOrEmpty(NMatricule))
            {
                foreach (var item in AllSeries) 
                {
                    FiltredSeries.Add(item);
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
                            FiltredSeries.Add(item);
                        }
                        else if (item.NoMachine.Contains(NMachine.ToUpper())) FiltredSeries.Add(item);
                    }
                }
            }

            OperateurLabel = !OperateurManager.CheckOpExist(NMatricule)? "" :OperateurManager.GetOpName(NMatricule.ToUpper());
            MachineLabel = string.IsNullOrEmpty(NMachine) ? "" : MachineManager.GetLibelle(NMachine);
        }

        public string GenerateNSerie()
        {
            var lSerie = EnregManager.GetLastSerie();
            int n = int.Parse(lSerie.Substring(2, lSerie.Length-2)) + 1;
            return "SN"+n.ToString("D4");
        }
        private void OnPropertyChanger(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
