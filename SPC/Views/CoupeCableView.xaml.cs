using System.Windows;
using SPC.ViewModel;

namespace SPC.Views
{
    public partial class CoupeCableView : Window
    {
        public CoupeCableView()
        {
            InitializeComponent();

            // Abonnement pour fermer la fenêtre quand RequestClose est invoqué
            if (DataContext is CoupeCableViewModel viewModel)
            {
                viewModel.RequestClose = () => this.Close();
            }

            // Alternative: gérer via Loaded event
            this.Loaded += CoupeCableView_Loaded;
        }

        private void CoupeCableView_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is CoupeCableViewModel viewModel)
            {
                viewModel.RequestClose = () => this.Close();
            }
        }
    }
}
