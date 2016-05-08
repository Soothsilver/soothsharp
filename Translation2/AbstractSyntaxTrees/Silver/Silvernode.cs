using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public abstract class Silvernode
    {
        public SyntaxNode OriginalNode;
        protected Silvernode(SyntaxNode originalNode)
        {
            OriginalNode = originalNode;
        }
    }
}
