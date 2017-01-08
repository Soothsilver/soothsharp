using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Files.Basic
{
    class MethodCall
    {
        static void a()
        {
            MethodCall.b();
        }
        static void b()
        {

        }
    }
}
