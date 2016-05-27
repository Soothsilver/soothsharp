using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;
using static Sharpsilver.Contracts.Contract;

namespace Sharpsilver.Translation.Tests.Files.Mathematical
{
    class Theorems
    {
        [Verified]
        public static void TwoLeafTheorem(Graph g)
        {
            Requires(g != null);
            // ...
        }

        public static void GroupDivisibility(object group)
        {
            // ...
        }
    }

    class Graph
    {
        
    }
}
