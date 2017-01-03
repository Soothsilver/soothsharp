using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    /// <summary>
    /// Contains several statement silvernodes that are separated by newlines.
    /// </summary>
    /// <seealso cref="StatementSilvernode" />
    public class StatementsSequenceSilvernode : StatementSilvernode
    {
        public StatementsSequenceSilvernode(SyntaxNode node, params StatementSilvernode[] nodes) : base(node)
        {
            List.AddRange(nodes);
        }

        public override IEnumerable<Silvernode> Children => List.WithSeparator<Silvernode>("\n" + Tabs());

        public List<StatementSilvernode> List = new List<StatementSilvernode>();

        protected override void OptimizePre()
        {
            List.RemoveAll(sn => String.IsNullOrEmpty(sn.ToString()));
        }
    }
}
