using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.Globalization;
using System.Net.Sockets;
using System.IO;

namespace ComputerManager
{
    public enum Status
    {
        On,
        Off,
        Unknown,
    }

    public class Group
    {
        private string _name;
        private List<Computer> _computers;

        public Group()
            : this(null)
        {
        }

        public Group(string name)
        {
            _name = name;
            _computers = new List<Computer>();
        }

        [XmlAttribute("name")]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public List<Computer> Computers
        {
            get
            {
                return _computers;
            }
            set
            {
                _computers = value;
            }
        }
    }

    public class Computer
    {
        private static readonly List<char> hexChars;

        private string _hostname;
        private byte[] _macAddress;

        static Computer()
        {
            hexChars = new List<char>(new char[] {
                '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 
                'a', 'b', 'c', 'd', 'e', 'f',
                'A', 'B', 'C', 'D', 'E', 'F',
            });
        }

        public Computer()
            : this(null, null)
        {
        }

        public Computer(string hostname)
            : this(hostname, null)
        {
        }

        public Computer(string hostname, byte[] macAddress)
        {
            _hostname = hostname;

            if (macAddress == null)
            {
                _macAddress = new byte[6];
            }
            else
            {
                _macAddress = (byte[])macAddress.Clone();
            }
        }

        public void PopulateMacAddressFromHostname()
        {
            _macAddress = WakeOnLan.GetMacAddress(this._hostname);
        }

        public Status CheckStatus()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(_hostname, 35353);

                    using (StreamWriter writer = new StreamWriter(client.GetStream()))
                    using (StreamReader reader = new StreamReader(client.GetStream()))
                    {
                        writer.WriteLine("STATUS");

                        if (reader.ReadLine() == "OK")
                            return Status.On;
                    }
                }
            }
            catch
            {
            }

            return Status.Off;
        }

        public void WakeUp()
        {
            WakeOnLan.WakeUp(_macAddress);
        }

        private string GetByteValue(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);

            for (int i = 0; i < bytes.Length; i++)
            {
                if (i > 0)
                    sb.Append("-");

                sb.Append(bytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private void SetByteValue(string val, byte[] bytes)
        {
            if (val == null)
            {
                throw new ArgumentNullException("val");
            }

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            StringBuilder hex = new StringBuilder();

            foreach (char c in val)
            {
                if (hexChars.Contains(c))
                {
                    hex.Append(c);
                }
            }

            if ((hex.Length % 2) != 0)
            {
                throw new ArgumentException("String length must be divisible by 2 (2 characters per byte).", "val");
            }

            if (bytes.Length != hex.Length / 2)
            {
                throw new ArgumentException("String length divided by 2 must equal bytes length.");
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.ToString(i * 2, 2), NumberStyles.HexNumber);
            }
        }

        [XmlAttribute("hostname")]
        public string Hostname
        {
            get
            {
                return _hostname;
            }
            set
            {
                _hostname = value;
            }
        }

        [XmlIgnore()]
        public byte[] MacAddress
        {
            get
            {
                return (byte[])_macAddress.Clone();
            }
            set
            {
                _macAddress = (byte[])value.Clone();
            }
        }

        [XmlAttribute("macAddress")]
        public string MacAddressString
        {
            get
            {
                return GetByteValue(_macAddress);
            }
            set
            {
                SetByteValue(value, _macAddress);
            }
        }
    }
}
