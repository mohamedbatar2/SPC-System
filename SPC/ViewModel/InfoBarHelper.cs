using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SPC.ViewModel
{
    public class InfoBarHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string date;
        public string Date
        {
            get { return date; }
            set { 
                date = value;
                OnPropertyChange(nameof(Date));
            }
        }
        private int timer;
        public int Timer
        {
            get { return timer; }
            set {
                timer = value;
                OnPropertyChange(nameof(Timer));
            }
        }

        private string idPc;
        public string IdPc
        {
            get { return idPc; }
            set {
                idPc = value;
                OnPropertyChange(nameof(IdPc));
            }
        }

        private string session;

        public string Session
        {
            get { return session; }
            set {
                session = value;
                OnPropertyChange(nameof(Session));
            }
        }
        public DispatcherTimer _closingTimer { get; set; }

        public InfoBarHelper()
        {
            Date = "05/23/2025";
            Session = Environment.UserName;
            IdPc = Environment.MachineName;
            Timer = 1000;
            _closingTimer = new DispatcherTimer();
            _closingTimer.Interval = TimeSpan.FromSeconds(1);
            _closingTimer.Tick += (e, s) =>
            {
                Timer--;
                if(Timer == 0)
                    Application.Current.Shutdown();
            };
            _closingTimer.Start();
        }

        public void RestartTimer()
        {
            _closingTimer.Stop();
            Timer = 1000;
            _closingTimer.Start();
        }
        private void OnPropertyChange(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
