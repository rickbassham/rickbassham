using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ComputerManager
{
    public class Service
    {
        private static readonly int _port;

        static Service()
        {
            _port = 35353;
        }

        private TcpListener _listener;

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, _port);

            _listener.Start();

            _listener.BeginAcceptTcpClient(DoBeginAcceptTcpClient, _listener);
        }

        public void Stop()
        {
            _listener.Stop();
        }

        private void DoBeginAcceptTcpClient(IAsyncResult ar)
        {
            using (TcpClient client = _listener.EndAcceptTcpClient(ar))
            {
                using (StreamWriter writer = new StreamWriter(client.GetStream()))
                using (StreamReader reader = new StreamReader(client.GetStream()))
                {
                    string command = reader.ReadLine();

                    switch (command)
                    {
                        case "STATUS":
                            writer.WriteLine("OK");
                            break;
                    }
                }

                client.Close();
            }
        }
    }
}