using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class NewStarSilvernode : ExpressionSilvernode
    {
        public NewStarSilvernode(
            SyntaxNode originalNode) : base(originalNode, SilverType.Ref)
        {

        }

        public override string ToString()
        {
            return "new(*)";
        }
    }
}