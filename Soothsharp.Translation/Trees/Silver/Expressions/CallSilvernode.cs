using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class CallSilvernode : ExpressionSilvernode
    {
        private List<Silvernode> expressions;
        private Identifier identifier;

        public CallSilvernode(
            Identifier identifier, 
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