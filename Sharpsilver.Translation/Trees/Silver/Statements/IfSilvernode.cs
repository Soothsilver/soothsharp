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
            OriginalNode = originalNode;
            this.condition = condition;
            this.then = then;
            this.elseBranch = elseBranch;
            // TODO do this for Sharpnodes, rather.
            // TODO (elsewhere) issue a warning when verification conditions are not top-level
            if (!(this.then is BlockSilvernode))
            {
                this.then = new BlockSilvernode(null, new List<StatementSilvernode> { then });
            }
            if (this.elseBranch != null && !(this.elseBranch is BlockSilvernode))
            {
                this.elseBranch = new BlockSilvernode(null, new List<StatementSilvernode> { elseBranch });
            }
        }

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "if (";
                yield return condition;
                yield return ") ";
                yield return this.then;
                if (elseBranch != null)
                {
                    yield return " else ";
                    yield return elseBranch;
                }
            }
        }
    }
}