using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Simple
{
    class NewlineSilvernode : Silvernode
    {
        public NewlineSilvernode() : base(null)
        {
        }

        public override string ToString()
        {
            return "\n";
        }
    }
}
