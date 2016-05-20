using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class CallSilvernode : ExpressionSilvernode
    {
        private List<Silvernode> expressions;
        private string identifier;

        public CallSilvernode(
            string identifier, 
            List<Silvernode> expressions, 
            SilverType returnType,
            SyntaxNode originalNode) : base(originalNode, returnType)
        {
            this.identifier = identifier;
            this.expressions = expressions;
        }

        public override string ToString()
        {
            return identifier + " (" + String.Join(", ", expressions) + ")";
        }
    }
}