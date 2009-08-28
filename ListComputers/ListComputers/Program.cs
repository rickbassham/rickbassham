using System;
using System.Collections.Generic;
using System.Text;

using System.DirectoryServices;
using System.Net;

namespace ListComputers
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectorySearcher searcher = new DirectorySearcher();

            searcher.Filter = "(&ObjectCategory=computer)";

            SearchResultCollection results = searcher.FindAll();

            foreach (SearchResult result in results)
            {
                string path = result.Path.Replace("LDAP://", string.Empty);

                string[] pairs = path.Split(',');

                string computerName = pairs[0].Split('=')[1];

Console.WriteLine(computerName);
continue;

                string ip = null;
                IPAddress[] ips = new IPAddress[0];
                string[] aliases = new string[0];

                try
                {
                    IPHostEntry entry = Dns.GetHostEntry(computerName);
                    ips = entry.AddressList;
                    aliases = entry.Aliases;

                    if (ips.Length > 0)
                    {
                        ip = ips[0].ToString();
                    }
                }
                catch
                {
                }

                Console.WriteLine("{0},{1}", computerName, ip);

                for (int i = 1; i < ips.Length; i++)
                {
                    Console.WriteLine(",{0}", ips[i]);
                }

                for (int i = 0; i < aliases.Length; i++)
                {
                    Console.WriteLine(",{0}", aliases[i]);
                }
            }
        }
    }
}
