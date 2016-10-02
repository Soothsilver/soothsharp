using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation.Trees.Silver.Statements;

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
