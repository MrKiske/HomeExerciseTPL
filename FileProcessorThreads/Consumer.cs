using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace FileProcessorThreads
{
    public class Consumer
    {
        private readonly BlockingCollection<string> blockingCollection;
        private readonly CountdownEvent countdownEvent;
        private readonly ConcurrentDictionary<string, string> data;

        public Consumer(BlockingCollection<string> blockingCollection, CountdownEvent countdownEvent)
        {
            this.blockingCollection = blockingCollection;
            this.countdownEvent = countdownEvent;
            this.data = new ConcurrentDictionary<string, string>();
        }

        public void Start()
        {
            var limit = 0;
            while (true && countdownEvent.InitialCount> limit)
            {
                var result = this.blockingCollection.TryTake(out var path,1000);
                if(result)
                {
                    try
                    {
                        var content = File.ReadAllText(path);
                        this.data.TryAdd(path, content);
                        this.countdownEvent.Signal();
                        limit++;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Retry for {path} on {Thread.CurrentThread.ManagedThreadId}");
                        this.blockingCollection.Add(path);
                    }
                }
               
               
            }
        }

        public ConcurrentDictionary <String, String> GetData()
        {
            return this.data;
        }
    }
}
