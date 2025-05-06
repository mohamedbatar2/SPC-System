using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using SPC.Tools;

namespace SPC.ViewModel
{
    public class LaunchFormViewModel
    {
        public string UAP { get; set; }
        public string Operateur { get; set; }
        public string NOperateur { get; set; }

        public Action RequestExit { get; set; }
        public Action<string[]> PassData { get; set; }
        public ICommand TerminerCommand{ get; set; }

        public LaunchFormViewModel()
        {
            TerminerCommand = new RelayCommand(Terminer, parm => true);
        }

        private void Terminer(object obj)
        {
            if (!string.IsNullOrEmpty(UAP) && !string.IsNullOrEmpty(Operateur) && !string.IsNullOrEmpty(NOperateur))
            {
                if (!UAPFormat())
                {
                    MessageBox.Show("S'il vous plait, gardez ce format pour UAP : UAP-(num)");
                    return;
                }
                // send the props back to mainviewmodel
                PassData?.Invoke(new string[] { UAP, Operateur, NOperateur });
                RequestExit?.Invoke();
            }
            else MessageBox.Show("Remplir Toutes les cases");
        }
        private bool UAPFormat()
        {
            if (UAP.Length != 5) return false;
            string firstHalf = UAP.Substring(0, 4);
            if (firstHalf.Equals("UAP-") && int.TryParse(UAP[UAP.Length - 1].ToString(), out int num)) return true;
            else return false;
        }
    }
    
}
