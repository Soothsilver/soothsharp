using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements
{
    class ErrorStatementSilvernode : StatementSilvernode
    {
        public ErrorStatementSilvernode(SyntaxNode node) : base(node)
        {

        }

        protected override IEnumerable<Silvernode> Children { get; } = new Silvernode[0];

        public override string ToString()
        {
            return "!ERROR!";
        }
    }
}
