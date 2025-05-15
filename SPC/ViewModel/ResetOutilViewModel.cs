using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SPC.Models;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class ResetOutilViewModel
    {
        public ICommand ResetCommand { get; set; }
        public string NOutil { get; set; }


        public ResetOutilViewModel()
        {
            ResetCommand = new RelayCommand(Reset , parm=>true);
        }

        private void Reset(object obj)
        {
            MessageBox.Show("start");
            MessageBox.Show(NOutil);
            if (NOutil == null) 
                MessageBox.Show("not found");
            else if (OtaPrvntfManager.GetPrvntf(NOutil) != null)
            {
                OtaPrvntfManager.ResetOtaPrvntf(NOutil);
                MessageBox.Show("found and reset");
            }
        }
    }
}
