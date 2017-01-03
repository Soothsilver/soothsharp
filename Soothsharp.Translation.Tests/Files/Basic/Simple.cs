//FAILS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.TranslationTests.Files
{
    [Verified]
    static class Simple
    {
        static void test()
        {            
            test2(-5);
        }
        static System.Int32 test2(int x)
        {
            Contract.Requires(x > 0);
            Contract.Ensures(Contract.IntegerResult == x * x);

            return x * x;
        }
    }
}
