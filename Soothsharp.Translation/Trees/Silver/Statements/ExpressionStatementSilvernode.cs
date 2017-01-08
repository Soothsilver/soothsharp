using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class ExpressionStatementSilvernode : StatementSilvernode
    {
        private Silvernode expression;

        public ExpressionStatementSilvernode(Silvernode expression, SyntaxNode originalNode)
            : base(originalNode)
        {
            this.expression = expression;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return this.expression;
            }
        }
    }
}