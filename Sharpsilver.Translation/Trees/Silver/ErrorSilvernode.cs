using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    class ErrorSilvernode : Silvernode
    {
        public ErrorSilvernode(SyntaxNode node) : base(node)
        {

        }
        public override string ToString()
        {
            return "!ERROR!";
        }
    }
}
