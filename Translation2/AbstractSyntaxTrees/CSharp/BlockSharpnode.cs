using System;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class BlockSharpnode : Sharpnode
    {
        public BlockSharpnode(SyntaxNode originalNode) : base(originalNode)
        {
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            throw new NotImplementedException();
        }
    }
}