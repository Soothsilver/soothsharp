using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public abstract class ComplexSilvernode : Silvernode
    {
        protected ComplexSilvernode(SyntaxNode node) : base(node)
        {
            
        }

        protected abstract override IEnumerable<Silvernode> Children { get; }
        public override string ToString()
        {
            return String.Join("", Children);
        }
    }
}
