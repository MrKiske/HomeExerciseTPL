using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace FileProcessorThreads
{
    public class Publisher
    {
        private readonly BlockingCollection<string> blockingCollection;

        public Publisher(BlockingCollection<string> blockingCollection)
        {
            this.blockingCollection = blockingCollection;
        }

        public void Start(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path);
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.FileName |
                                    NotifyFilters.LastAccess |
                                    NotifyFilters.LastWrite |
                                    NotifyFilters.DirectoryName;

            watcher.Created += (sender, args) =>
            {
                Console.WriteLine($"File read on {Thread.CurrentThread.ManagedThreadId}");
                this.blockingCollection.Add(args.FullPath);
            };
            watcher.EnableRaisingEvents = true;
        }
    }
}
