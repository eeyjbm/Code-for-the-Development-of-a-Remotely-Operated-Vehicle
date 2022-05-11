using ROV_Interface.Commands;
using ROV_Interface.Models;
using ROV_Interface.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ROV_Interface.ViewModels.SettingsViewModels
{
    public class SettingsPanelSubViewModel1 : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly SettingsStore _settingsStore;

        SettingsDataModel SettingsData = new SettingsDataModel();

        public ViewModelBase CurrentSettingsViewModel => _navigationStore.CurrentSettingsViewModel;
        public ICommand SettingsMainViewCommand { get; }
        public ICommand SettingsSubView2Command { get; }
        public RelayCommand SyncCommand { get; private set; }

        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }


        private int gear1;
        public int Gear1
        {
            get
            {
                return gear1;
            }
            set
            {
                gear1 = value;
                if (gear1 > 100)
                    gear1 = 100;
                if (gear1 < 0)
                    gear1 = 0;
                OnPropertyChanged("Gear1");
            }
        }
        private int gear2;
        public int Gear2
        {
            get
            {
                return gear2;
            }
            set
            {
                gear2 = value;
                if (gear2 > 100)
                    gear2 = 100;
                if (gear2 < 0)
                    gear2 = 0;
                OnPropertyChanged("Gear2");
            }
        }
        private int gear3;
        public int Gear3
        {
            get
            {
                return gear3;
            }
            set
            {
                gear3 = value;
                if (gear3 > 100)
                    gear3 = 100;
                if (gear3 < 0)
                    gear3 = 0;
                OnPropertyChanged("Gear3");
            }
        }

        private int maxThrusterChange;
        public int MaxThrusterChange
        {
            get
            {
                return maxThrusterChange;
            }
            set
            {
                maxThrusterChange = value;
                if (maxThrusterChange > 100)
                    maxThrusterChange = 100;
                if (maxThrusterChange < 0)
                    maxThrusterChange = 0;

                OnPropertyChanged("MaxThrusterChange");
            }
        }

        

        private int fluidDensity;
        public int FluidDensity
        {
            get
            {
                return fluidDensity;
            }
            set
            {
                fluidDensity = value;
                if (fluidDensity > 9999)
                    fluidDensity = 9999;
                if (fluidDensity < 0)
                    fluidDensity = 0;

                OnPropertyChanged("FluidDensity");
            }
        }

        

        private int recoveryTimeDelay;
        public int RecoveryTimeDelay
        {
            get
            {
                return recoveryTimeDelay;
            }
            set
            {
                recoveryTimeDelay = value;
                if (recoveryTimeDelay > 9)
                    recoveryTimeDelay = 9;
                if (recoveryTimeDelay < 0)
                    recoveryTimeDelay = 0;

                OnPropertyChanged("RecoveryTimeDelay");
            }
        }

        


        private int recoveryThrustorPower;
        public int RecoveryThrustorPower
        {
            get
            {
                return recoveryThrustorPower;
            }
            set
            {
                recoveryThrustorPower = value;
                if (recoveryThrustorPower > 100)
                    recoveryThrustorPower = 100;
                if (recoveryThrustorPower < 0)
                    recoveryThrustorPower = 0;

                OnPropertyChanged("RecoveryThrustorPower");
            }
        }
        public SettingsPanelSubViewModel1(NavigationStore navigationStore, SettingsStore settingsStore) 
        {
            this._navigationStore = navigationStore;
            this._settingsStore = settingsStore;
            _navigationStore.CurrentSettingsViewModelChanged += _navigationStore_CurrentSettingsViewModelChanged;

            SettingsMainViewCommand = new SettingsNavigationCommand<SettingsPanelMainViewModel>(navigationStore, () => new SettingsPanelMainViewModel(navigationStore, settingsStore)); ;
            SettingsSubView2Command = new SettingsNavigationCommand<SettingsPanelSubViewModel2>(navigationStore, () => new SettingsPanelSubViewModel2(navigationStore, settingsStore)); ;

            SyncCommand = new RelayCommand(SyncCommandCall); // commands

            RefreshCommand = new RelayCommand(RefreshCommandCall); // commands

            SaveCommand = new RelayCommand(SaveCommandCall); // commands

            Gear1 = Properties.Settings.Default.Gear1;
            Gear2 = Properties.Settings.Default.Gear2;
            Gear3 = Properties.Settings.Default.Gear3;
            MaxThrusterChange = Properties.Settings.Default.MaxThrusterChange;
            FluidDensity = Properties.Settings.Default.FluidDensity;
            RecoveryTimeDelay = Properties.Settings.Default.RecoveryTimeDelay;
            RecoveryThrustorPower = Properties.Settings.Default.RecoveryThrustorPower;
        }

        public void RefreshCommandCall(object message)
        {
            Gear1 =Properties.Settings.Default.Gear1;
            Gear2 = Properties.Settings.Default.Gear2;
            Gear3 = Properties.Settings.Default.Gear3;
            MaxThrusterChange = Properties.Settings.Default.MaxThrusterChange;
            FluidDensity = Properties.Settings.Default.FluidDensity;
            RecoveryTimeDelay = Properties.Settings.Default.RecoveryTimeDelay;
            RecoveryThrustorPower = Properties.Settings.Default.RecoveryThrustorPower;

            Properties.Settings.Default.Reload();

        }
        public void SaveCommandCall(object message)
        {
            Properties.Settings.Default.Gear1 = Gear1;
            Properties.Settings.Default.Gear2 = Gear2;
            Properties.Settings.Default.Gear3 = Gear3;
            Properties.Settings.Default.MaxThrusterChange = MaxThrusterChange;
            Properties.Settings.Default.FluidDensity = FluidDensity;
            Properties.Settings.Default.RecoveryTimeDelay = RecoveryTimeDelay;
            Properties.Settings.Default.RecoveryThrustorPower = RecoveryThrustorPower;
            Properties.Settings.Default.Save();
        }

        public void SyncCommandCall(object message)
        {
            SettingsData.SetSettingsModel();
            _settingsStore.SendSettingsData(SettingsData);
            // Properties.Settings.Default.MaxThrusterChange;
        }
        private void _navigationStore_CurrentSettingsViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentSettingsViewModel));
        }
    }
}
