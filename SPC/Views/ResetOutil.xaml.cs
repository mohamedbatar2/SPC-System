using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SPC.ViewModel;

namespace SPC.Views
{
    /// <summary>
    /// Interaction logic for ResetOutil.xaml
    /// </summary>
    public partial class ResetOutil : Window
    {
        public ResetOutil()
        {
            InitializeComponent();

            var vm = new ResetOutilViewModel();
            this.DataContext = vm;
        }
    }
}
