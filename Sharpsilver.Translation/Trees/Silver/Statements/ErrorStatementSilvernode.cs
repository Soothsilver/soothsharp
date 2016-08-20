using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements
{
    /// <summary>
    /// The error statement is syntactically incorrect in Silver in all circumstances and is used as the placeholder for sharpnodes that
    /// were not successfully translated.
    /// </summary>
    /// <seealso cref="Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements.StatementSilvernode" />
    class ErrorStatementSilvernode : StatementSilvernode
    {
        public ErrorStatementSilvernode(SyntaxNode node) : base(node)
        {

        }

        protected override IEnumerable<Silvernode> Children => new Silvernode[] { Constants.SilverErrorString };
    }
}
