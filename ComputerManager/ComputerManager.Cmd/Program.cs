using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace ComputerManager.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] macAddress = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 };

            try
            {
                byte[] mac = WakeOnLan.GetMacAddress(new System.Net.IPAddress(
                    new byte[] { 10, 97, 202, 41 }));

                Computer c = new Computer("localhost", mac);

                Settings s = new Settings();
                s.Computers.Add(c);

                XmlSerializer ser = new XmlSerializer(typeof(Settings));

                using (StreamWriter stream = File.CreateText("test.xml"))
                    ser.Serialize(stream, s);

                using (StreamReader stream = File.OpenText("test.xml"))
                    s = (Settings)ser.Deserialize(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
