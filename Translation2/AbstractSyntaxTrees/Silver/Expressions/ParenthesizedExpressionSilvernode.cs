using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class ParenthesizedExpressionSilvernode : ExpressionSilvernode
    {
        private ExpressionSilvernode Expression;

        public ParenthesizedExpressionSilvernode(
            ExpressionSilvernode silvernode,
            SyntaxNode originalNode) : 
            base(originalNode, silvernode.Type)
        {
            this.Expression = silvernode;
        }

        public override string ToString()
        {
            return "(" + Expression + ")";
        }
    }
}