// expect SSIL130
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Syntax.Contracts
{
    class BadInvariant
    {
        void a()
        {
            Contract.Invariant(2 == 3);
        }
    }
}
