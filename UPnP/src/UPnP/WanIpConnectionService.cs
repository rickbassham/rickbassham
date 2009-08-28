using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace UPnP
{
    public class WanIpConnectionService : Service
    {
        [Flags]
        public enum ConnectionTypes
        {
            Unknown,
            IP_Routed,
            IP_Bridged,
        }

        public enum ConnectionStatus
        {
            Unconfigured,
            Connecting,
            Connected,
            PendingDisconnect,
            Disconnecting,
            Disconnected,
        }

        public enum LastConnectError
        {
            None,
            CommandAborted,
            NotEnabledForInternet,
            UserDisconnect,
            IspDisconnect,
            IdleDisconnect,
            ForcedDisconnect,
            NoCarrier,
            IpConfiguration,
            Unknown,
        }

        public enum PortMappingProtocol
        {
            TCP,
            UDP,
        }

        public WanIpConnectionService(UPNPLib.IUPnPService service)
        {
            _service = service;
        }

        public void GetSpecificPortMappingEntry()
        {
        }

        public void AddPortMapping(IPAddress remoteHost, int externalPort, string protocol, int internalPort, IPAddress internalClient, bool enabled, string description, TimeSpan duration)
        {
            object inParams = new object[] {
                remoteHost,
                externalPort,
                protocol,
                internalPort,
                internalClient,
                enabled,
                description,
                duration
            };

            object outParam = new object[] { };

            _service.InvokeAction("AddPortMapping", inParams, ref outParam);
        }

        public void DeletePortMapping(IPAddress remoteHost, int externalPort, string protocol)
        {
            object inParams = new object[] {
                remoteHost,
                externalPort,
                protocol
            };

            object outParam = new object[] { };

            _service.InvokeAction("DeletePortMapping", inParams, ref outParam);
        }

        public string ConnectionType
        {
            get
            {
                return _service.QueryStateVariable("ConnectionType") as string;
            }
        }

        public string GetConnectionStatus
        {
            get
            {
                return _service.QueryStateVariable("ConnectionStatus") as string;
            }
        }

        public TimeSpan Uptime
        {
            get
            {
                int seconds = (int)_service.QueryStateVariable("Uptime");

                return TimeSpan.FromSeconds(seconds);
            }
        }

        public IPAddress ExternalIPAddress
        {
            get
            {
                string ipAddress = _service.QueryStateVariable("ExternalIPAddress") as string;

                return IPAddress.Parse(ipAddress);
            }
        }
    }
}
