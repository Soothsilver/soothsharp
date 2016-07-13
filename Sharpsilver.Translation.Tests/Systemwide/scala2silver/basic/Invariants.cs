﻿using Sharpsilver.Contracts;
using AlternativeNameForContract = Sharpsilver.Contracts.Contract;

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.basic
{
    internal static class Invariants
    {
        public static int Sum(int n)
        {
            Contract.Requires(n > 0);
            Contract.Ensures(AlternativeNameForContract.IntegerResult == n * (n + 1) / 2);
            int i = 0;
            int s = 0;
            while (i < n)
            {
                AlternativeNameForContract.Invariant(s == i * (i + 1) / 2);
                AlternativeNameForContract.Invariant(i <= n);
                i++;
                s += i;
            }
            return s;
        }
    }
}
