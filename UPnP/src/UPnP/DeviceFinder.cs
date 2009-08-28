using System;
using System.Collections.Generic;
using System.Text;

using UPNPLib;

namespace UPnP
{
    public class DeviceFinder
    {
        public static void Find()
        {
            UPnPDeviceFinder finder = new UPnPDeviceFinderClass();

            UPnPDevices devices = finder.FindByType("urn:schemas-upnp-org:device:InternetGatewayDevice:1", 0);

            foreach (UPnPDevice device in devices)
            {
                Console.WriteLine(device.Type);
            }
        }
    }
}
