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
        public IdentifierDeclaration Identifier;
        public string Label;

        public LabelSilvernode(IdentifierDeclaration identifier, SyntaxNode original) : base(original)
        {
            Identifier = identifier;
        }

        public LabelSilvernode(string text, SyntaxNode original) : base(original)
        {
            Label = text;
        }

        public override string ToString()
        {
            return "label " + (Identifier?.ToString() ?? Label);
        }
    }
}
