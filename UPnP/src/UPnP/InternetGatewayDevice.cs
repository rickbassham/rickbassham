using System;
using System.Collections.Generic;
using System.Text;

namespace UPnP
{
    public class InternetGatewayDevice : Device
    {
        internal InternetGatewayDevice(UPNPLib.IUPnPDevice device)
        {
            _device = device;

            foreach (UPNPLib.IUPnPService service in _device.Services)
            {
                switch(service.ServiceTypeIdentifier)
                {
                    case URN.WanIpConnection:
                        _wanIpConnection = new WanIpConnectionService(service);
                        break;
                }
            }
        }

        private WanIpConnectionService _wanIpConnection;
        public WanIpConnectionService WanIpConnection
        {
            get
            {
                return _wanIpConnection;
            }
        }
    }
}
