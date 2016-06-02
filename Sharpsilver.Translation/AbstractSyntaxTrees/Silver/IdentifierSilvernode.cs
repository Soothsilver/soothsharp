using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class IdentifierSilvernode : Silvernode
    {
        private SyntaxToken identifierToken;
        private IdentifierDeclaration silverIdentifier;
        private IdentifierReference silverIdentifierReference;

        public IdentifierSilvernode(SyntaxToken identifierToken, IdentifierDeclaration silverIdentifier) : base(identifierToken)
        {
            this.identifierToken = identifierToken;
            this.silverIdentifier = silverIdentifier;
        }
        public IdentifierSilvernode(SyntaxToken identifierToken, IdentifierReference silverIdentifier) : base(identifierToken)
        {
            this.identifierToken = identifierToken;
            this.silverIdentifierReference = silverIdentifier;
        }

        public override string ToString()
        {
            if (silverIdentifier != null)
                return silverIdentifier.ToString();
            else
                return silverIdentifierReference.ToString();
        }
    }
}