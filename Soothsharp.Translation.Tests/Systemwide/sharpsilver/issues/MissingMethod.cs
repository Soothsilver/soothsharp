using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Files.Issues
{
    class MissingMethod
    {
        static void a()
        {
            // expect SSIL204
            MissingMethod.b();
        }
        [Unverified]
        static void b()
        {

        }
    }
}
