using Soothsharp.Contracts;
using AlternativeNameForContract = Soothsharp.Contracts.Contract;

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.basic
{
    internal static class Invariants
    {
        public static int Sum(int n)
        {
            Contract.Requires(n > 0);
            Contract.Ensures(Contract.IntegerResult == n * (n + 1) / 2);
            int i = 0;
            int s = 0;
            while (i < n)
            {
                Contract.Invariant(s == i * (i + 1) / 2);
                Contract.Invariant(i <= n);
                i++;
                s += i;
            }
            return s;
        }
    }
}
