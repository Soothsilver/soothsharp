using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class AbsoluteValueSilvernode : ExpressionSilvernode
    {
        private readonly Silvernode _silvernode;

        public AbsoluteValueSilvernode(Silvernode silvernode, SyntaxNode node) : base(node, SilverType.Int)
        {
            this._silvernode = silvernode;
        }

        public override string ToString()
        {
            return "|" + this._silvernode + "|";
        }
    }
}