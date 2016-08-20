using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class ParameterSilvernode : ComplexSilvernode
    {
        public TypeSilvernode Type;
        public Identifier Identifier;
        public ParameterSilvernode(Identifier identifier, TypeSilvernode type, SyntaxNode node) : base(node)
        {
            Identifier = identifier;
            Type = type;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return new IdentifierSilvernode(Identifier);
                yield return " : ";
                yield return Type;
            }
        }
    }
}