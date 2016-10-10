using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Sharpsilver.Translation.Trees.Silver
{
    public class TextStatementSilvernode : StatementSilvernode
    {
        private string text;
        public TextStatementSilvernode(string text, SyntaxNode originalNode = null) : base(originalNode)
        {
            this.text = text;
        }
        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return text;
            }
        }

    }
}
