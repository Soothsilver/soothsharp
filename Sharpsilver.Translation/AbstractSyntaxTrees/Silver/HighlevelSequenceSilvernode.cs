using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MoreLinq;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Simple;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class HighlevelSequenceSilvernode : ComplexSilvernode
    {
        public List<Silvernode> List;

        public HighlevelSequenceSilvernode(SyntaxNode originalNode, params Silvernode[] topLevelSilvernodes)
            : base(originalNode)
        {
            List = new List<Silvernode>(topLevelSilvernodes);
        }
        
        protected override IEnumerable<Silvernode> Children
        {
            get { return List.SelectMany(s => new Silvernode[] {s, new NewlineSilvernode()}); }
        }
    }
}
