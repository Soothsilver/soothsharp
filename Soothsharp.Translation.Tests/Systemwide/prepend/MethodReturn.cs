using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.prepend
{
    class MethodReturn
    {
        int a()
        {
            Contract.Ensures(Contract.IntegerResult == 3);
            return b() + 1;
        }

        [Abstract]
        int b()
        {
            Contract.Ensures(Contract.IntegerResult == 2);
            return 4;
        }
    }
}
