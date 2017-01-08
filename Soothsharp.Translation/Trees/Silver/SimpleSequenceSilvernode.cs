using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class SimpleSequenceSilvernode : ComplexSilvernode
    {
        public List<Silvernode> List;

        public SimpleSequenceSilvernode(SyntaxNode originalNode, params Silvernode[] topLevelSilvernodes)
            : base(originalNode)
        {
            this.List = new List<Silvernode>(topLevelSilvernodes);
        }

        protected override IEnumerable<Silvernode> Children
        {
            get { return this.List.SelectMany(s => new[] { s }); }
        }
    }
}
