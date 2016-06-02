using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class GotoSilvernode : Silvernode
    {
        public Identifier Identifier;
        public string Label;
        public GotoSilvernode(Identifier identifier, SyntaxNode original) : base(original)
        {
            Identifier = identifier;
        }

        public GotoSilvernode(string text, SyntaxNode original) : base(original)
        {
            Label = text;
        }

        public override string ToString()
        {
            return "goto " + (Identifier?.ToString() ?? Label);
        }
    }
}
