using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class ErrorSilvernode : Silvernode
    {
        public ErrorSilvernode(SyntaxNode node) : base(node)
        {

        }
        public override string ToString()
        {
            return "!ERROR!";
        }
    }
}
