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
        public SyntaxToken OriginalToken;
        public SyntaxTrivia OriginalTrivia;

        public virtual bool IsVerificationCondition()
        {
            return false;
        }

        protected Silvernode(SyntaxNode originalNode)
        {
            OriginalNode = originalNode;
        }
        protected Silvernode(SyntaxToken originalToken)
        {
            OriginalToken = originalToken;
        }
        protected Silvernode(SyntaxTrivia originalTrivia)
        {
            OriginalTrivia = originalTrivia;
        }

        public abstract override string ToString();
    }
}
