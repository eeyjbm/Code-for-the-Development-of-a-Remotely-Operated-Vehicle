using ROV_Interface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Stores
{
    public class NavigationStore
    {
        public event Action CurrentGraphViewModelChanged;
        public event Action CurrentSettingsViewModelChanged;
        public event Action CurrentTelemetryViewModelChanged;

        private ViewModelBase _currentGraphViewModel;
        public ViewModelBase CurrentGraphViewModel
        {
            get => _currentGraphViewModel;
            set
            {
                _currentGraphViewModel = value;
                OnCurrentGraphViewModelChanged();
            }
        }
        private void OnCurrentGraphViewModelChanged()
        {
            CurrentGraphViewModelChanged?.Invoke();
        }

        private ViewModelBase _currentSettingsViewModel;
        public ViewModelBase CurrentSettingsViewModel
        {
            get => _currentSettingsViewModel;
            set
            {
                _currentSettingsViewModel = value;
                OnCurrentSettingsViewModelChanged();
            }
        }
        private void OnCurrentSettingsViewModelChanged()
        {
            CurrentSettingsViewModelChanged?.Invoke();
        }

        private ViewModelBase _currentTelemetryViewModel;
        public ViewModelBase CurrentTelemetryViewModel
        {
            get => _currentTelemetryViewModel;
            set
            {
                _currentTelemetryViewModel = value;
                OnCurrentTelemetryViewModelChanged();
            }
        }

        private void OnCurrentTelemetryViewModelChanged()
        {
            CurrentTelemetryViewModelChanged?.Invoke();
        }
    }
}
