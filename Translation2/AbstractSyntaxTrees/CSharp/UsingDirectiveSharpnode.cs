using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class UsingDirectiveSharpnode : Sharpnode
    {
        private UsingDirectiveSyntax uds;

        public UsingDirectiveSharpnode(UsingDirectiveSyntax uds) : base(uds)
        {
            this.uds = uds;
        }
        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return TranslationResult.Silvernode(new EmptySilvernode(uds));
        }
    }
}