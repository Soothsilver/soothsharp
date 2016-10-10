using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using Microsoft.CodeAnalysis.CSharp;

namespace Sharpsilver.Translation.Trees.CSharp.Expressions
{
    public class LiteralExpressionSharpnode : ExpressionSharpnode
    {
        private LiteralExpressionSyntax literalExpressionSyntax;
        private LiteralKind Kind;
        private bool booleanValue;
        private int integerValue;

        public LiteralExpressionSharpnode(LiteralExpressionSyntax literalExpressionSyntax, int literalValue) : base(literalExpressionSyntax)
        {
            this.integerValue = literalValue;
            this.Kind = LiteralKind.Int32;
            this.literalExpressionSyntax = literalExpressionSyntax;
        }
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
                case LiteralKind.Int32:
                    sn = new TextSilvernode(integerValue.ToString(), OriginalNode);
                    break;
            }
            if (sn != null)
                return TranslationResult.FromSilvernode(sn);
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