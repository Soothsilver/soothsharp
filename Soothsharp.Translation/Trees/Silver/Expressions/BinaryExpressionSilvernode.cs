using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class BinaryExpressionSilvernode : ExpressionSilvernode
    {
        private Silvernode left;
        private Silvernode right;
        private string Operator;

        public BinaryExpressionSilvernode(Silvernode left, string @operator, Silvernode right, SyntaxNode originalNode) : base(originalNode, SilverType.Int)
        {
            //TODO handle types correctly
            this.Operator = @operator;
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            // TODO handle parentheses correctly
            return this.left + " " + this.Operator + " " + this.right;
        }
    }
}