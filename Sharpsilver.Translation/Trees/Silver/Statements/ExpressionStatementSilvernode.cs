using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class ExpressionStatementSilvernode : StatementSilvernode
    {
        private Silvernode Expression;

        public ExpressionStatementSilvernode(Silvernode expression, SyntaxNode originalNode)
            : base(originalNode)
        {
            this.Expression = expression;
        }

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return Expression;
                yield return ";";
            }
        }
    }
}