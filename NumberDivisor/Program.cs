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
            var parts = 8;
            var arraySize = 1000000;
            var totalNumbers = arraySize / parts;
            var array= BuildAnArray(arraySize);

            var stopwatch = Stopwatch.StartNew();
            
            int i = 0;
            var splits = from item in array
                         group item by i++ % parts into part
                         select part.AsEnumerable();

            var firstArray = splits.ToArray()[0].ToArray();
            var secondArray = splits.ToArray()[1].ToArray();
            var thirdArray = splits.ToArray()[2].ToArray();
            var fourthArray = splits.ToArray()[3].ToArray();
            var fifthArray = splits.ToArray()[4].ToArray();
            var sixthArray = splits.ToArray()[5].ToArray();
            var seventhArray = splits.ToArray()[6].ToArray();
            var lastArray = splits.ToArray()[7].ToArray();


            #region Instance
            var arrayProcessorFirst = new ArrayProcessor(firstArray.ToArray(), 0, firstArray.ToList().Count, totalNumbers);
            var arrayProcessorSecond = new ArrayProcessor(secondArray.ToArray(), 0, secondArray.ToList().Count, totalNumbers/* * 2*/);
            var arrayProcessorThird = new ArrayProcessor(thirdArray.ToArray(), 0, thirdArray.ToList().Count, totalNumbers /** 3*/);

            var arrayProcessorFourth = new ArrayProcessor(fourthArray.ToArray(), 0, fourthArray.ToList().Count, totalNumbers /** 4*/);
            var arrayProcessorFifth = new ArrayProcessor(fifthArray.ToArray(), 0, fifthArray.ToList().Count, totalNumbers /** 5*/);
            var arrayProcessorSixth = new ArrayProcessor(sixthArray.ToArray(), 0, sixthArray.ToList().Count, totalNumbers /** 6*/);

            var arrayProcessorSeventh = new ArrayProcessor(seventhArray.ToArray(), 0, seventhArray.ToList().Count, totalNumbers /** 7*/);
            var arrayProcessorLast = new ArrayProcessor(lastArray.ToArray(), 0, lastArray.ToList().Count, totalNumbers /** 8*/);
            #endregion

            #region Threads
            Thread firstThread = new Thread(arrayProcessorFirst.CalculateDivisor);
            Thread secondThread = new Thread(arrayProcessorSecond.CalculateDivisor);
            Thread thirdThread = new Thread(arrayProcessorThird.CalculateDivisor);
            Thread fourthThread = new Thread(arrayProcessorFourth.CalculateDivisor);
            Thread fifthThread = new Thread(arrayProcessorFifth.CalculateDivisor);
            Thread sixthThread = new Thread(arrayProcessorSixth.CalculateDivisor);
            Thread seventhThread = new Thread(arrayProcessorSeventh.CalculateDivisor);
            Thread lastThread = new Thread(arrayProcessorLast.CalculateDivisor);
            #endregion

            #region Start
            firstThread.Start();
            secondThread.Start();
            thirdThread.Start();

            fourthThread.Start();
            fifthThread.Start();
            sixthThread.Start();

            seventhThread.Start();
            lastThread.Start();
            #endregion

            Console.WriteLine("Please wait...");

            #region Join
            lastThread.Join();
            seventhThread.Join();
            
            sixthThread.Join();
            fifthThread.Join();
            fourthThread.Join();

            thirdThread.Join();
            secondThread.Join();
            firstThread.Join();
            #endregion

            #region group
            var totalSum = arrayProcessorFirst.results;
            totalSum.AddRange(arrayProcessorSecond.results);
            totalSum.AddRange(arrayProcessorThird.results);
            totalSum.AddRange(arrayProcessorFourth.results);
            totalSum.AddRange(arrayProcessorFifth.results);
            totalSum.AddRange(arrayProcessorSixth.results);
            totalSum.AddRange(arrayProcessorSeventh.results);
            totalSum.AddRange(arrayProcessorLast.results);
            #endregion

            stopwatch.Stop();

            Console.WriteLine($"Elapsed time: {stopwatch.Elapsed.TotalMilliseconds} ms");
            var result = totalSum.First(n=> n.Amount == totalSum.Max(m=> m.Amount));

            Console.WriteLine($"The most number is : {result.MostNumber} with {result.Amount} times.");

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
