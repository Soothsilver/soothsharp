using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class EnsuresSilvernode : Silvernode
    {
        public Silvernode Postcondition;

        public EnsuresSilvernode(Silvernode postcondition, SyntaxNode originalNode) : base(originalNode)
        {
            Postcondition = postcondition;
        }

        public override bool IsVerificationCondition()
        {
            return true;
        }
        public override string ToString()
        {
            return "ensures (" + Postcondition + ")";
        }
    }
}
