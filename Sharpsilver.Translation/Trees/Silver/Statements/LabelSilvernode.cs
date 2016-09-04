using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Statements
{
    class LabelSilvernode : StatementSilvernode
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

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "label ";
                yield return (Identifier?.ToString() ?? Label);
            }
        }
    }
}
