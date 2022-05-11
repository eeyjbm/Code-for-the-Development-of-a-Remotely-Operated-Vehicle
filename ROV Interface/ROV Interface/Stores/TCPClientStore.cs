using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROV_Interface.Stores
{
    public class TCPClientStore
    {

        public event Action<bool> SendROVConnectedEvent;

        public TCPClientStore()
        { }
        public void SendROVConnected(bool Value)
        {
            SendROVConnectedEvent?.Invoke(Value);
        }
    }

}
