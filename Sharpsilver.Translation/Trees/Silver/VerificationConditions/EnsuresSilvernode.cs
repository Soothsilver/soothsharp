using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Trees.Silver
{
    class EnsuresSilvernode : VerificationConditionSilvernode
    {
        public Silvernode Postcondition;

        public EnsuresSilvernode(Silvernode postcondition, SyntaxNode originalNode) : base(originalNode)
        {
            Postcondition = postcondition;
        }

        public override int CompareTo(VerificationConditionSilvernode other)
        {
            if (other.GetType() == typeof(EnsuresSilvernode))
                return 0;
            else
                return 1;
        }
        
        public override string ToString()
        {
            return "ensures (" + Postcondition + ")";
        }
    }
}
