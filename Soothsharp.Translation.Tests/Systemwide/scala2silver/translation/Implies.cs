using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.translation
{
    class Implies
    {
        public void Main(bool b)
        {
            Contract.Assert((!b).Implies(!b));
            Contract.Assert(!(true.Implies(false)));
            Contract.Assert(true.Implies(true));
            Contract.Assert(false.Implies(false));
            Contract.Assert(false.Implies(true));
            Contract.Assert(Fun(2, 1).Implies(Fun(1, 2)));
            Contract.Assert(!(Fun(1, 2).Implies(Fun(2, 1))));
            // expect SSIL204 at next
            Contract.Assert(true.Implies(false));
        }

        [Pure]
        public bool Fun(int a, int b)
        {
            return a < b;
        }
    }
}
