using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class SinglelineCommentSilvernode : Silvernode
    {
        private string Comment;

        public SinglelineCommentSilvernode(string v, SyntaxNode originalNode) : base(originalNode)
        {
            this.Comment = v;
        }

        public override string ToString()
        {
            return "// " + Comment + "";
        }
    }
}