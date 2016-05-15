using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public class SequenceSilvernode : Silvernode
    {
        public SequenceSilvernode(SyntaxNode node, params Silvernode[] nodes) : base(node)
        {
            List.AddRange(nodes);
        }

        public override string ToString()
        {
            return String.Join("\n", List.Select(sn => sn.ToString()));
        }

        public List<Silvernode> List = new List<Silvernode>();
    }
}
