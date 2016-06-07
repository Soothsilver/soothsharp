using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public class TextSilvernode : Silvernode
    {
        private string text;
        public TextSilvernode(string text, SyntaxNode originalNode = null) : base(originalNode)
        {
            this.text = text;
        }
        public override string ToString()
        {
            return text;
        }
    }
}
