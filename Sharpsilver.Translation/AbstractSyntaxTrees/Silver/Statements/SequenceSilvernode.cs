using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements
{
    public class SequenceSilvernode : StatementSilvernode
    {
        public SequenceSilvernode(SyntaxNode node, params StatementSilvernode[] nodes) : base(node)
        {
            List.AddRange(nodes);
        }

        protected override IEnumerable<Silvernode> Children => List.WithSeparator<Silvernode>("\n" + Tabs());

        public List<StatementSilvernode> List = new List<StatementSilvernode>();
    }
}
