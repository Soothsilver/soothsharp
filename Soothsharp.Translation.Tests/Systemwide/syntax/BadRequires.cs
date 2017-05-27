// expect SSIL129
// expect SSIL129
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Syntax.Contracts
{
    class BadRequires
    {
        int a()
        {
            int c = 3;
            while (c == 2)
            {
                // This cannot be in a loop or inner block.
                Contract.Requires(2 != 3);
            }
            return 3;
        }
        int b()
        {
            int c = 3;
            while (c == 2)
            {
                // This cannot be in a loop or inner block.
                Contract.Ensures(2 != 3);
            }
            return 3;
        }
    }
}
