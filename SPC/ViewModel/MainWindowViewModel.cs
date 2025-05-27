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
        public ObservableCollection<SPCEnregComplet> FiltredSeries { get; set; }
        public ObservableCollection<SPCEnregComplet> AllSeries { get; set; }
        private SPCEnregComplet serieSelected;

        public SPCEnregComplet SerieSelected
        {
            get { return serieSelected; }
            set {
                if ( serieSelected != value) 
                {
                    serieSelected = value;
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
                OutilFilter = "";
                InfoBar.RestartTimer();
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
                InfoBar.RestartTimer();
                OutilFilter = "";
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
                if (string.IsNullOrEmpty(value))
                {
                    machineLabel = "Machine:";
                    OnPropertyChanger(nameof(MachineLabel));
                }
                else if (value != machineLabel)
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
                if (string.IsNullOrEmpty(value))
                {
                    opLabel = "Matricule:";
                    OnPropertyChanger(nameof(OperateurLabel));
                }
                else if (value != opLabel)
                {
                    opLabel = value;
                    OnPropertyChanger(nameof(OperateurLabel));
                }
            }
        }
        private Outil selectedOutil;
        public Outil SelectedOutil
        {
            get { return selectedOutil; }
            set 
            {
                selectedOutil = value;
                if(SelectedOutil == null)
                {
                    VisiOutilInfor = "Hidden";
                    OnPropertyChanger(nameof(VisiOutilInfor));
                }
                else
                {
                    VisiOutilInfor = "Visible";
                    OnPropertyChanger(nameof(VisiOutilInfor));
                }
                    OnPropertyChanger(nameof(SelectedOutil));
            }
        }
        public string VisiOutilInfor { get; set; }

        private string outilFilter;

        public string OutilFilter
        {
            get { return outilFilter; }
            set 
            {
                outilFilter = value; 
                ReloadOutils();
                OnPropertyChanger(nameof(OutilFilter));
            }
        }

        public ObservableCollection<Outil> FiltredOutils { get; set; }
        public string VisiOutilGrid { get; set; }
        public InfoBarHelper InfoBar { get; set; }

        public DispatcherTimer _timerDispatcher { get; set; }
        public DispatcherTimer _reloadTimer { get; set; }

        public MainWindowViewModel()
        {
            AllSeries = SPCEnregCompletManager.GetAll("NF");
            FiltredSeries = new ObservableCollection<SPCEnregComplet>();
            ReloadFilter();
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

            _reloadTimer = new DispatcherTimer();
            _reloadTimer.Interval = TimeSpan.FromMinutes(1);
            _reloadTimer.Tick += (s, e) =>
            {
                ReloadAllSeries();
            };
            _reloadTimer.Start();

            var resetOutilView = new ResetOutil();
            resetOutilView.Show();
            InfoBar = new InfoBarHelper();

            FiltredOutils = new ObservableCollection<Outil>();

            VisiOutilGrid = "hidden";
            VisiOutilInfor = "Hidden";
        }

        private void OpenHistory(object obj)
        {
            InfoBar.RestartTimer();
            var historyView = new SerieHistory
            {
                DataContext = new HistoryViewModel()
            };
            historyView.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OpenNewSeries(object obj)
        {
            InfoBar.RestartTimer();
            if (string.IsNullOrEmpty(NMachine) || string.IsNullOrEmpty(MachineManager.GetTypeSPC(NMachine))) MessageBox.Show("verifier le machine");
            else if (string.IsNullOrEmpty(NMatricule) || !OperateurManager.CheckOpExist(NMatricule)) MessageBox.Show("verifier le matricule");
            else if (string.Equals(MachineManager.GetTypeSPC(NMachine), "CRIP1"))
            {
                MonoExtrimiteViewModel viewModel;
                var laststatus = SPCEnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new")) viewModel = new MonoExtrimiteViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule);
                else if (obj.Equals("surr") && SerieSelected != null) viewModel = new MonoExtrimiteViewModel(SPCEnregManager.GetSerie(SerieSelected.NoSerie)); //if there is no prev one it would arise error solve this
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
                var laststatus = SPCEnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new")) viewModel = new DualExtrimiteSertisseuseViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule);
                else if (obj.Equals("surr") && SerieSelected != null) viewModel = new DualExtrimiteSertisseuseViewModel(SPCEnregManager.GetSerie(SerieSelected.NoSerie));                 else
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
            else if (string.Equals(MachineManager.GetTypeSPC(NMachine), "CRIP3"))
            {

                TripleExtrimiteSetisseuseViewModel viewModel;
                var laststatus = SPCEnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new")) viewModel = new TripleExtrimiteSetisseuseViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule);
                else if (obj.Equals("surr") && SerieSelected != null) viewModel = new TripleExtrimiteSetisseuseViewModel(SPCEnregManager.GetSerie(SerieSelected.NoSerie));                 else
                {
                    MessageBox.Show("Selectionner une serie!!!!");
                    return;
                }

                var view = new TripleExtrimiteSertisseuse
                {
                    DataContext = viewModel
                };

                viewModel.RequestClose = () => view.Close();

                view.ShowDialog();

                viewModel.ExitLoops = true;
                ReloadAllSeries();
                ReloadFilter();
            }
            else if (string.Equals(MachineManager.GetTypeSPC(NMachine), "DENUD"))
            {
                DenudeuseViewModel viewModel;
                var laststatus = SPCEnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new")) viewModel = new DenudeuseViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule);
                else if (obj.Equals("surr") && SerieSelected != null) viewModel = new DenudeuseViewModel(SPCEnregManager.GetSerie(SerieSelected.NoSerie)); //if there is no prev one it would arise error solve this
                else
                {
                    MessageBox.Show("Selectionner une serie!!!!");
                    return;
                }

                var view = new DenudeuseView
                {
                    DataContext = viewModel
                };

                viewModel.RequestClose = () => view.Close();

                view.ShowDialog();

                viewModel.ExitLoops = true;
                ReloadAllSeries();
                ReloadFilter();
            }
            else if (string.Equals(MachineManager.GetTypeSPC(NMachine), "CCD"))
            {
                CoupeCableViewModel viewModel;
                var laststatus = SPCEnregCompletManager.LastSerieByOpStatus(NMatricule);

                if (obj.Equals("new")) viewModel = new CoupeCableViewModel(NMachine.ToUpper(), GenerateNSerie(), NMatricule);
                else if (obj.Equals("surr") && SerieSelected != null) viewModel = new CoupeCableViewModel(SPCEnregManager.GetSerie(SerieSelected.NoSerie), FiltredSeries.Where(s=> SerieSelected.NoSerie.Equals(s.NoSerie)).All(s=> !string.IsNullOrEmpty(s.Marquage))); // last is checking if there was a serie without marquage to hide it 
                else
                {
                    MessageBox.Show("Selectionner une serie!!!!");
                    return;
                }

                var view = new CoupeCableView
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
            AllSeries = SPCEnregCompletManager.GetAll("NF");
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
        private void ReloadOutils()
        {
            FiltredOutils.Clear();
            var outils = OutilManager.GetOutilsByCndO("", OutilFilter).Count() > 0 ? OutilManager.GetOutilsByCndO("", OutilFilter) : OutilManager.GetOutilsByCndO(OutilFilter, "");
            foreach (var item in outils)
            {
                FiltredOutils.Add(item);
            }
            if (FiltredOutils.Count() > 0)
                VisiOutilGrid = "Visible";
            else
            {
                VisiOutilGrid = "Hidden";
                selectedOutil = null;
            }
            OnPropertyChanger(nameof(VisiOutilGrid));
        }

        public string GenerateNSerie()
        {
            var lSerie = SPCEnregManager.GetLastSerie();
            int n = int.Parse(lSerie.Substring(2, lSerie.Length-2)) + 1;
            return "SN"+n.ToString("D4");
        }
        private void OnPropertyChanger(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
