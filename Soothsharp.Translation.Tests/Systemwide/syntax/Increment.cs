using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Translation.Tests.Systemwide.syntax
{
    class Increment
    {
        void a()
        {
            int b = 2;
            int c = (3 + b++);
            c++;
        }
    }
}
