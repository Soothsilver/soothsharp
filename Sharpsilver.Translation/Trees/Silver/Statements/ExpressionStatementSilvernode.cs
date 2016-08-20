using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class ExpressionStatementSilvernode : Silvernode
    {
        private Silvernode Expression;

        public ExpressionStatementSilvernode(Silvernode expression, SyntaxNode originalNode)
            : base(originalNode)
        {
            this.Expression = expression;
        }

        public override string ToString()
        {
            return Expression.ToString() + ";";
        }
    }
}