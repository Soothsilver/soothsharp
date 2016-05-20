//SUCCEEDS
using Sharpsilver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.TranslationTests.Files
{
    [Verified]
    static class For
    {
        static int ForCycle()
        {
            Contract.Ensures(Contract.IntegerResult == 10);

            int j = 0;
            for (int i = 0; i < 10; i = i + 1)
            {
                Contract.Invariant(i <= 10);
                Contract.Invariant(i == j);
                j = j + 1;
            }
            return j;
        }
    }
}
