using Soothsharp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Examples.Multifile
{
    class Multiple_Main
    {
        static void Primary()
        {
            SideClass sideClass = new Multifile.SideClass(10);
            int b = sideClass.A*2;
            Contract.Assert(b == 40);
        }
    }
}
