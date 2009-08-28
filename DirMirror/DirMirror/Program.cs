using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System;
using System.IO;
using DirMirror.Properties;

namespace DirMirror
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
/*
            for (int i = 0; i < 10000; i++)
            {
                using (FileStream fs = File.Create(Path.Combine(Settings.Default.Source, string.Format("temp\\{0}.txt", i))))
                {
                }
            }

            return;
*/
            if (Environment.UserInteractive)
            {
                Service s = new Service();
                s.Start();

                Console.WriteLine("Press any key to stop.");
                Console.ReadKey(true);

                s.Stop();

                s.Dispose();
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] { new Service() });
            }
        }
    }
}