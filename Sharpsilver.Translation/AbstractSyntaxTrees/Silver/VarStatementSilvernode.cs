using System;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    public class VarStatementSilvernode : Silvernode
    {
        private Identifier identifier;
        private string type;

        public VarStatementSilvernode(Identifier identifier, string v, SyntaxNode originalNode) : base(originalNode)
        {
            this.identifier = identifier;
            this.type = v;
        }

        public override string ToString()
        {
            return "var " + identifier + " : " + type;
        }
    }
}