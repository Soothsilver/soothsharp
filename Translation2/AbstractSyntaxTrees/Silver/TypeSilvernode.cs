using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class TypeSilvernode : Silvernode
    {
        private TypeSyntax returnType;
        private string silverType;

        public TypeSilvernode(TypeSyntax typeSyntax, string silverType) : base(typeSyntax)
        {
            this.returnType = typeSyntax;
            this.silverType = silverType;
        }

        public override string ToString()
        {
            return silverType;
        }
    }
}