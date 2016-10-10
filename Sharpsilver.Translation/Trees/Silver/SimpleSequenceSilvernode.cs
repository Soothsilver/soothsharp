using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    class SimpleSequenceSilvernode : ComplexSilvernode
    {
        public List<Silvernode> List;

        public SimpleSequenceSilvernode(SyntaxNode originalNode, params Silvernode[] topLevelSilvernodes)
            : base(originalNode)
        {
            List = new List<Silvernode>(topLevelSilvernodes);
        }

        public override IEnumerable<Silvernode> Children
        {
            get { return List.SelectMany(s => new[] { s }); }
        }
    }
}
