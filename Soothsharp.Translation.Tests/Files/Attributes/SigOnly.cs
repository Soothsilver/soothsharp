using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Files.Attributes
{
    [SignatureOnly]
    class SigOnly
    {
        public static int Return4()
        {
            return 4;
        }
    }
    class Full
    {
        [SignatureOnly]
        public static int Return5()
        {
            return 5;
        }
    }
}
