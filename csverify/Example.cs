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
        public void test()
        {
            Seq<bool> seq = new Seq<bool>();
            Assert(seq.Contains(true));
            Assert(seq[2] == false);
        }
    }
}
