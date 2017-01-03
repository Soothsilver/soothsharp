using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    /// <summary>
    /// The error statement is syntactically incorrect in Silver in all circumstances and is used as the placeholder for sharpnodes that
    /// were not successfully translated.
    /// </summary>
    /// <seealso cref="StatementSilvernode" />
    class ErrorStatementSilvernode : StatementSilvernode
    {
        public ErrorStatementSilvernode(SyntaxNode node) : base(node)
        {

        }

        public override IEnumerable<Silvernode> Children => new Silvernode[] { Constants.SilverErrorString };
    }
}
