using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

using SharpSvn;
using System.IO;

namespace SvnMonitor.Cmd
{
    class Program
    {
        static bool _stop = false;

        static void Main(string[] args)
        {
            Console.TreatControlCAsInput = false;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);

            string path = ConfigurationManager.AppSettings["Path"];

            long? lastRevision = null;

            using (SvnClient client = new SvnClient())
            {
                string sourceUrl = client.GetUriFromWorkingCopy(path).ToString();

                SvnUpdateResult updateResult;

                client.Update(path, out updateResult);

                if (updateResult.HasResultMap)
                {
                    foreach (string key in updateResult.ResultMap.Keys)
                    {
                        Console.WriteLine(key);
                    }
                }

                if (updateResult.HasRevision)
                {
                    lastRevision = updateResult.Revision;
                }

                while (!_stop)
                {
                    SvnRevisionRange range;

                    if (lastRevision == null)
                    {
                        range = new SvnRevisionRange(SvnRevision.Working, SvnRevision.Head);
                    }
                    else
                    {
                        range = new SvnRevisionRange(new SvnRevision(lastRevision.Value), SvnRevision.Head);
                    }

                    SvnLogArgs logArgs = new SvnLogArgs(range);

                    List<string> paths = new List<string>();

                    client.Log(path, logArgs, delegate(object sender, SvnLogEventArgs e)
                    {
                        if (lastRevision == null || e.Revision > lastRevision)
                        {
                            paths = new List<string>(e.ChangedPaths.Count);

                            foreach (SvnChangeItem item in e.ChangedPaths)
                            {
                                string changedPath = RemovePrefixFromUrl(sourceUrl, item.Path).Replace('/', '\\');
                                changedPath = Path.Combine(path, changedPath);
                            }

                            lastRevision = e.Revision;
                        }
                    });

                    if (paths.Count > 0)
                    {
                        client.Update(paths);

                        foreach (string updatedPath in paths)
                        {
                            Console.WriteLine(updatedPath);
                        }
                    }

                    Console.WriteLine("{0}", lastRevision);

                    Thread.Sleep(TimeSpan.FromSeconds(60));
                }
            }
        }

        static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Cancel key pressed...");
            e.Cancel = true;
            _stop = true;
        }

        private static string RemovePrefixFromUrl(string baseUrl, string fullUrl)
        {
            string token = string.Empty;

            if (baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl.Substring(0, baseUrl.Length - 1);
            }

            while (true)
            {
                int index = baseUrl.LastIndexOf('/');

                if (index < 0)
                {
                    return string.Empty;
                }

                token = baseUrl.Substring(index) + token;
                baseUrl = baseUrl.Substring(0, index);

                if (fullUrl.StartsWith(token, StringComparison.OrdinalIgnoreCase))
                {
                    if (fullUrl == token)
                    {
                        return string.Empty;
                    }

                    return fullUrl.Substring(token.Length + 1);
                }
            }
        }
    }
}
