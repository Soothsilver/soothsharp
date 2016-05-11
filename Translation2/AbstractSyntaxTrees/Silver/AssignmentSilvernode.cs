using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class AssignmentSilvernode : Silvernode
    {
        private Silvernode right;
        private TextSilvernode left;

        public AssignmentSilvernode(TextSilvernode left, Silvernode right, SyntaxNode originalNode) : base(originalNode)
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