using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Soothsharp.Translation.Trees.CSharp
{
    class UnknownSharpnode : Sharpnode
    {
        public UnknownSharpnode(SyntaxNode node) : base(node)
        {
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return TranslationResult.Error(OriginalNode, Diagnostics.SSIL101_UnknownNode, OriginalNode.Kind());
        }
    }
}
