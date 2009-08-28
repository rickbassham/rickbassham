using System;
using System.Diagnostics;

using FlashTools;

namespace SWFInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("\nUsage: SWFInfo filename");
                return;
            }

            // Send trace information to the console
            Trace.Listeners.Add(new ConsoleTraceListener());

            SWFFile swf = new SWFFile(args[0]);
            swf.Close();
        }
    }
}
