using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Statements
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
        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return left;
                yield return " := ";
                yield return right;
            }
        }
    }
}