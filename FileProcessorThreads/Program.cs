using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessorThreads
{
    internal class Program
    {
        static string path= @"C:\Users\Public\Downloads";
        public static void Main(string[] args)
        {
            var collection = new BlockingCollection<string>(4);
            var countdownEvent = new CountdownEvent(10);
            var publisher = new Publisher(collection);
            var consumer = new Consumer(collection, countdownEvent);

            Thread thread1 = new  Thread(()=> publisher.Start(path));
            Thread thread2 = new Thread(consumer.Start);
            
            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            countdownEvent.Wait();

            var data = consumer.GetData();
            Parallel.ForEach(data, item =>
            {
                Thread thread = new Thread(() =>
                {
                    Console.WriteLine($"{item.Key} - {item.Value}");
                });

                thread.Start();
            });

        }
    }
}
