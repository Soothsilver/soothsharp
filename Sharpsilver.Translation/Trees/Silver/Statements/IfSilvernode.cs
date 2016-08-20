using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class IfSilvernode : Silvernode
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

        public override string ToString()
        {
            return "if (" + condition.ToString() + ") " + this.then.ToString()
                + (elseBranch != null ? " else " + this.elseBranch.ToString() : "");
        }
    }
}