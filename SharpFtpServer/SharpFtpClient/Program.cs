using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpFtpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "localhost";
            int port = 21;

            string username = "rick";
            string password = "test";


            FtpConnection conn = new FtpConnection();

            conn.Connect(host, port);

            conn.Login(username, password);
        }
    }
}
