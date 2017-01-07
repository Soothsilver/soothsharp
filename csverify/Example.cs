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
        public static void test()
        {
            Seq<bool> a = null;
            a = a + a;
        }
    }
}
