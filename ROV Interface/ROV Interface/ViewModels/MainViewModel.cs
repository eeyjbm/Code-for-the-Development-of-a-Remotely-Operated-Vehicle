using ROV_Interface.Commands;
using ROV_Interface.Services;
using ROV_Interface.Stores;
using ROV_Interface.ViewModels.TelemertyViewModels;
using ROV_Interface.ViewModels.SettingsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ROV_Interface.Models;

namespace ROV_Interface.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly RemoteStateStore _remoteStateStore;
        private readonly TelemetryStateStore _telemetryStateStore;
        private readonly SettingsStore _settingsStore;
        private readonly TCPClientStore _tCPClientStore;


        public RemoteService _remoteService;
        public TCPClientService _tCPClientService;
        double AlertVoltage;
        public ViewModelBase CurrentTelemetryViewModel => _navigationStore.CurrentTelemetryViewModel;
        public ViewModelBase CurrentSettingsViewModel => _navigationStore.CurrentSettingsViewModel;
        public ViewModelBase CurrentGraphViewModel => _navigationStore.CurrentGraphViewModel;

        public ICommand TelemetrySubView1Command { get; }
        public ICommand TelemetryMainViewCommand { get; }
        public RelayCommand ConnectCommand { get; private set; }
        public RelayCommand TelemetryCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }


        SettingsDataModel SettingsData = new SettingsDataModel();


        private string telemetryPanelVisibility;
        public string TelemetryPanelVisibility
        {
            get
            {
                return telemetryPanelVisibility;
            }
            set
            {
                telemetryPanelVisibility = value;
                OnPropertyChanged("TelemetryPanelVisibility");
            }
        }

        private string settingsPanelVisibility;
        public string SettingsPanelVisibility
        {
            get
            {
                return settingsPanelVisibility;
            }
            set
            {
                settingsPanelVisibility = value;
                OnPropertyChanged("SettingsPanelVisibility");
            }
        }

        private string remoteConnectColour;
        public string RemoteConnectColour
        {
            get
            {
                return remoteConnectColour;
            }
            set
            {
                remoteConnectColour = value;
                OnPropertyChanged("RemoteConnectColour");
            }
        }

        private string _ROVConnectColour;
        public string ROVConnectColour
        {
            get
            {
                return _ROVConnectColour;
            }
            set
            {
                _ROVConnectColour = value;
                OnPropertyChanged("ROVConnectColour");
            }
        }

        private int gear;
        public int Gear
        {
            get
            {
                return gear;
            }
            set
            {
                gear = value;
                OnPropertyChanged("Gear");
            }
        }

        private string batteryStatus;
        public string BatteryStatus
        {
            get
            {
                return batteryStatus;
            }
            set
            {
                batteryStatus = value;
                OnPropertyChanged("BatteryStatus");
            }
        }

        
        public MainViewModel(NavigationStore navigationStore, RemoteStateStore remoteStateStore, TelemetryStateStore telemetryStateStore, SettingsStore settingsStore, TCPClientStore tCPClientStore)
        {
            this._navigationStore = navigationStore;
            this._remoteStateStore = remoteStateStore;
            this._telemetryStateStore = telemetryStateStore;
            this._settingsStore = settingsStore;
            this._tCPClientStore = tCPClientStore;

            _remoteService = new RemoteService(remoteStateStore);
            _tCPClientService = new TCPClientService(remoteStateStore, telemetryStateStore, settingsStore, tCPClientStore);

            ConnectCommand = new RelayCommand(ConnectCommandCall); // commands
            TelemetryCommand = new RelayCommand(TelemetryCommandCall); // commands
            SettingsCommand = new RelayCommand(SettingsCommandCall); // commands


            _navigationStore.CurrentGraphViewModelChanged += _navigationStore_CurrentGraphViewModelChanged;
            _navigationStore.CurrentSettingsViewModelChanged += _navigationStore_CurrentSettingsViewModelChanged;
            _navigationStore.CurrentTelemetryViewModelChanged += _navigationStore_CurrentTelemetryViewModelChanged;

            _remoteStateStore.RemoteConnectedEvent += _remoteStateStore_RemoteConnectedEvent1;
            _remoteStateStore.SendRemoteDataEvent += _remoteStateStore_SendRemoteDataEvent;

            _telemetryStateStore.SendTelemetryAndBatteryDataEvent += _telemetryStateStore_SendTelemetryAndBatteryDataEvent;

            _tCPClientStore.SendROVConnectedEvent += _tCPClientStore_SendROVConnectedEvent;

            Properties.Settings.Default.PropertyChanged += Default_PropertyChanged;

            SettingsData.SetSettingsModel();
            _settingsStore.SendSettingsData(SettingsData);
            // TelemetrySubView1Command = new TelemetryNavigationCommand<TelemetryPanelSubViewModel1>(navigationStore, () => new TelemetryPanelSubViewModel1()); ;
            // TelemetryMainViewCommand = new TelemetryNavigationCommand<TelemetryPanelMainViewModel>(navigationStore, () => new TelemetryPanelMainViewModel(remoteStateStore, telemetryStateStore)); ;

            AlertVoltage = Properties.Settings.Default.AlertVoltage;

            TelemetryPanelVisibility = "Visible";

            if(Properties.Settings.Default.ShowSettingsOnStartUp)
            SettingsPanelVisibility = "Visible";
            else
            SettingsPanelVisibility = "Hidden";

            if (Properties.Settings.Default.ShowTelemertyOnStartUp)
                TelemetryPanelVisibility = "Visible";
            else
            TelemetryPanelVisibility = "Hidden";

            BatteryStatus = "Hidden";

            ROVConnectColour = "Red";
            RemoteConnectColour = "Red";
            Gear = 0;
        }

        private void _tCPClientStore_SendROVConnectedEvent(bool obj)
        {
            if(!obj)
                ROVConnectColour = "Red";
            else
                ROVConnectColour = "Green";

        }

        private void Default_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AlertVoltage = Properties.Settings.Default.AlertVoltage;
        }

        private void _telemetryStateStore_SendTelemetryAndBatteryDataEvent(Models.TelemetryDataModel obj)
        {
            if(obj.Cell1 < AlertVoltage || obj.Cell2 < AlertVoltage || obj.Cell3 < AlertVoltage || obj.Cell4 < AlertVoltage || obj.Cell5 < AlertVoltage)
                BatteryStatus = "Visible";
            else
                BatteryStatus = "Hidden";

        }

        private void _remoteStateStore_SendRemoteDataEvent(Models.RemoteData obj)
        {
            if (obj.ARM)
                Gear = obj.Gear;
            else
                Gear = 0;

        }

        private void _remoteStateStore_RemoteConnectedEvent1(bool obj)
        {
            if (obj)
                RemoteConnectColour = "Green";
            else
            {
                RemoteConnectColour = "Red";
                Gear = 0;

            }
        }

        public void ConnectCommandCall(object message)
        {
            _remoteService.ConnectToRemote();
            if (_tCPClientService.ConnectToPI())
                ROVConnectColour = "Green";
            else
            {
                ROVConnectColour = "Red";
                Gear = 0;
            }
        }

        public void TelemetryCommandCall(object message)
        {
            if (TelemetryPanelVisibility == "Visible")
                TelemetryPanelVisibility = "Hidden";
            else
                TelemetryPanelVisibility = "Visible";
        }

        public void SettingsCommandCall(object message)
        {
            if (SettingsPanelVisibility == "Visible")
                SettingsPanelVisibility = "Hidden";
            else
                SettingsPanelVisibility = "Visible";
        }

        private void _navigationStore_CurrentTelemetryViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentTelemetryViewModel));
        }

        private void _navigationStore_CurrentSettingsViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentSettingsViewModel));
        }

        private void _navigationStore_CurrentGraphViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentGraphViewModel));
        }


    }
}
