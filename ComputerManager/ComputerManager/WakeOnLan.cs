using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace ComputerManager
{
    public class WakeOnLan
    {
        private static readonly int broadcastPort;
        private static readonly byte[] header;

        static WakeOnLan()
        {
            broadcastPort = 1; // Can be any port for WOL.  I like 1.
            header = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        }

        private WakeOnLan()
        {
            // Singleton...
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        public static extern int SendARP(uint DestIP, uint SrcIP, byte[] pMacAddr, ref uint PhyAddrLen);

        public static byte[] GetMacAddress(string hostname)
        {
            IPHostEntry host = Dns.GetHostEntry(hostname);

            if (host.AddressList.Length == 0)
            {
                throw new Exception("Unable to resolve hostname.");
            }

            return GetMacAddress(host.AddressList[0]);
        }

        public static byte[] GetMacAddress(IPAddress address)
        {
            uint macAddressLength = 6;
            byte[] macAddress = new byte[macAddressLength];

            int hr = SendARP((uint)address.Address, 0, macAddress, ref macAddressLength);

            if (hr > 0)
            {
                throw new Win32Exception(hr);
            }

            return (byte[])macAddress.Clone();
        }

        public static void WakeUp(byte[] macAddress)
        {
            if (macAddress.Length != 6)
            {
                throw new ArgumentException("MAC Address must be 6 bytes long.", "macAddress");
            }

            byte[] payload = new byte[102];

            // The "Magic Packet" consists of 6 bytes of 0xFF, followed by the
            // MAC Address repeated 16 times.

            header.CopyTo(payload, 0);

            for (int i = 1; i < 17; i++)
            {
                macAddress.CopyTo(payload, i * macAddress.Length);
            }

            UdpClient client = new UdpClient();
            client.Connect(IPAddress.Broadcast, broadcastPort);
            client.Send(payload, payload.Length);
        }
    }
}
