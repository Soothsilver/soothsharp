//SUCCEEDS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.TranslationTests.Files
{
    [Verified]
    static class Implications
    {
        static void Impl(bool a)
        {
            Requires(a.Implies(true));
              
            bool b = a.Implies(M());
        }
        static Boolean M()
        {
            return true;
        }
    }
}
