// ReSharper disable All
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples
{
    class A2
    {
        public static int Maximum(int a, int b)
        {
            Ensures(a > b ? IntegerResult == a : IntegerResult == b);
            if (a >= b)
                return a;
            else
                return b;
        }
    }
}
