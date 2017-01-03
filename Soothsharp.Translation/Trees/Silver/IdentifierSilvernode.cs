using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class IdentifierSilvernode : Silvernode
    {
        private Identifier identifier;
        
        
        public IdentifierSilvernode(Identifier silverIdentifier, SyntaxNode originalNode = null) : base(originalNode)
        {
            this.identifier = silverIdentifier;
        }

        public static implicit operator IdentifierSilvernode(Identifier identifier)
        {
            return new IdentifierSilvernode(identifier);
        }

        public override string ToString()
        {
            return identifier.ToString();
        }
    }
}