using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class ParenthesizedExpressionSilvernode : ExpressionSilvernode
    {
        private readonly ExpressionSilvernode expression;

        public ParenthesizedExpressionSilvernode(
            ExpressionSilvernode silvernode,
            SyntaxNode originalNode) : 
            base(originalNode, silvernode?.Type ?? SilverType.Error)
        {
            expression = silvernode;
        }

        public override string ToString()
        {
            return "(" + expression + ")";
        }
    }
}