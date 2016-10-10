using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class SinglelineCommentSilvernode : Silvernode
    {
        private string comment;

        public SinglelineCommentSilvernode(string v, SyntaxNode originalNode) : base(originalNode)
        {
            this.comment = v;
        }

        public override string ToString()
        {
            return "// " + this.comment + "";
        }
    }
}