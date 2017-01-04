// ReSharper disable All
using static Soothsharp.Contracts.Contract;
using static Soothsharp.Contracts.Permission;

namespace Soothsharp.Examples
{
    class Data
    {
        public int Value;
    }
    class C
    {
        public static int ReadValue(Data d)
        {
            Requires(Acc(d.Value, Wildcard));

            return d.Value;
        }
        public static int ReadValueError(Data d)
        {
            Requires(Acc(d.Value, Wildcard));

            d.Value = 3; // <= This will trigger an error.
            return d.Value;
        }
    }
}
