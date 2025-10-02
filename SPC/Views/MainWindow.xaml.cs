using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using SPC.Services;
using SPC.ViewModel;

namespace SPC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        
            this.Language = XmlLanguage.GetLanguage("fr-FR");

           
            ISpcDataService dataService = new AccessDataService();

            MainWindowViewModel viewModel = new MainWindowViewModel(dataService);
            this.DataContext = viewModel;
        }
    }
}
