using System;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class TypeSharpnode : Sharpnode
    {
        public TypeSharpnode(SyntaxNode originalNode) : base(originalNode)
        {
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            throw new NotImplementedException();
        }
    }
}