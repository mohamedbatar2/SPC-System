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
                month = sp[0];
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
            for (int j = int.Parse(year); j > 2015; j--)
            {
                if(j == int.Parse(year))
                {
                    for (int i = int.Parse(month); i>0 ; i--)
                    { 
                        DateItems.Add($"{i:D2}/{j}");
                    }
                }
                else
                {
                    for (int i = 12; i > 0; i--)
                    {
                        DateItems.Add($"{i:D2}/{j}");
                    }
                }
            }
        }
        private void ReloadData()
        {
            
            AllSeries.Clear();
            foreach (var item in SPCEnregCompletManager.GetAll("F", month, year))
            {
                if(month == "05")
                AllSeries.Add(item);
            }
        }
    }
}
