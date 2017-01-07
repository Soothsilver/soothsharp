using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Csverify
{
    class T
    {
       public int Field;
    }
    class T2
    {
        public static void NeedAccess(T a)
        {
            Contract.Requires(Acc(a.Field, Permission.Create(1,3)));
            Ensures(Acc(a.Field, Permission.Create(1, 3)));
            int rd = a.Field;
        }
        public static void test()
        {
            T a = new Csverify.T();
            NeedAccess(a);
            Assert(Permission.FromLocation(a.Field) == Permission.Write);
        }
    }
}
