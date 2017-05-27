using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Syntax.Contracts
{
    class DoStatement
    {
        void a()
        {
            int a = 1;
            do
            {
                Contract.Invariant(a <= 3);
                a++;
            }
            while (a != 3);

            Contract.Assert(a == 3);
        }
    }
}
