// expect SSIL204

using Sharpsilver.Contracts;

// ReSharper disable BuiltInTypeReferenceStyle

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.basic
{
    [Verified]
    internal static class Simple
    {
        public static void Test()
        {            
            Test2(-5);
        }

        public static global::System.Int32 Test2(int x)
        {
            Contract.Requires(x > 0);
            Contract.Ensures(Contract.IntegerResult == x * x);

            return x * x;
        }
    }
}
