using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class AssignmentSilvernode : StatementSilvernode
    {
        private Silvernode right;
        private Silvernode left;

        public AssignmentSilvernode(Silvernode left, Silvernode right, SyntaxNode originalNode) : base(originalNode)
        {
            this.left = left;
            this.right = right;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return this.left;
                yield return " := ";
                yield return this.right;
            }
        }
    }
}