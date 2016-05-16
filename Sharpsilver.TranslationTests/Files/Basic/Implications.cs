//SUCCEEDS
using Sharpsilver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sharpsilver.Contracts.Contract;

namespace Sharpsilver.TranslationTests.Files
{
    [Verified]
    static class Implications
    {
        static void impl(bool a)
        {
            Requires(a.Implies(true));
              
            bool b = a.Implies(m());
        }
        static Boolean m()
        {
            return true;
        }
    }
}
