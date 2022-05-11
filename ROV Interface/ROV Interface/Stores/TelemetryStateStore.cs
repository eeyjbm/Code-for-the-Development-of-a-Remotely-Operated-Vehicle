using ROV_Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Stores
{
   public  class TelemetryStateStore
    {
        public event Action<TelemetryDataModel> SendTelemetryDataEvent;
        public event Action<TelemetryDataModel> SendTelemetryAndBatteryDataEvent;

        public TelemetryStateStore()
        { }

        public void SendTelemetryData(TelemetryDataModel Value)
        {
            SendTelemetryDataEvent?.Invoke(Value);
        }

        public void SendTelemetryAndBatteryData(TelemetryDataModel Value)
        {
            SendTelemetryAndBatteryDataEvent?.Invoke(Value);
        }
    }
}
