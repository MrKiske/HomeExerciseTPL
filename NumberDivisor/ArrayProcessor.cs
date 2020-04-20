using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NumberDivisor
{
    public class ArrayProcessor
    {
        private readonly int[] array;
        private readonly int nrOfElementsToProcess;
        private readonly int startIndex;

        public ArrayProcessor(int[] array, int startIndex, int nrOfElementsToProcess)
        {
            this.Sum = 0;

            this.array = array;
            this.startIndex = startIndex;
            this.nrOfElementsToProcess = nrOfElementsToProcess;
        }

        public BigInteger Sum { get; private set; }

        public void CalculateDivisor()
        {
            try
            {
                for (int i = 0; i <= array.Length - 1; i++)
                {
                    int number = array[i];
                    if (number != 1)
                    { 

                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write($"Error: {ex.Message}");
            }

        }
    }
}