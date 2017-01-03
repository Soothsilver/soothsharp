using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class LiteralExpressionSharpnode : ExpressionSharpnode
    {
        private LiteralKind kind;
        private bool booleanValue;
        private int integerValue;

        public LiteralExpressionSharpnode(LiteralExpressionSyntax literalExpressionSyntax, int literalValue) : base(literalExpressionSyntax)
        {
            this.integerValue = literalValue;
            this.kind = LiteralKind.Int32;
        }
        public LiteralExpressionSharpnode(LiteralExpressionSyntax literalExpressionSyntax, bool literalValue) : base(literalExpressionSyntax)
        {
            this.booleanValue = literalValue;
            this.kind = LiteralKind.Boolean;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            Silvernode sn = null;
            switch(this.kind)
            {
                case LiteralKind.Boolean:
                    sn = this.booleanValue ? new TextSilvernode("true", this.OriginalNode) : new TextSilvernode("false", this.OriginalNode);
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