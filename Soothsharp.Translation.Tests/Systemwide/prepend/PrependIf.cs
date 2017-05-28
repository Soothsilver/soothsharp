using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.prepend
{
    class PrependIf
    {
        int a()
        {
            Contract.Ensures(Contract.IntegerResult == 2);
            if (b() + 6 == 8)
            {
                return 2;
            }
            else
            {
                return 3;
            }
        }

        [Abstract]
        int b()
        {
            Contract.Ensures(Contract.IntegerResult == 2);
            return 4;
        }
    }
}
