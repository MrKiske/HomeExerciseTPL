using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazUI
{
   
    public static class StringExtensions
    {
        public static double ToDouble(this string input)
        {
            return double.Parse(input);
        }

        public static int ToInteger(this string input)
        {
            return int.Parse(input);
        }
    }
}
