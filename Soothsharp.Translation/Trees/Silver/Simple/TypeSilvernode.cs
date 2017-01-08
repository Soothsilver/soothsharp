using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class TypeSilvernode : Silvernode
    {
        private SilverType silverType;

        public TypeSilvernode(TypeSyntax typeSyntax, SilverType silverType) : base(typeSyntax)
        {
            this.silverType = silverType;
        }

        public override string ToString()
        {
            return this.silverType.ToString();
        }

        public bool RepresentsVoid()
        {
            return this.silverType == SilverType.Void;
        }
    }
}