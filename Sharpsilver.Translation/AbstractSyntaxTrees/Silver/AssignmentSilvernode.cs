using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class AssignmentSilvernode : Silvernode
    {
        private Silvernode right;
        private Silvernode left;

        public AssignmentSilvernode(Silvernode left, Silvernode right, SyntaxNode originalNode) : base(originalNode)
        {
            this.left = left;
            this.right = right;
        }
        public override string ToString()
        {
            return left + " := " + right;
        }
    }
}