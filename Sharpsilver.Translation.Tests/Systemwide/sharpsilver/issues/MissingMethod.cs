using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;

namespace Sharpsilver.Translation.Tests.Files.Issues
{
    class MissingMethod
    {
        static void a()
        {
            // expect SSIL204
            b();
        }
        [Unverified]
        static void b()
        {

        }
    }
}
