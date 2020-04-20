using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace NumberDivisor
{
    class Program
    {
        static void Main(string[] args)
        {
            var arraySize = 10000000;
            var array= BuildAnArray(arraySize);

            var stopwatch = Stopwatch.StartNew();
            var firstArray = array.Take(array.Length / 2);
            var lastArray = array.Skip(array.Length / 2);

            var arrayProcessorFirst = new ArrayProcessor(firstArray.ToArray(), 0, firstArray.ToList().Count);
            var arrayProcessorLast = new ArrayProcessor(lastArray.ToArray(), 0, lastArray.ToList().Count);

            Thread firstThread = new Thread(arrayProcessorFirst.CalculateDivisor);
            Thread lastThread = new Thread(arrayProcessorLast.CalculateDivisor);

            
            firstThread.Start();
            lastThread.Start();


            lastThread.Join();
            firstThread.Join();
            

            var totalSum = arrayProcessorFirst.Sum + arrayProcessorLast.Sum;
            stopwatch.Stop();

            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalMilliseconds} ms");
            Console.WriteLine($"The most number is : {totalSum}");


        }

        public static int[] BuildAnArray(int size)
        {
            var array = new int[size];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = i;
            }

            return array;
        }
    }
}
