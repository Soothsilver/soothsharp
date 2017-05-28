using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Translation.Tests.Systemwide.syntax
{
    class Tabs
    {
        int a()
        {
            bool b = true;
            while (b)
            {
                int u = 4;
                if (u == 5)
                {
                    Assert(false);
                }
                else
                {
                    Assert(true);
                }
                return 2;
            }
            return 4;
        }
    }
}
