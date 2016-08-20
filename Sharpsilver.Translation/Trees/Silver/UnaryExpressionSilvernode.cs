using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions
{
    internal class PrefixUnaryExpressionSilvernode : Silvernode
    {
        private Silvernode Operand;
        private string Operator;

        public PrefixUnaryExpressionSilvernode(string @operator, Silvernode operand, SyntaxNode originalNode) : base(originalNode)
        {
            this.Operator = @operator;
            this.Operand = operand;
        }

        public override string ToString()
        {
            // TODO handle parentheses correctly
            return Operator + Operand.ToString();
        }
    }
}