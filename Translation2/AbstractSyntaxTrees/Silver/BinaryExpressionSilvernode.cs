using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions
{
    internal class BinaryExpressionSilvernode : Silvernode
    {
        private Silvernode left;
        private Silvernode right;
        private string Operator;

        public BinaryExpressionSilvernode(Silvernode left, string @operator, Silvernode right, SyntaxNode originalNode) : base(originalNode)
        {
            this.Operator = @operator;
            this.left = left;
            this.right = right;
        }

        public override string ToString()
        {
            // TODO handle parentheses correctly
            return left.ToString() + " " + Operator + " " + right.ToString();
        }
    }
}