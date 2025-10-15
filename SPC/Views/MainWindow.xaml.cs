using System;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;
using SPC.Services;
using SPC.ViewModel;

namespace SPC
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set French culture
            this.Language = XmlLanguage.GetLanguage("fr-FR");

            // Initialize ViewModel with data service
            ISpcDataService dataService = new AccessDataService();
            MainWindowViewModel viewModel = new MainWindowViewModel(dataService);
            this.DataContext = viewModel;
        }
    }
}
