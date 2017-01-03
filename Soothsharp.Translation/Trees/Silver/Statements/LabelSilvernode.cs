using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class LabelSilvernode : StatementSilvernode
    {
        public Identifier Identifier;
        public string Label;

        public LabelSilvernode(Identifier identifier, SyntaxNode original) : base(original)
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
