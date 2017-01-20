using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Csverify.Examples
{
    class Test
    {
        static void AA()
        {
            int[] pole = new[] {2, 4, 6};
            Contract.Assert(pole[1] == 4);
        }
        
    }
}
