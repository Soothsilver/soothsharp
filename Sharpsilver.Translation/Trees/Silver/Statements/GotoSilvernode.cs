using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Statements
{
    class GotoSilvernode : StatementSilvernode
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
        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "goto ";
                yield return (Identifier?.ToString() ?? Label);
            }
        }
    }
}
