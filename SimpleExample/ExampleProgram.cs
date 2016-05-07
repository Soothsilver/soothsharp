using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;

namespace Sharpsilver.Examples.SimpleExample
{
    class ExampleProgram
    {
        [Verified]
        static int Maximum(int a, int b)
        {
            Contract.Ensures(a >= b ? a == Contract.IntegerResult : b == Contract.IntegerResult);
            Contract.Ensures(true);


            if (a >= b)
                return a;
            else
                return b;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Maximum of 2 and 5 is: " + Maximum(2, 5));
        }
    }
}
