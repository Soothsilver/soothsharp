using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;
using static Sharpsilver.Contracts.Contract;

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.translation
{
    class Implies
    {
        public void Main(bool b)
        {
            Assert(!b.Implies(!b));
            Assert(!(true.Implies(false)));
            Assert(true.Implies(true));
            Assert(false.Implies(false));
            Assert(false.Implies(true));
            Assert(Fun(2, 1).Implies(Fun(1, 2)));
            Assert(!(Fun(1, 2).Implies(Fun(2, 1))));
            // expect SSIL204 at next
            Assert(true.Implies(false));
        }

        [Pure]
        public bool Fun(int a, int b)
        {
            return a < b;
        }
    }
}
