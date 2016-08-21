using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    class NewSilvernode : ExpressionSilvernode
    {
        public NewSilvernode(
            SyntaxNode originalNode) : base(originalNode, SilverType.Ref)
        {

        }

        public override string ToString()
        {
            return "new()";
        }
    }
}