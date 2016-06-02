//SUCCEEDS
using Sharpsilver.Contracts;
using AlternativeNameForContract = Sharpsilver.Contracts.Contract;

namespace Sharpsilver.TranslationTests.Files
{
    static class Invariants
    {
        public static int sum(int n)
        {
            Sharpsilver.Contracts.Contract.Requires(n > 0);
            Contract.Ensures(AlternativeNameForContract.IntegerResult == n * (n + 1) / 2);
            int i = 0;
            int s = 0;
            while (i < n)
            {
                AlternativeNameForContract.Invariant(s == i * (i + 1) / 2);
                AlternativeNameForContract.Invariant(i <= n);
                i++;
                s++;
            }
            return s;
        }
    }
}
