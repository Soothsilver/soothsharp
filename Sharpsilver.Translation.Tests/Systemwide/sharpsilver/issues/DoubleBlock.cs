using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Tests.Systemwide.sharpsilver.issues
{
    class DoubleBlock
    {
        public static void main()
        {
            int a = 1;
            {
                {
                    {
                        a = 2 + 2;
                    }
                }
            }
            Contracts.Contract.Assert(a == 4);
        }
    }
}
