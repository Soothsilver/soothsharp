using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class IdentifierSilvernode : Silvernode
    {
        private SyntaxToken identifierToken;
        private string silverIdentifier;

        public IdentifierSilvernode(SyntaxToken identifierToken, string silverIdentifier) : base(identifierToken)
        {
            this.identifierToken = identifierToken;
            this.silverIdentifier = silverIdentifier;
        }

        public override string ToString()
        {
            return silverIdentifier;
        }
    }
}