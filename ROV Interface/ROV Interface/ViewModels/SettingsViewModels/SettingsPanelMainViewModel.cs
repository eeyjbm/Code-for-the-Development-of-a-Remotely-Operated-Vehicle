using ROV_Interface.Commands;
using ROV_Interface.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ROV_Interface.ViewModels.SettingsViewModels
{
    public class SettingsPanelMainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly SettingsStore _settingsStore;

        public ViewModelBase CurrentSettingsViewModel => _navigationStore.CurrentSettingsViewModel;
        public ICommand SettingsSubView1Command { get; }
        public ICommand SettingsSubView2Command { get; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        private string _ip;
        public string IP
        {
            get
            {
                return _ip;
            }
            set
            {
                _ip = value;
                OnPropertyChanged("IP");
            }
        }

        private int port;
        public int Port
        {
            get
            {
                return port;
            }
            set
            {
                port = value;
                OnPropertyChanged("Port");
            }
        }

        
                    private int streamPort;
        public int StreamPort
        {
            get
            {
                return streamPort;
            }
            set
            {
                streamPort = value;
                OnPropertyChanged("StreamPort");
            }
        }

        private double alertVoltage;
        public double AlertVoltage
        {
            get
            {
                return alertVoltage;
            }
            set
            {
                alertVoltage = value;
                if (alertVoltage < 0)
                    alertVoltage = 0;
                OnPropertyChanged("AlertVoltage");
            }
        }
        public SettingsPanelMainViewModel(NavigationStore navigationStore, SettingsStore settingsStore)
        {
            this._navigationStore = navigationStore;
            this._settingsStore = settingsStore;
            _navigationStore.CurrentSettingsViewModelChanged += _navigationStore_CurrentSettingsViewModelChanged;

            SettingsSubView1Command = new SettingsNavigationCommand<SettingsPanelSubViewModel1>(navigationStore, () => new SettingsPanelSubViewModel1(navigationStore, settingsStore)); ;
            SettingsSubView2Command = new SettingsNavigationCommand<SettingsPanelSubViewModel2>(navigationStore, () => new SettingsPanelSubViewModel2(navigationStore, settingsStore)); ;

            RefreshCommand = new RelayCommand(RefreshCommandCall); // commands

            SaveCommand = new RelayCommand(SaveCommandCall); // commands

            IP = Properties.Settings.Default.ServerIP;
            Port = Properties.Settings.Default.ServerPort;
            AlertVoltage = Properties.Settings.Default.AlertVoltage;
            streamPort = Properties.Settings.Default.StreamPort;

        }
        public void RefreshCommandCall(object message)
        {
            IP = Properties.Settings.Default.ServerIP;
            Port = Properties.Settings.Default.ServerPort;
            AlertVoltage = Properties.Settings.Default.AlertVoltage;
            streamPort = Properties.Settings.Default.StreamPort;
            Properties.Settings.Default.Reload();

        }
        public void SaveCommandCall(object message)
        {
            Properties.Settings.Default.ServerIP = IP;
            Properties.Settings.Default.ServerPort = Port;
            Properties.Settings.Default.AlertVoltage = AlertVoltage;
            Properties.Settings.Default.StreamPort = streamPort;

            Properties.Settings.Default.Save();
        }

        private void _navigationStore_CurrentSettingsViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentSettingsViewModel));
        }
    }
}
