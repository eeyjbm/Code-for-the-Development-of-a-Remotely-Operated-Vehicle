using ROV_Interface.Stores;
using ROV_Interface.ViewModels;
using ROV_Interface.ViewModels.TelemertyViewModels;
using ROV_Interface.ViewModels.SettingsViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ROV_Interface
{
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;
        private readonly RemoteStateStore _remoteStateStore;
        private readonly TelemetryStateStore _telemetryStateStore; 
        private readonly SettingsStore _settingsStore;
        private readonly TCPClientStore _tCPClientStore;
        private MainViewModel _mainViewModel;
        public App()
        {
            _navigationStore = new NavigationStore();
            _remoteStateStore = new RemoteStateStore();
            _telemetryStateStore = new TelemetryStateStore();
            _settingsStore = new SettingsStore();
            _tCPClientStore = new TCPClientStore();
            _mainViewModel = new MainViewModel(_navigationStore, _remoteStateStore, _telemetryStateStore, _settingsStore, _tCPClientStore);
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            _navigationStore.CurrentTelemetryViewModel = new TelemetryPanelMainViewModel(_remoteStateStore, _telemetryStateStore);
            _navigationStore.CurrentSettingsViewModel = new SettingsPanelSubViewModel1(_navigationStore, _settingsStore);

            MainWindow = new MainWindow()
            {
                DataContext = _mainViewModel
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _mainViewModel._tCPClientService.DisconnectFromPI();
        }

    }
}
