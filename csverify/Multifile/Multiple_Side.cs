using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples.Multifile
{
    class SideClass
    {
        public int A;
        public SideClass(int a)
        {
            Ensures(Acc(this.A) && this.A == a*2);
            A = a*2;
        }
    }
}
