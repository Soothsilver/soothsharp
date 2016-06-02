using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    abstract class ExpressionSilvernode : Silvernode
    {
        public SilverType Type { get; private set; }

        protected ExpressionSilvernode(SyntaxNode node, SilverType type) : base(node)
        {
            Type = type;
        }
    }
}
