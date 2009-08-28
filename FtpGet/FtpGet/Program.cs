using System;
using System.Collections.Generic;
using System.Text;
using EnterpriseDT.Net.Ftp;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace FtpGet
{
    class Program
    {
        static long iterations = 0;
        static long totalSize = 0;

        static Queue<long> bytesQueue = new Queue<long>(20);
        static Queue<DateTime> timeQueue = new Queue<DateTime>(20);

        static List<float> speedList = new List<float>(5);

        static void Main(string[] args)
        {
            Uri uri = new Uri(args[0]);

            using (FTPConnection conn = new FTPConnection())
            {
                conn.Downloading += new FTPFileTransferEventHandler(conn_Downloading);
                conn.BytesTransferred += new BytesTransferredHandler(conn_BytesTransferred);
                conn.CommandSent += new FTPMessageHandler(conn_CommandSent);
                conn.ReplyReceived += new FTPMessageHandler(conn_ReplyReceived);

                conn.AutoLogin = true;
                conn.ServerAddress = uri.Host;
                conn.ServerPort = uri.Port;
                conn.UserName = uri.UserInfo.Split(':')[0];
                conn.Password = uri.UserInfo.Split(':')[1];
                conn.ConnectMode = FTPConnectMode.PASV;
                conn.Protocol = FileTransferProtocol.FTP;

                conn.Connect();

                conn.ServerDirectory = Path.GetDirectoryName(uri.AbsolutePath);
                conn.DownloadFile(Path.GetFileName(uri.AbsolutePath), Path.GetFileName(uri.AbsolutePath));
            }
        }

        static void conn_Downloading(object sender, FTPFileTransferEventArgs e)
        {
            FTPConnection conn = (FTPConnection)sender;

            conn.TransferNotifyInterval = e.FileSize / 200;

            timeQueue.Enqueue(DateTime.Now);
            bytesQueue.Enqueue(0);
            totalSize = e.FileSize;
        }

        static void conn_CommandSent(object sender, FTPMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void conn_ReplyReceived(object sender, FTPMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        static void conn_BytesTransferred(object sender, BytesTransferredEventArgs e)
        {
            timeQueue.Enqueue(DateTime.Now);
            bytesQueue.Enqueue(e.ByteCount);

            int y = Console.CursorTop;

            if (iterations > 0)
            {
                y = y - 1;
            }

            TimeSpan remaining = TimeSpan.MinValue;
            float speed = 0f;

            if (timeQueue.Count == 20)
            {
                DateTime startTime = timeQueue.Dequeue();
                long startBytes = bytesQueue.Dequeue();

                TimeSpan elapsed = DateTime.Now - startTime;
                long bytesDownloaded = e.ByteCount - startBytes;

                speed = bytesDownloaded / 1024 / (float)elapsed.TotalSeconds;

                if (speedList.Count == 5)
                {
                    speedList.RemoveAt(0);
                }

                speedList.Add(speed);

                long bytesLeft = totalSize - e.ByteCount;

                remaining = TimeSpan.FromSeconds((float)bytesLeft / 1024 / speed);

                speed = GetAverage(speedList);
            }

            Console.SetCursorPosition(0, y);
            Console.Write("                                          ");
            Console.SetCursorPosition(0, y);
            Console.WriteLine("{0:000.00}% {1:0000.00} KB/s {2:00}:{3:00}:{4:00} remaining", (float)e.ByteCount / totalSize * 100, speed, remaining.Hours, remaining.Minutes, remaining.Seconds);

            iterations++;
        }

        private static float GetAverage(List<float> list)
        {
            float total = 0;
            foreach (float value in list)
            {
                total += value;
            }
            float average = total / list.Count;
            return average;
        }  

    }
}
