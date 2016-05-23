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

            int res = 0;
            for (int i = 0; i < 10; i++)
            {
                Contract.Invariant(i <= 10);
                Contract.Invariant(i == res);

                res++;
            }
            return res;
        }
    }
}
