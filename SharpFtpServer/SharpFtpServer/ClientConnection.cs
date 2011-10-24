using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Net;
using log4net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SharpFtpServer
{
    public class ClientConnection : IDisposable
    {
        ILog _log = LogManager.GetLogger(typeof(ClientConnection));

        private static long CopyStream(Stream input, Stream output, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            long total = 0;

            while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, count);
                total += count;
            }

            return total;
        }

        private static long CopyStreamAscii(Stream input, Stream output, int bufferSize)
        {
            char[] buffer = new char[bufferSize];
            int count = 0;
            long total = 0;

            using (StreamReader rdr = new StreamReader(input, Encoding.ASCII))
            {
                using (StreamWriter wtr = new StreamWriter(output, Encoding.ASCII))
                {
                    while ((count = rdr.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        wtr.Write(buffer, 0, count);
                        total += count;
                    }
                }
            }

            return total;
        }

        private long CopyStream(Stream input, Stream output)
        {
            if (_connectionType == TransferType.Image)
            {
                return CopyStream(input, new RateLimitingStream(output, 131072, 0.5), 4096);
            }
            else
            {
                return CopyStreamAscii(input, new RateLimitingStream(output, 131072, 0.5), 4096);
            }
        }

        private enum TransferType
        {
            Ascii,
            Ebcdic,
            Image,
            Local,
        }

        private enum FormatControlType
        {
            NonPrint,
            Telnet,
            CarriageControl,
        }

        private enum DataConnectionType
        {
            Passive,
            Active,
        }

        private enum FileStructureType
        {
            File,
            Record,
            Page,
        }

        private bool _disposed = false;

        private TcpListener _passiveListener;

        private TcpClient _controlClient;
        private TcpClient _dataClient;

        private NetworkStream _controlStream;
        private StreamReader _controlReader;
        private StreamWriter _controlWriter;

        private StreamReader _dataReader;
        private StreamWriter _dataWriter;

        private TransferType _connectionType;

//        private FormatControlType _formatControlType;
//        private int _localFormatByteSize = -1;

        private DataConnectionType _dataConnectionType;

//        private FileStructureType _fileStructureType = FileStructureType.File;

        private string _username;
        private string _root;
        private string _currentDirectory;
        private IPEndPoint _dataEndpoint;
        private IPEndPoint _remoteEndPoint;

        private X509Certificate _cert = null;
        private SslStream _sslStream;

        string _clientIP;

        private User _currentUser;

        private List<string> _validCommands;

        public ClientConnection(TcpClient client)
        {
            _controlClient = client;

            _validCommands = new List<string>();
        }

        private string CheckUser()
        {
            if (_currentUser == null)
            {
                return "530 Not logged in";
            }

            return null;
        }

        public void HandleClient(object obj)
        {
            _remoteEndPoint = (IPEndPoint)_controlClient.Client.RemoteEndPoint;

            _clientIP = _remoteEndPoint.Address.ToString();

            _controlStream = _controlClient.GetStream();

            _controlReader = new StreamReader(_controlStream);
            _controlWriter = new StreamWriter(_controlStream);

            _controlWriter.WriteLine("220 Service Ready.");
            _controlWriter.Flush();

            _validCommands.AddRange(new string[] { "AUTH", "USER", "PASS", "QUIT", "HELP", "NOOP" });

            string line;

            _dataClient = new TcpClient();

            string renameFrom = null;

            try
            {
                while ((line = _controlReader.ReadLine()) != null)
                {
                    string response = null;

                    string[] command = line.Split(' ');

                    string cmd = command[0].ToUpperInvariant();
                    string arguments = command.Length > 1 ? line.Substring(command[0].Length + 1) : null;

                    if (arguments != null && arguments.Trim().Length == 0)
                    {
                        arguments = null;
                    }

                    LogEntry logEntry = new LogEntry
                    {
                        Date = DateTime.Now,
                        CIP = _clientIP,
                        CSUriStem = arguments
                    };

                    if (!_validCommands.Contains(cmd))
                    {
                        response = CheckUser();
                    }

                    if (cmd != "RNTO")
                    {
                        renameFrom = null;
                    }

                    if (response == null)
                    {
                        switch (cmd)
                        {
                            case "USER":
                                response = User(arguments);
                                break;
                            case "PASS":
                                response = Password(arguments);
                                logEntry.CSUriStem = "******";
                                break;
                            case "CWD":
                                response = ChangeWorkingDirectory(arguments);
                                break;
                            case "CDUP":
                                response = ChangeWorkingDirectory("..");
                                break;
                            case "QUIT":
                                response = "221 Service closing control connection";
                                break;
                            case "REIN":
                                _currentUser = null;
                                _username = null;
                                _passiveListener = null;
                                _dataClient = null;

                                response = "220 Service ready for new user";
                                break;
                            case "PORT":
                                response = Port(arguments);
                                logEntry.CPort = _dataEndpoint.Port.ToString();
                                break;
                            case "PASV":
                                response = Passive();
                                logEntry.SPort = ((IPEndPoint)_passiveListener.LocalEndpoint).Port.ToString();
                                break;
                            case "TYPE":
                                response = Type(command[1], command.Length == 3 ? command[2] : null);
                                logEntry.CSUriStem = command[1];
                                break;
                            case "STRU":
                                response = Structure(arguments);
                                break;
                            case "MODE":
                                if (arguments.ToUpperInvariant() == "S")
                                {
                                    response = "200 OK";
                                }
                                else
                                {
                                    response = "504 Command not implemented for that parameter";
                                }

                                break;
                            case "RNFR":
                                renameFrom = arguments;
                                response = "350 Requested file action pending further information";
                                break;
                            case "RNTO":
                                response = Rename(renameFrom, arguments);
                                break;
                            case "DELE":
                                response = Delete(arguments);
                                break;
                            case "RMD":
                                response = RemoveDir(arguments);
                                break;
                            case "MKD":
                                response = CreateDir(arguments);
                                break;
                            case "PWD":
                                response = PrintWorkingDirectory();
                                break;
                            case "AUTH":
                                response = Auth(arguments);
                                break;
                            case "RETR":
                                response = Retrieve(arguments);
                                logEntry.Date = DateTime.Now;
                                break;
                            case "STOR":
                                response = Store(arguments);
                                logEntry.Date = DateTime.Now;
                                break;
                            case "STOU":
                                response = StoreUnique();
                                logEntry.Date = DateTime.Now;
                                break;
                            case "APPE":
                                response = Append(arguments);
                                logEntry.Date = DateTime.Now;
                                break;
                            case "LIST":
                                response = List(arguments ?? _currentDirectory);
                                logEntry.Date = DateTime.Now;
                                break;
                            case "SYST":
                                response = "215 UNIX Type: L8";
                                break;
                            case "NOOP":
                                response = "200 OK";
                                break;
                            case "ACCT":
                                response = "200 OK";
                                break;
                            case "ALLO":
                                response = "200 OK";
                                break;
                            case "NLST":
                                response = "502 Command not implemented";
                                break;
                            case "SITE":
                                response = "502 Command not implemented";
                                break;
                            case "STAT":
                                response = "502 Command not implemented";
                                break;
                            case "HELP":
                                response = "502 Command not implemented";
                                break;
                            case "SMNT":
                                response = "502 Command not implemented";
                                break;
                            case "REST":
                                response = "502 Command not implemented";
                                break;
                            case "ABOR":
                                response = "502 Command not implemented";
                                break;
                            // Extensions defined by rfc2389
                            case "FEAT":
                                response = FeatureList();
                                break;
                            case "OPTS":
                                response = Options(arguments);
                                break;

                            // Extensions defined by rfc3659
                            case "MDTM":
                                response = FileModificationTime(arguments);
                                break;
                            case "SIZE":
                                response = FileSize(arguments);
                                break;

                            default:
                                response = "502 Command not implemented";
                                break;
                        }
                    }

                    logEntry.CSMethod = cmd;
                    logEntry.CSUsername = _username;
                    logEntry.SCStatus = response.Substring(0, response.IndexOf(' '));

                    _log.Info(logEntry);

                    if (_controlClient != null && _controlClient.Connected)
                    {
                        _controlWriter.WriteLine(response);
                        _controlWriter.Flush();

                        if (cmd == "AUTH")
                        {
                            _cert = new X509Certificate("localhost.cer");
                            _sslStream = new SslStream(_controlStream, true, RemoteCallback, LocalCallback, EncryptionPolicy.RequireEncryption);

                            _sslStream.AuthenticateAsServer(_cert, false, System.Security.Authentication.SslProtocols.Tls, false);

                            _controlReader = new StreamReader(_sslStream);
                            _controlWriter = new StreamWriter(_sslStream);
                        }
                    }
                    else
                    {
                        break;
                    }

                    if (response.StartsWith("221"))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
            }

            Dispose();
        }

        private bool IsPathValid(string path)
        {
            return path.StartsWith(_root);
        }

        private string NormalizeFilename(string path)
        {
            if (path == "/")
            {
                return _root;
            }
            else if (path.StartsWith("/"))
            {
                return new FileInfo(Path.Combine(_root, path.Substring(1))).FullName;
            }
            else
            {
                return new FileInfo(Path.Combine(_currentDirectory, path)).FullName;
            }
        }

        #region FTP Commands

        private bool RemoteCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private X509Certificate LocalCallback(Object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return localCertificates[0];
        }

        private string FeatureList()
        {
            _controlWriter.WriteLine("211- Extensions supported:");
            _controlWriter.WriteLine(" MDTM");
            _controlWriter.WriteLine(" SIZE");
            return "211 End";
        }

        private string Options(string arguments)
        {
            return "200 Looks good to me...";
        }

        private string Auth(string authMode)
        {
            if (authMode == "TLS")
            {
                return "234 Enabling TLS Connection";
            }
            else
            {
                return "504 Unrecognized AUTH mode";
            }
        }

        private string User(string username)
        {
            _username = username;

            return "331 Username ok, need password";
        }

        private string Password(string password)
        {
            _currentUser = UserStore.Validate(_username, password);

            if (_currentUser != null)
            {
                _root = _currentUser.HomeDir;
                _currentDirectory = _root;

                return "230 User logged in";
            }
            else
            {
                return "530 Not logged in";
            }
        }

        private string ChangeWorkingDirectory(string pathname)
        {
            if (pathname == "/")
            {
                _currentDirectory = _root;
            }
            else
            {
                string newDir;

                if (pathname.StartsWith("/"))
                {
                    pathname = pathname.Substring(1).Replace('/', '\\');
                    newDir = Path.Combine(_root, pathname);
                }
                else
                {
                    pathname = pathname.Replace('/', '\\');
                    newDir = Path.Combine(_currentDirectory, pathname);
                }

                if (Directory.Exists(newDir))
                {
                    _currentDirectory = new DirectoryInfo(newDir).FullName;

                    if (!IsPathValid(_currentDirectory))
                    {
                        _currentDirectory = _root;
                    }
                }
                else
                {
                    _currentDirectory = _root;
                }
            }

            return "200 Changed to new directory";
        }

        private string Port(string hostPort)
        {
            _dataConnectionType = DataConnectionType.Active;

            string[] ipAndPort = hostPort.Split(',');

            byte[] ipAddress = new byte[4];
            byte[] port = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                ipAddress[i] = Convert.ToByte(ipAndPort[i]);
            }

            for (int i = 4; i < 6; i++)
            {
                port[i - 4] = Convert.ToByte(ipAndPort[i]);
            }

            _dataEndpoint = new IPEndPoint(new IPAddress(ipAddress), (port[0] << 8 | port[1]));

            return "200 Data Connection Established";
        }

        private string Passive()
        {
            _dataConnectionType = DataConnectionType.Passive;

            _passiveListener = new TcpListener(IPAddress.Loopback, 0);
            _passiveListener.Start();

            byte[] address = ((IPEndPoint)_passiveListener.LocalEndpoint).Address.GetAddressBytes();
            int port = ((IPEndPoint)_passiveListener.LocalEndpoint).Port;

            byte[] portArray = new byte[2];

            portArray[0] = (byte)(port >> (8 & 0xFF));
            portArray[1] = (byte)(port >> (0 & 0xFF));

            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }

        private string Type(string typeCode, string formatControl)
        {
            switch (typeCode.ToUpperInvariant())
            {
                case "A":
                    _connectionType = TransferType.Ascii;
                    break;
                case "I":
                    _connectionType = TransferType.Image;
                    break;
                default:
                    return "504 Command not implemented for that parameter";
            }

            if (!string.IsNullOrWhiteSpace(formatControl))
            {
                switch (formatControl.ToUpperInvariant())
                {
                    case "N":
//                        _formatControlType = FormatControlType.NonPrint;
                        break;
                    default:
                        return "504 Command not implemented for that parameter";
                }
            }

            return string.Format("200 Type set to {0}", _connectionType);
        }

        private string Delete(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (File.Exists(pathname))
                {
                    File.Delete(pathname);
                }
                else
                {
                    return "550 File Not Found";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 File Not Found";
        }

        private string RemoveDir(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (Directory.Exists(pathname))
                {
                    Directory.Delete(pathname);
                }
                else
                {
                    return "550 Directory Not Found";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 Directory Not Found";
        }

        private string CreateDir(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (!Directory.Exists(pathname))
                {
                    Directory.CreateDirectory(pathname);
                }
                else
                {
                    return "550 Directory already exists";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 Directory Not Found";
        }

        private string FileModificationTime(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (File.Exists(pathname))
                {
                    return string.Format("213 {0}", File.GetLastWriteTime(pathname).ToString("yyyyMMddHHmmss.fff"));
                }
            }

            return "550 File Not Found";
        }

        private string FileSize(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (File.Exists(pathname))
                {
                    long length = 0;

                    using (FileStream fs = File.Open(pathname, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        length = fs.Length;
                    }

                    return string.Format("213 {0}", length);
                }
            }

            return "550 File Not Found";
        }

        private string Retrieve(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (File.Exists(pathname))
                {
                    if (_dataConnectionType == DataConnectionType.Active)
                    {
                        _dataClient = new TcpClient();
                        _dataClient.BeginConnect(_dataEndpoint.Address, _dataEndpoint.Port, DoRetrieve, pathname);
                    }
                    else
                    {
                        _passiveListener.BeginAcceptTcpClient(DoRetrieve, pathname);
                    }

                    return string.Format("150 Opening {0} mode data transfer for RETR", _dataConnectionType);
                }
            }

            return "550 File Not Found";
        }

        private string Store(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (_dataConnectionType == DataConnectionType.Active)
                {
                    _dataClient = new TcpClient();
                    _dataClient.BeginConnect(_dataEndpoint.Address, _dataEndpoint.Port, DoStore, pathname);
                }
                else
                {
                    _passiveListener.BeginAcceptTcpClient(DoStore, pathname);
                }

                return string.Format("150 Opening {0} mode data transfer for STOR", _dataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        private string Append(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (IsPathValid(pathname))
            {
                if (_dataConnectionType == DataConnectionType.Active)
                {
                    _dataClient = new TcpClient();
                    _dataClient.BeginConnect(_dataEndpoint.Address, _dataEndpoint.Port, DoAppend, pathname);
                }
                else
                {
                    _passiveListener.BeginAcceptTcpClient(DoAppend, pathname);
                }

                return string.Format("150 Opening {0} mode data transfer for APPE", _dataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        private string StoreUnique()
        {
            string pathname = NormalizeFilename(new Guid().ToString());

            if (_dataConnectionType == DataConnectionType.Active)
            {
                _dataClient = new TcpClient();
                _dataClient.BeginConnect(_dataEndpoint.Address, _dataEndpoint.Port, DoStore, pathname);
            }
            else
            {
                _passiveListener.BeginAcceptTcpClient(DoStore, pathname);
            }

            return string.Format("150 Opening {0} mode data transfer for STOU", _dataConnectionType);
        }

        private string PrintWorkingDirectory()
        {
            string current = _currentDirectory.Replace(_root, string.Empty).Replace('\\', '/');

            if (current.Length == 0)
            {
                current = "/";
            }

            return string.Format("257 \"{0}\" is current directory.", current); ;
        }

        private string List(string pathname)
        {
            if (pathname == null)
            {
                pathname = string.Empty;
            }

            pathname = new DirectoryInfo(Path.Combine(_currentDirectory, pathname)).FullName;

            if (IsPathValid(pathname))
            {
                if (_dataConnectionType == DataConnectionType.Active)
                {
                    _dataClient = new TcpClient();
                    _dataClient.BeginConnect(_dataEndpoint.Address, _dataEndpoint.Port, DoList, pathname);
                }
                else
                {
                    _passiveListener.BeginAcceptTcpClient(DoList, pathname);
                }

                return string.Format("150 Opening {0} mode data transfer for LIST", _dataConnectionType);
            }

            return "450 Requested file action not taken";
        }

        private string Structure(string structure)
        {
            switch (structure)
            {
                case "F":
//                    _fileStructureType = FileStructureType.File;
                    break;
                case "R":
//                    _fileStructureType = FileStructureType.Record;
                    break;
                case "P":
                    return "504 STRU not implemented for \"P\"";
                default:
                    return string.Format("501 Parameter {0} not recognized", structure);
            }

            return "200 Command OK";
        }

        private string Rename(string renameFrom, string renameTo)
        {
            if (string.IsNullOrWhiteSpace(renameFrom) || string.IsNullOrWhiteSpace(renameTo))
            {
                return "450 Requested file action not taken";
            }

            renameFrom = NormalizeFilename(renameFrom);
            renameTo = NormalizeFilename(renameTo);

            if (IsPathValid(renameFrom) && IsPathValid(renameTo))
            {
                if (File.Exists(renameFrom))
                {
                    File.Move(renameFrom, renameTo);
                }
                else if (Directory.Exists(renameFrom))
                {
                    Directory.Move(renameFrom, renameTo);
                }
                else
                {
                    return "450 Requested file action not taken";
                }

                return "250 Requested file action okay, completed";
            }

            return "450 Requested file action not taken";
        }

        #endregion

        #region DataConnection Operations

        private void HandleAsyncResult(IAsyncResult result)
        {
            if (_dataConnectionType == DataConnectionType.Active)
            {
                _dataClient.EndConnect(result);
            }
            else
            {
                _dataClient = _passiveListener.EndAcceptTcpClient(result);
            }
        }

        private void DoRetrieve(IAsyncResult result)
        {
            HandleAsyncResult(result);

            string pathname = (string)result.AsyncState;

            long bytes = 0;

            using (NetworkStream dataStream = _dataClient.GetStream())
            {
                using (FileStream fs = new FileStream(pathname, FileMode.Open, FileAccess.Read))
                {
                    bytes = CopyStream(fs, dataStream);
                }
            }

            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                CIP = _clientIP,
                CSMethod = "RETR",
                CSUsername = _username,
                SCStatus = "226",
                SCBytes = bytes.ToString()
            };

            _log.Info(logEntry);

            _dataClient.Close();
            _dataClient = null;

            _controlWriter.WriteLine("226 Closing data connection, file transfer successful");
            _controlWriter.Flush();
        }

        private void DoStore(IAsyncResult result)
        {
            HandleAsyncResult(result);

            string pathname = (string)result.AsyncState;

            long bytes = 0;

            using (NetworkStream dataStream = _dataClient.GetStream())
            {
                using (FileStream fs = new FileStream(pathname, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
                {
                    bytes = CopyStream(dataStream, fs);
                }
            }

            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                CIP = _clientIP,
                CSMethod = "STOR",
                CSUsername = _username,
                SCStatus = "226",
                CSBytes = bytes.ToString()
            };

            _log.Info(logEntry);

            _dataClient.Close();
            _dataClient = null;

            _controlWriter.WriteLine("226 Closing data connection, file transfer successful");
            _controlWriter.Flush();
        }

        private void DoAppend(IAsyncResult result)
        {
            HandleAsyncResult(result);

            string pathname = (string)result.AsyncState;

            long bytes = 0;

            using (NetworkStream dataStream = _dataClient.GetStream())
            {
                using (FileStream fs = new FileStream(pathname, FileMode.Append, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
                {
                    bytes = CopyStream(dataStream, fs);
                }
            }

            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                CIP = _clientIP,
                CSMethod = "APPE",
                CSUsername = _username,
                SCStatus = "226",
                CSBytes = bytes.ToString()
            };

            _log.Info(logEntry);

            _dataClient.Close();
            _dataClient = null;

            _controlWriter.WriteLine("226 Closing data connection, file transfer successful");
            _controlWriter.Flush();
        }

        private void DoList(IAsyncResult result)
        {
            HandleAsyncResult(result);

            string pathname = (string)result.AsyncState;

            using (NetworkStream dataStream = _dataClient.GetStream())
            {
                _dataReader = new StreamReader(dataStream);
                _dataWriter = new StreamWriter(dataStream);

                IEnumerable<string> directories = Directory.EnumerateDirectories(pathname);

                foreach (string dir in directories)
                {
                    DirectoryInfo d = new DirectoryInfo(dir);

                    string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                        d.LastWriteTime.ToString("MMM dd  yyyy") :
                        d.LastWriteTime.ToString("MMM dd HH:mm");

                    string line = string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, d.Name);

                    _dataWriter.WriteLine(line);
                    _dataWriter.Flush();
                }

                IEnumerable<string> files = Directory.EnumerateFiles(pathname);

                foreach (string file in files)
                {
                    FileInfo f = new FileInfo(file);

                    string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                        f.LastWriteTime.ToString("MMM dd  yyyy") :
                        f.LastWriteTime.ToString("MMM dd HH:mm");

                    string line = string.Format("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", f.Length, date, f.Name);

                    _dataWriter.WriteLine(line);
                    _dataWriter.Flush();
                }
            }

            LogEntry logEntry = new LogEntry
            {
                Date = DateTime.Now,
                CIP = _clientIP,
                CSMethod = "LIST",
                CSUsername = _username,
                SCStatus = "226"
            };

            _log.Info(logEntry);

            _dataClient.Close();
            _dataClient = null;

            _controlWriter.WriteLine("226 Transfer complete");
            _controlWriter.Flush();
        }

        #endregion

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
                    if (_controlClient != null)
                    {
                        _controlClient.Close();
                    }

                    if (_dataClient != null)
                    {
                        _dataClient.Close();
                    }

                    if (_controlStream != null)
                    {
                        _controlStream.Close();
                    }

                    if (_controlReader != null)
                    {
                        _controlReader.Close();
                    }

                    if (_controlWriter != null)
                    {
                        _controlWriter.Close();
                    }

                    if (_dataReader != null)
                    {
                        _dataReader.Close();
                    }

                    if (_dataWriter != null)
                    {
                        _dataWriter.Close();
                    }
                }
            }

            _disposed = true;
        }
    }
}