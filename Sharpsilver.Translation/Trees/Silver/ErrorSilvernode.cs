using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
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
