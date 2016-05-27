using Sharpsilver.Contracts;

namespace Sharpsilver.Translation.Tests.Files.Basic
{
    static class ScalaPaperExample
    {
        public static int Sum(int n)
        {
            Contract.Requires(n >= 0);
            Contract.Ensures(Contract.IntegerResult == n*(n + 1)/2);
            var i = 0;
            var s = 0;
            while (i < n)
            {
                Contract.Invariant(s == i*(i + 1)/2);
                Contract.Invariant(i <= n);
                i += 1;
                s += i;
            }
            return s;
        }
    }
}
