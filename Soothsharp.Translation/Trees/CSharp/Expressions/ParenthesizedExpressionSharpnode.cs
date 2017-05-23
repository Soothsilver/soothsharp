using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class ParenthesizedExpressionSharpnode : ExpressionSharpnode
    {
        private readonly ExpressionSharpnode InnerExpression;

        public ParenthesizedExpressionSharpnode(ParenthesizedExpressionSyntax syntax) : base(syntax)
        {
            this.InnerExpression = RoslynToSharpnode.MapExpression(syntax.Expression);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var expressionResult = this.InnerExpression.Translate(context);
            if (expressionResult.Silvernode is ExpressionSilvernode)
            {
                return TranslationResult.FromSilvernode(
                    new ParenthesizedExpressionSilvernode((ExpressionSilvernode) expressionResult.Silvernode,
                        this.OriginalNode),
                    expressionResult.Errors);
            }
            else
            {
                return TranslationResult.FromSilvernode(
                     expressionResult.Silvernode,
                    expressionResult.Errors);
            }
        }
    }
}