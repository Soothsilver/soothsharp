using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class IdentifierSilvernode : Silvernode
    {
        private IdentifierDeclaration silverIdentifier;
        private IdentifierReference silverIdentifierReference;

        public IdentifierSilvernode(Identifier identifier) : base(null)
        {
            if (identifier is IdentifierDeclaration)
                silverIdentifier = identifier as IdentifierDeclaration;
            else
                silverIdentifierReference = identifier as IdentifierReference;
        }

        public IdentifierSilvernode(IdentifierDeclaration silverIdentifier, SyntaxNode originalNode = null) : base(originalNode)
        {
            this.silverIdentifier = silverIdentifier;
        }
        public IdentifierSilvernode(IdentifierReference silverIdentifier, SyntaxNode originalNode = null) : base(originalNode)
        {
            this.silverIdentifierReference = silverIdentifier;
        }

        public static implicit operator IdentifierSilvernode(Identifier identifier)
        {
            return new IdentifierSilvernode(identifier);
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