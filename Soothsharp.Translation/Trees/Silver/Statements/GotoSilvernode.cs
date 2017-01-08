using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class GotoSilvernode : StatementSilvernode
    {
        private Identifier Identifier;
        public string Label;
        public GotoSilvernode(Identifier identifier, SyntaxNode original) : base(original)
        {
            this.Identifier = identifier;
        }

        public GotoSilvernode(string text, SyntaxNode original) : base(original)
        {
            this.Label = text;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "goto ";
                yield return (this.Identifier?.ToString() ?? this.Label);
            }
        }
    }
}
