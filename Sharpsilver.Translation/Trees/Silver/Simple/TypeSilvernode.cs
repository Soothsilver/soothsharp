using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.Trees.Silver
{
    internal class TypeSilvernode : Silvernode
    {
        private TypeSyntax returnType;
        private SilverType silverType;

        public TypeSilvernode(TypeSyntax typeSyntax, SilverType silverType) : base(typeSyntax)
        {
            this.returnType = typeSyntax;
            this.silverType = silverType;
        }

        public override string ToString()
        {
            return TypeTranslator.SilverTypeToString(silverType);
        }

        public bool RepresentsVoid()
        {
            return silverType == SilverType.Void;
        }
    }
}