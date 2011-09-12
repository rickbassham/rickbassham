using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace SharpFtpClient
{
    public class FtpConnection
    {
        private TcpClient _controlClient;
        private TcpClient _dataClient;

        private NetworkStream _controlStream;
        private StreamReader _controlReader;
        private StreamWriter _controlWriter;

        private StreamReader _dataReader;
        private StreamWriter _dataWriter;

        public void Connect(string host, int port)
        {
            _controlClient = new TcpClient();

            _controlClient.Connect(host, port);

            _controlStream = _controlClient.GetStream();

            _controlReader = new StreamReader(_controlStream);
            _controlWriter = new StreamWriter(_controlStream);

            _controlWriter.AutoFlush = true;

            string response = _controlReader.ReadLine();

            if (!response.StartsWith("220"))
            {
                throw new Exception(response);
            }
        }

        public void Login(string username, string password)
        {
            _controlWriter.WriteLine("USER {0}", username);

            string response = _controlReader.ReadLine();

            if (response.StartsWith("331"))
            {
                _controlWriter.WriteLine("PASS {0}");

                response = _controlReader.ReadLine();

                if (!response.StartsWith("230"))
                {
                    throw new Exception(response);
                }
            }
            else
            {
                throw new Exception(response);
            }
        }
    }
}
