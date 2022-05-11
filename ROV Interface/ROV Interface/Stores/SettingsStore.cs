using ROV_Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Stores
{
    public class SettingsStore
    {
        public event Action<SettingsDataModel> SendSettingsDataEvent;
        public SettingsStore()
        { 
        
        }

        public void SendSettingsData(SettingsDataModel Value)
        {
            SendSettingsDataEvent?.Invoke(Value);
        }

    }
}
