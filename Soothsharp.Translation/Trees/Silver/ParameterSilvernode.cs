using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    internal class ParameterSilvernode : ComplexSilvernode
    {
        private TypeSilvernode Type;
        private Identifier Identifier;
        public ParameterSilvernode(Identifier identifier, TypeSilvernode type, SyntaxNode node) : base(node)
        {
            this.Identifier = identifier;
            this.Type = type;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return new IdentifierSilvernode(this.Identifier);
                yield return " : ";
                yield return this.Type;
            }
        }
    }
}