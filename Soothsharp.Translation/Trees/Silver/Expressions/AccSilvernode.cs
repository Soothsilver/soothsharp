using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class AccSilvernode : ExpressionSilvernode
    {
        private Silvernode ProtectedElement;
        private Silvernode Permission;

        public AccSilvernode(Silvernode protectedElement, Silvernode permission, SyntaxNode originalNode) :
            base(originalNode, SilverType.Bool)
        {
            this.ProtectedElement = protectedElement;
            this.Permission = permission;
        }

        public override string ToString()
        {
            return "acc(" + this.ProtectedElement + ", " + this.Permission + ")";
        }
    }
}
