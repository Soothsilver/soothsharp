using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class ExpressionStatementSilvernode : StatementSilvernode
    {
        private Silvernode expression;

        public ExpressionStatementSilvernode(Silvernode expression, SyntaxNode originalNode)
            : base(originalNode)
        {
            this.expression = expression;
        }

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return this.expression;
            }
        }
    }
}