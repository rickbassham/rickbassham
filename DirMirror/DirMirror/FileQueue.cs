using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using DirMirror.Properties;

namespace DirMirror
{
    internal class FileQueue
    {
        AutoResetEvent signal;

        string dest = Settings.Default.Destination;
        string src = Settings.Default.Source;

        Queue<FileSystemEventArgs> _items;

        private bool _executing = false;

        public FileQueue()
        {
            _items = new Queue<FileSystemEventArgs>(64 * 1024);

            signal = new AutoResetEvent(false);

            ThreadPool.RegisterWaitForSingleObject(signal, new WaitOrTimerCallback(PerformAction), null, -1, false);
        }

        public void AddItem(FileSystemEventArgs item)
        {
            _items.Enqueue(item);

            if (!_executing)
            {
                signal.Set();
            }
        }

        private void PerformAction(object state, bool timedOut)
        {
            _executing = true;

            while (_items.Count > 0)
            {
                bool failed = false;
                int i = 0;

                FileSystemEventArgs item = _items.Dequeue();

                string srcPath = null;
                string srcOldPath = null;
                string destPath = null;
                string destOldPath = null;

                srcPath = item.FullPath;
                destPath = item.FullPath.Replace(src, string.Empty);
                destPath = destPath.StartsWith("\\") ? destPath.Remove(0, 1) : destPath;
                destPath = Path.Combine(dest, destPath);

                if (item is RenamedEventArgs)
                {
                    RenamedEventArgs rItem = (RenamedEventArgs)item;

                    srcOldPath = rItem.OldFullPath;
                    destOldPath = rItem.OldFullPath.Replace(src, string.Empty);
                    destOldPath = destPath.StartsWith("\\") ? destOldPath.Remove(0, 1) : destOldPath;
                    destOldPath = Path.Combine(dest, destOldPath);
                }

                do
                {
                    i++;

                    Thread.Sleep(Convert.ToInt32(Math.Pow(i, i)));
                    try
                    {
                        switch (item.ChangeType)
                        {
                            case WatcherChangeTypes.Deleted:
                                if (File.Exists(destPath))
                                {
                                    File.Delete(destPath);
                                }
                                else if (Directory.Exists(destPath))
                                {
                                    Directory.Delete(destPath);
                                }
                                break;
                            case WatcherChangeTypes.Changed:
                                if (File.Exists(srcPath))
                                {
                                    File.Copy(srcPath, destPath, true);
                                }
                                break;
                            case WatcherChangeTypes.Renamed:
                                if (File.Exists(srcPath))
                                {
                                    File.Move(destOldPath, destPath);
                                }
                                if (Directory.Exists(srcPath))
                                {
                                    Directory.Move(destOldPath, destPath);
                                }
                                break;
                            case WatcherChangeTypes.Created:
                                if (Directory.Exists(srcPath))
                                {
                                    Directory.CreateDirectory(destPath);
                                }
                                else if (File.Exists(srcPath))
                                {
                                    File.Copy(srcPath, destPath, true);
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(i);

                        failed = true;

                        if (i >= 9)
                        {
                            Logger.LoggerFactory.Error(ex);

                            Console.WriteLine("srcPath = {0}", srcPath);
                            Console.WriteLine("srcOldPath = {0}", srcOldPath);
                            Console.WriteLine("destPath = {0}", destPath);
                            Console.WriteLine("destOldPath = {0}", destOldPath);

                            throw;
                        }
                    }
                } while (failed && i < 9);
            }

            _executing = false;
        }
    }
}