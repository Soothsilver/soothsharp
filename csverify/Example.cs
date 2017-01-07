using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
// ReSharper disable All

namespace Soothsharp.Csverify
{
  
    class T2
    {
        Seq<bool> a;
        public void test()
        {
           Inhale(Acc(this.a));

           Assume(a.Length > 1);
           Assume(a.TakeDrop(1,2)[0] == false);
            Assert(a.Contains(false));
        }
    }
}
