// ReSharper disable All
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples
{
    class A
    {
        public static int Maximum(int a, int b)
        {
            Ensures(a > b ? IntegerResult == a : IntegerResult == b);
            return a;
        }

    }
}
