using System;
using Sharpsilver.Contracts;

namespace Sharpsilver.Examples.SimpleExample
{
    [Verified]
    static class ExampleProgram
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

    [Unverified]
    class DifficultProgram
    {
        static int propa { get; }
        static int a()
        {
            return 4;
        }
    }
}

