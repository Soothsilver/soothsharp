using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public class ConditionalExpressionSilvernode : Silvernode
    {
        private Silvernode condition;
        private Silvernode whenTrue;
        private Silvernode whenFalse;

        public ConditionalExpressionSilvernode(Silvernode a, Silvernode b, Silvernode c, SyntaxNode originalNode) : base(originalNode)
        {
            this.condition = a;
            this.whenTrue = b;
            this.whenFalse = c;
        }

        public override string ToString()
        {
            return condition + " ? " + whenTrue + " : " + whenFalse;
        }
    }
}