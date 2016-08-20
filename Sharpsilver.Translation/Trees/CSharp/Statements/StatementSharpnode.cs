using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    public abstract class StatementSharpnode : Sharpnode
    {

        protected StatementSharpnode(SyntaxNode stmt) : base(stmt)
        {

        }

    }
}