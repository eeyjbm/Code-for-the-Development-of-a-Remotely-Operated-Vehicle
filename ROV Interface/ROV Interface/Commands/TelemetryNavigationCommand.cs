using ROV_Interface.Stores;
using ROV_Interface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Commands
{
    class TelemetryNavigationCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _currentTelemetryViewModel;

        public TelemetryNavigationCommand(NavigationStore navigationStore, Func<TViewModel> currentTelemetryViewModel)
        {
            _navigationStore = navigationStore;
            _currentTelemetryViewModel = currentTelemetryViewModel;
        }
        public override void Execute(object parameter)
        {
            _navigationStore.CurrentTelemetryViewModel = _currentTelemetryViewModel();
        }
    }
}
