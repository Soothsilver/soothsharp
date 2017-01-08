using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Soothsharp.Translation.Trees.Silver
{
    public class TextStatementSilvernode : StatementSilvernode
    {
        private string text;
        public TextStatementSilvernode(string text, SyntaxNode originalNode = null) : base(originalNode)
        {
            this.text = text;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return this.text;
            }
        }

    }
}
