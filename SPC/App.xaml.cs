using System;
using System.Windows;
using SPC.Services;

namespace SPC
{
    public partial class App : Application
    {
        public static ApiService ApiService { get; private set; }
        public static AuthenticationService AuthService { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                ApiService = new ApiService("https://localhost:7191");
                AuthService = new AuthenticationService(ApiService);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to initialize API services: {ex.Message}");
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ApiService?.Dispose();
            base.OnExit(e);
        }
    }
}
