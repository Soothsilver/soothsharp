using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class IfSilvernode : StatementSilvernode
    {
        private Silvernode condition;
        private Silvernode then;
        private Silvernode elseBranch;

        public IfSilvernode(SyntaxNode originalNode,
            Silvernode condition,
            StatementSilvernode then,
            StatementSilvernode elseBranch)
            : base(originalNode)
        {
            this.condition = condition;
            this.then = then;
            this.elseBranch = elseBranch;
            if (!(this.then is BlockSilvernode))
            {
                this.then = new BlockSilvernode(null, new List<StatementSilvernode> { then });
            }
            if (this.elseBranch != null && !(this.elseBranch is BlockSilvernode))
            {
                this.elseBranch = new BlockSilvernode(null, new List<StatementSilvernode> { elseBranch });
            }
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "if (";
                yield return this.condition;
                yield return ") ";
                yield return this.then;
                if (this.elseBranch != null)
                {
                    yield return " else ";
                    yield return this.elseBranch;
                }
            }
        }
    }
}