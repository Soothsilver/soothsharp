using System;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    internal class ParameterSilvernode : Silvernode
    {
        public TypeSilvernode Type;
        public string Identifier;
        public ParameterSilvernode(string identifier, TypeSilvernode type, SyntaxNode node) : base(node)
        {
            Identifier = identifier;
            Type = type;
        }

        public override string ToString()
        {
            throw new Exception("This ToString() method should never be called.");
        }
    }
}