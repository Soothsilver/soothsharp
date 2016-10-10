using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    public class EmptySilvernode : Silvernode
    {
        public EmptySilvernode(SyntaxNode node) : base(node)
        {

        }

        public override string ToString()
        {
            return "";
        }
    }
}
