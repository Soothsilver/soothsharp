// expect SSIL204

using Soothsharp.Contracts;

// ReSharper disable BuiltInTypeReferenceStyle

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.basic
{
    [Verified]
    internal static class Simple
    {
        public static void Test()
        {            
            Simple.Test2(-5);
        }

        public static System.Int32 Test2(int x)
        {
            Contract.Requires(x > 0);
            Contract.Ensures(Contract.IntegerResult == x * x);

            return x * x;
        }
    }
}
