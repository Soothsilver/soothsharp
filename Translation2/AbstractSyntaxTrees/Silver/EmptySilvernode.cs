using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public class EmptySilvernode : Silvernode
    {
        public EmptySilvernode(SyntaxNode node) : base(node)
        {

        }

        public override string ToString()
        {
            return "";
        }
    }
}
