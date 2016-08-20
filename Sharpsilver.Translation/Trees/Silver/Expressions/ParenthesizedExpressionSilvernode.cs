using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    class ParenthesizedExpressionSilvernode : ExpressionSilvernode
    {
        private readonly ExpressionSilvernode expression;

        public ParenthesizedExpressionSilvernode(
            ExpressionSilvernode silvernode,
            SyntaxNode originalNode) : 
            base(originalNode, silvernode?.Type ?? SilverType.Error)
        {
            expression = silvernode;
        }

        public override string ToString()
        {
            return "(" + expression + ")";
        }
    }
}