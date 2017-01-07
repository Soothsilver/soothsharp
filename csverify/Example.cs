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
  
    enum S
    {
        A,
        B = 4,
        C
    }
    class T2
    {
        const bool testbool = false;
        public static void test()
        { 
            if (testbool)
            {
                int a = 5;
            }
            else
            {
                int b = 6;
            }
        }
    }
}
