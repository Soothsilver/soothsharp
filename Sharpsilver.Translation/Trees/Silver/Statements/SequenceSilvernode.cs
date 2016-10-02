using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Statements
{
    /// <summary>
    /// Contains several statement silvernodes that are separated by newlines.
    /// </summary>
    /// <seealso cref="Sharpsilver.Translation.Trees.Silver.Statements.StatementSilvernode" />
    public class SequenceSilvernode : StatementSilvernode
    {
        public SequenceSilvernode(SyntaxNode node, params StatementSilvernode[] nodes) : base(node)
        {
            List.AddRange(nodes);
        }

        public override IEnumerable<Silvernode> Children => List.WithSeparator<Silvernode>("\n" + Tabs());

        public List<StatementSilvernode> List = new List<StatementSilvernode>();

        protected override void Optimize()
        {
            List.RemoveAll(sn => String.IsNullOrEmpty(sn.ToString()));
        }
    }
}
