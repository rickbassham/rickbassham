using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.IO;
using DirMirror.Properties;

namespace DirMirror
{
    public partial class Service : ServiceBase
    {
        private FileQueue queue;
        private FileSystemWatcher watcher;

        public Service()
        {
            InitializeComponent();
        }

        public void Start()
        {
            queue = new FileQueue();
            watcher = new FileSystemWatcher();

            watcher.Path = Settings.Default.Source;
            watcher.InternalBufferSize = 64 * 1024;
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.DirectoryName | NotifyFilters.FileName;

            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.Created += new FileSystemEventHandler(watcher_Created);
            watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
            watcher.Error += new ErrorEventHandler(watcher_Error);
            watcher.Renamed += new RenamedEventHandler(watcher_Renamed);

            watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            watcher.Dispose();
            watcher = null;
        }

        private void watcher_Created(object sender, FileSystemEventArgs e)
        {
            queue.AddItem(e);
        }

        private void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            queue.AddItem(e);
        }

        private void watcher_Error(object sender, ErrorEventArgs e)
        {
            Logger.LoggerFactory.Error(e.GetException());
        }

        private void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            queue.AddItem(e);
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            queue.AddItem(e);
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        protected override void OnStop()
        {
            Stop();
        }
    }
}
