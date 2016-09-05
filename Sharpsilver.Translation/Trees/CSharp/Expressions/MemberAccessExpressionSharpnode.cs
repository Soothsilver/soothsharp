using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Sharpsilver.Translation.Trees.CSharp;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation
{
    internal class MemberAccessExpressionSharpnode : ExpressionSharpnode
    {
        public MemberAccessExpressionSyntax Expression;

        public MemberAccessExpressionSharpnode(MemberAccessExpressionSyntax syntax) : base(syntax)
        {
            Expression = syntax;
        }


        public override TranslationResult Translate(TranslationContext context)
        {
            return null;
        }
    }
}