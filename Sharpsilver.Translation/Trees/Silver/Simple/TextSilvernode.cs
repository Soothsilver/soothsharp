using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    public class TextSilvernode : Silvernode
    {
        private string text;
        public TextSilvernode(string text, SyntaxNode originalNode = null) : base(originalNode)
        {
            this.text = text;
        }
        public override string ToString()
        {
            return text;
        }
    }
}
