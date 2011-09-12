using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;

using log4net;

namespace SharpFtpServer
{
    public class FtpServer : IDisposable
    {
        ILog _log = LogManager.GetLogger(typeof(FtpServer));

        private bool _disposed = false;

        TcpListener _listener;

        List<ClientConnection> _activeConnections;

        public FtpServer()
        {
        }

        public void Start()
        {
            _listener = new TcpListener(IPAddress.Any, 21);

            _log.Info("#Version: 1.0");
            _log.Info("#Fields: date time c-ip c-port cs-username cs-method cs-uri-stem sc-status sc-bytes cs-bytes s-name s-port");

            _listener.Start();

            _activeConnections = new List<ClientConnection>();

            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);
        }

        public void Stop()
        {
            _log.Info("Stopping FtpServer");

            _listener.Stop();

            _listener = null;
        }

        private void HandleAcceptTcpClient(IAsyncResult result)
        {
            TcpClient client = _listener.EndAcceptTcpClient(result);

            ClientConnection connection = new ClientConnection(client);

            _activeConnections.Add(connection);

            ThreadPool.QueueUserWorkItem(connection.HandleClient, client);

            _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_listener != null)
                    {
                        _listener.Stop();
                    }

                    foreach (ClientConnection conn in _activeConnections)
                    {
                        conn.Dispose();
                    }
                }
            }

            _disposed = true;
        }
    }
}
