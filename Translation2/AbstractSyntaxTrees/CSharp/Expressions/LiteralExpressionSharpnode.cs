using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using Microsoft.CodeAnalysis.CSharp;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions
{
    public class LiteralExpressionSharpnode : ExpressionSharpnode
    {
        private LiteralExpressionSyntax literalExpressionSyntax;
        private LiteralKind Kind;
        private bool booleanValue;

        public LiteralExpressionSharpnode(LiteralExpressionSyntax literalExpressionSyntax, bool literalValue) : base(literalExpressionSyntax)
        {
            this.literalExpressionSyntax = literalExpressionSyntax;
            this.booleanValue = literalValue;
            this.Kind = LiteralKind.Boolean;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            Silvernode sn = null;
            switch(Kind)
            {
                case LiteralKind.Boolean:
                    if (booleanValue)
                        sn = new TextSilvernode("true", OriginalNode);
                    else
                        sn = new TextSilvernode("false", OriginalNode);
                    break;
            }
            if (sn != null)
                return TranslationResult.Silvernode(sn);
            else
                return TranslationResult.Error(OriginalNode, Diagnostics.SSIL101_UnknownNode, OriginalNode.Kind());
        }
    }
    enum LiteralKind
    {
        Boolean,
        Int32,
        Null
    }
}