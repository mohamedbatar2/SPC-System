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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SPC.Views
{
    /// <summary>
    /// Interaction logic for MonoExtrimite.xaml
    /// </summary>
    public partial class MonoExtrimite : Window
    {
        public MonoExtrimite()
        {
            InitializeComponent();
            this.Language = XmlLanguage.GetLanguage("fr-FR");
        }
    }
}
