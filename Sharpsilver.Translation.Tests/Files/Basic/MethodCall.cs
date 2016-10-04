using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;

namespace Sharpsilver.Translation.Tests.Files.Basic
{
    class MethodCall
    {
        static void a()
        {
            b();
        }
        static void b()
        {

        }
    }
}
