using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    public abstract class StatementSharpnode : Sharpnode
    {
        private StatementSyntax stmt;

        protected StatementSharpnode(StatementSyntax stmt) : base(stmt)
        {
            this.stmt = stmt;
        }
    }
}