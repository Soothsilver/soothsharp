using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class LabelSilvernode : StatementSilvernode
    {
        private Identifier Identifier;
        public string Label;

        public LabelSilvernode(Identifier identifier, SyntaxNode original) : base(original)
        {
            this.Identifier = identifier;
        }

        public LabelSilvernode(string text, SyntaxNode original) : base(original)
        {
            this.Label = text;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "label ";
                yield return (this.Identifier?.ToString() ?? this.Label);
            }
        }
    }
}
