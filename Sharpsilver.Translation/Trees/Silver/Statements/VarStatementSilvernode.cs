using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Statements
{
    public class VarStatementSilvernode : StatementSilvernode
    {
        private IdentifierDeclaration identifier;
        private string type;

        public VarStatementSilvernode(IdentifierDeclaration identifier, string v, SyntaxNode originalNode) : base(originalNode)
        {
            this.identifier = identifier;
            this.type = v;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "var ";
                yield return identifier.ToString();
                yield return " : ";
                yield return type;
            }
        }
    }
}