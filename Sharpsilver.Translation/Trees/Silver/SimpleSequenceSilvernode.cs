using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreLinq;
using Sharpsilver.Translation.Trees.Silver.Simple;

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
            get { return List.SelectMany(s => new Silvernode[] {s }); }
        }
    }
}
