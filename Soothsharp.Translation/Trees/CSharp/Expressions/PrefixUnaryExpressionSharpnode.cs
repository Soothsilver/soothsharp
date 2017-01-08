using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class PrefixUnaryExpressionSharpnode : ExpressionSharpnode
    {
        private string Operator;
        private ExpressionSharpnode Expression;

        public PrefixUnaryExpressionSharpnode(PrefixUnaryExpressionSyntax syntax, string @operator) : base(syntax)
        {
            this.Operator = @operator;
            this.Expression = RoslynToSharpnode.MapExpression(syntax.Operand);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var left = this.Expression.Translate(context);
            return TranslationResult.FromSilvernode(new PrefixUnaryExpressionSilvernode(this.Operator, left.Silvernode, this.OriginalNode), left.Errors);
        }
    }
}