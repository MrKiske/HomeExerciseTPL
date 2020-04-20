using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessorTasks
{
    internal class Program
    {
        static string path = @"C:\Users\Public\Downloads";

        static void Main(string[] args)
        {
            var blockingCollection = new BlockingCollection<string>(4);
            var countdownEvent = new CountdownEvent(10);
            var cancellationTokenSource = new CancellationTokenSource();

            var publisher = new Publisher(blockingCollection,cancellationTokenSource.Token);
            var consumer = new Consumer(blockingCollection,  countdownEvent, cancellationTokenSource.Token );

            publisher.Start(path);
            Task.Run(() => consumer.Start(), cancellationTokenSource.Token);

            countdownEvent.Wait(cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            var data = consumer.GetData();
            Parallel.ForEach(data, item =>
            {
                Console.WriteLine($"{item.Key} - {item.Value}");
            });

        }


        public class Publisher
        {
            private readonly BlockingCollection<string> blockingCollection;
            private readonly CancellationToken cancellationToken;

            public Publisher(BlockingCollection<string> blockingCollection, CancellationToken cancellationToken)
            {
                this.blockingCollection = blockingCollection;
                this.cancellationToken = cancellationToken;
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
                    this.blockingCollection.Add(args.FullPath, cancellationToken);
                };
                watcher.EnableRaisingEvents = true;

            }

        }

        public class Consumer
        {
            private readonly BlockingCollection<string> blockingCollection;
            private readonly ConcurrentDictionary<string, string> data;
            private readonly CountdownEvent countdownEvent;
            private readonly CancellationToken cancellationToken;

            public Consumer(BlockingCollection<string> blockingCollection, CountdownEvent countdownEvent, CancellationToken cancellationToken)
            {
                this.blockingCollection = blockingCollection;
                this.data = new ConcurrentDictionary<string, string>();
                this.countdownEvent = countdownEvent;
                this.cancellationToken = cancellationToken;
            }

            public void Start()
            {

                while (true && !cancellationToken.IsCancellationRequested)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = this.blockingCollection.TryTake(out var path, 1000000, cancellationToken);
                            if (result)
                            {
                                try
                                {
                                    var content = File.ReadAllText(path);
                                    this.data.TryAdd(path, content);
                                    this.countdownEvent.Signal();
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine($"Retry for {path} on {Thread.CurrentThread.ManagedThreadId}");
                                    this.blockingCollection.Add(path);
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        } 
                    }
                }
            }

            public ConcurrentDictionary<string, string> GetData()
            {
                return this.data;
            }

        }
    }
}
