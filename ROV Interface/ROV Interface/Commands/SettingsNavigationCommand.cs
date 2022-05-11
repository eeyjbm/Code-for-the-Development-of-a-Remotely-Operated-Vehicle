using ROV_Interface.Stores;
using ROV_Interface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Commands
{
    class SettingsNavigationCommand<TViewModel> : CommandBase
         where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _currentSettingsViewModel;

        public SettingsNavigationCommand(NavigationStore navigationStore, Func<TViewModel> currentSettingsViewModel)
        {
            _navigationStore = navigationStore;
            _currentSettingsViewModel = currentSettingsViewModel;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentSettingsViewModel = _currentSettingsViewModel();
        }
    }
}
