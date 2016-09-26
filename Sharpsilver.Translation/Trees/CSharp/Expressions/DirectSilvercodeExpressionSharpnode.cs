using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.CSharp.Expressions
{
    public class DirectSilvercodeExpressionSharpnode : ExpressionSharpnode
    {
        public string Code;
        public DirectSilvercodeExpressionSharpnode(string code, ExpressionSyntax originalNode) : base(originalNode)
        {
            this.Code = code;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            return TranslationResult.FromSilvernode(Code);
        }
    }
}