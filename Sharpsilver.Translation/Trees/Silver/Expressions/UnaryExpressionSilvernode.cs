using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class PrefixUnaryExpressionSilvernode : Silvernode
    {
        private Silvernode operand;
        private string Operator;

        public PrefixUnaryExpressionSilvernode(string @operator, Silvernode operand, SyntaxNode originalNode) : base(originalNode)
        {
            this.Operator = @operator;
            this.operand = operand;
        }

        public override string ToString()
        {
            // TODO handle parentheses correctly
            return Operator + this.operand;
        }
    }
}