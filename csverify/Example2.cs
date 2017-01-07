using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Csverify
{
    class AA
    {
       public int Field;
    }
    class Example
    {
        public static bool NeedAccess(AA a)
        {
            a.Field = 8;
            return a.Field == 4;
        }
        public static void test()
        {
            Soothsharp.Contracts.Contract.Assert(true);
            AA a = new Csverify.AA();
            Contracts.Contract.Assert(NeedAccess(a));
        }
    }
}
