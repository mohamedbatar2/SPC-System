using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SPC.Models;

namespace SPC.ViewModel
{
    public class HistoryViewModel
    {
        public ObservableCollection<SPCEnregComplet> AllSeries { get; set; }
        public List<string> DateItems { get; set; }
        private string selectedDate;

        public string SelectedDate
        {
            get { return selectedDate; }
            set { selectedDate = value;
                var sp = value.Split('/');
                month = sp[0].Trim('0');
                year = sp[1];
                ReloadData();
            }
        }
        private string year, month;

        public event PropertyChangedEventHandler PropertyChanged;

        public HistoryViewModel()
        {
            DateItems = new List<string>();
            year = DateTime.Now.Year.ToString();
            month = DateTime.Now.Month.ToString();
            AllSeries = SPCEnregCompletManager.GetAll("F", month, year);
            for (int y = int.Parse(year); y > 2015; y--)
            {
                if(y == int.Parse(year))
                {
                    for (int m = int.Parse(month); m>0 ; m--)
                    { 
                        DateItems.Add($"{m:D2}/{y}");
                    }
                }
                else
                {
                    for (int m = 12; m > 0; m--)
                    {
                        DateItems.Add($"{m:D2}/{y}");
                    }
                }
            }
        }
        private void ReloadData()
        {
            
            AllSeries.Clear();
            foreach (var item in SPCEnregCompletManager.GetAll("F", month, year))
            {
                AllSeries.Add(item);
            }
        }
    }
}
