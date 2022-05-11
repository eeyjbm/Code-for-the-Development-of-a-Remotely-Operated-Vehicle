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
    public class SettingsPanelSubViewModel2 : ViewModelBase
    {

        private readonly NavigationStore _navigationStore;
        private readonly SettingsStore _settingsStore;

        public ViewModelBase CurrentSettingsViewModel => _navigationStore.CurrentSettingsViewModel;
        public ICommand SettingsMainViewCommand { get; }
        public ICommand SettingsSubView1Command { get; }
        public RelayCommand SyncCommand { get; private set; }
        public RelayCommand RefreshCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        SettingsDataModel SettingsData = new SettingsDataModel();


        private double pTerm;
        public double PTerm
        {
            get
            {
                return pTerm;
            }
            set
            {
                pTerm = value;
                if (pTerm > 9.999)
                    pTerm = 9.999;
                if (pTerm < 0)
                    pTerm = 0;
                OnPropertyChanged("PTerm");
            }
        }

        private double dTerm;
        public double DTerm
        {
            get
            {
                return dTerm;
            }
            set
            {
                dTerm = value;
                if (dTerm > 9.999)
                    dTerm = 9.999;
                if (dTerm < 0)
                    dTerm = 0;
                OnPropertyChanged("DTerm");
            }
        }
        private double iTerm;
        public double ITerm
        {
            get
            {
                return iTerm;
            }
            set
            {
                iTerm = value;
                if (iTerm > 9.999)
                    iTerm = 9.999;
                if (iTerm < 0)
                    iTerm = 0;
                OnPropertyChanged("ITerm");
            }
        }
        public SettingsPanelSubViewModel2(NavigationStore navigationStore, SettingsStore settingsStore)
        {
            this._navigationStore = navigationStore;
            this._settingsStore = settingsStore;

            _navigationStore.CurrentSettingsViewModelChanged += _navigationStore_CurrentSettingsViewModelChanged;

            SettingsMainViewCommand = new SettingsNavigationCommand<SettingsPanelMainViewModel>(navigationStore, () => new SettingsPanelMainViewModel(navigationStore, settingsStore)); ;
            SettingsSubView1Command = new SettingsNavigationCommand<SettingsPanelSubViewModel1>(navigationStore, () => new SettingsPanelSubViewModel1(navigationStore, settingsStore)); ;

            SyncCommand = new RelayCommand(SyncCommandCall); // commands
            RefreshCommand = new RelayCommand(RefreshCommandCall); // commands

            SaveCommand = new RelayCommand(SaveCommandCall); // commands

            PTerm = Properties.Settings.Default.PTerm;
            DTerm = Properties.Settings.Default.DTerm;
            ITerm = Properties.Settings.Default.ITerm;

        }



        public void RefreshCommandCall(object message)
        {
            PTerm = Properties.Settings.Default.PTerm;
            DTerm = Properties.Settings.Default.DTerm;
            ITerm = Properties.Settings.Default.ITerm;
            Properties.Settings.Default.Reload();

        }
        public void SaveCommandCall(object message)
        {
            Properties.Settings.Default.PTerm = PTerm;
            Properties.Settings.Default.DTerm = DTerm;
            Properties.Settings.Default.ITerm = ITerm;
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
