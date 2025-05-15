using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPC.Models;

namespace SPC.ViewModel
{
    public class HistoryViewModel
    {
        public ObservableCollection<EnregComplet> AllSeries { get; set; }
        public HistoryViewModel()
        {
            AllSeries = EnregCompletManager.GetAll("all");
        }
    }
}
