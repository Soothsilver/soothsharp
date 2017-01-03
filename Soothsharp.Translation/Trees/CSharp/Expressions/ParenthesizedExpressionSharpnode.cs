using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class ParenthesizedExpressionSharpnode : ExpressionSharpnode
    {
        public ExpressionSharpnode InnerExpression;

        public ParenthesizedExpressionSharpnode(ParenthesizedExpressionSyntax syntax) : base(syntax)
        {
            InnerExpression = RoslynToSharpnode.MapExpression(syntax.Expression);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var expressionResult = InnerExpression.Translate(context);
            return TranslationResult.FromSilvernode(
                new ParenthesizedExpressionSilvernode(expressionResult.Silvernode as ExpressionSilvernode, OriginalNode),
                expressionResult.Errors);
        }
    }
}