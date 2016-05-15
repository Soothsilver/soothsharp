using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class LabelSilvernode : Silvernode
    {
        public string Label;
        public LabelSilvernode(string text, SyntaxNode original) : base(original)
        {
            Label = text;
        }

        public override string ToString()
        {
            return "label " + Label;
        }
    }
}
