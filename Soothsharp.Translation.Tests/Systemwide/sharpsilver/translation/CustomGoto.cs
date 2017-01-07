using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.soothsharp.translation
{
    class CustomGoto
    {
        public static void main()
        {
            hello:;
            int a = 4;
            Contract.Assert(a == 4);
            if (a == 2) goto hi;
            goto hello;
            hi:
            Contract.Assert(false);
        }
    }
}
