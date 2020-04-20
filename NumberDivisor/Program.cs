using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
