using ROV_Interface.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Stores
{
    public class RemoteStateStore
    {
        public event Action<RemoteData> SendRemoteDataEvent;
        public event Action<bool> RemoteConnectedEvent;

        public RemoteStateStore() 
        {}

        public void SendRemoteData(RemoteData Value)
        {
            SendRemoteDataEvent?.Invoke(Value);
        }

        public void RemoteConnected(bool connected)
        {
            RemoteConnectedEvent?.Invoke(connected);
        }
    }
}
