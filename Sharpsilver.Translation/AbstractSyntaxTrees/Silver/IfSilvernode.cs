using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class IfSilvernode : Silvernode
    {
        private Silvernode condition;
        private Silvernode then;
        private Silvernode elseBranch;

        public IfSilvernode(SyntaxNode originalNode,
            Silvernode translationResult1,
            Silvernode then,
            Silvernode elseBranch)
            : base(originalNode)
        {
            OriginalNode = originalNode;
            this.condition = translationResult1;
            this.then = then;
            this.elseBranch = elseBranch;
            // TODO do this for Sharpnodes, rather.
            // TODO (elsewhere) issue a warning when verification conditions are not top-level
            if (!(this.then is BlockSilvernode))
            {
                this.then = new BlockSilvernode(null, new List<Silvernode> { then });
            }
            if (this.elseBranch != null && !(this.elseBranch is BlockSilvernode))
            {
                this.elseBranch = new BlockSilvernode(null, new List<Silvernode> { elseBranch });
            }
        }

        public override string ToString()
        {
            return "if (" + condition.ToString() + ") " + this.then.ToString()
                + (elseBranch != null ? " else " + this.elseBranch.ToString() : "");
        }
    }
}