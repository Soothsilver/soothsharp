﻿using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.CSharp
{
    public abstract class StatementSharpnode : Sharpnode
    {

        protected StatementSharpnode(SyntaxNode stmt) : base(stmt)
        {

        }

    }
}