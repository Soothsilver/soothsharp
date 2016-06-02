using System;
using Sharpsilver.Contracts;

namespace Sharpsilver.Examples
{
    [Verified]
    static class ExampleProgram
    {
        [Verified]
        static System.Int32 Maximum(int a, int b)
        {
            Contract.Ensures(a >= b ? a == Contract.IntegerResult : b == Contract.IntegerResult);
            Contract.Ensures(true); // <- just testing           

            if (a >= b)
                return a;
            else
                return b;
        }

        [Unverified]
        static void Main(string[] args)
        {
            Console.WriteLine("Maximum of 2 and 5 is: " + Maximum(2, 5));
        }
    }
}

