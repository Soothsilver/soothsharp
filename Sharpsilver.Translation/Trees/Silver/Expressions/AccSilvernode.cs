using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Trees.Silver
{
    class AccSilvernode : ExpressionSilvernode
    {
        public Silvernode ProtectedElement;
        public Silvernode Permission;

        public AccSilvernode(Silvernode protectedElement, Silvernode permission, SyntaxNode originalNode) :
            base(originalNode, SilverType.Bool)
        {
            ProtectedElement = protectedElement;
            Permission = permission;
        }

        public override string ToString()
        {
            return "acc(" + ProtectedElement + ", " + Permission + ")";
        }
    }
}
