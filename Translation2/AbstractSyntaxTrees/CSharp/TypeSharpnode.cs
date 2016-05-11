using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Exceptions;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    public class TypeSharpnode : Sharpnode
    {
        public TypeSyntax TypeSyntax;

        public TypeSharpnode(TypeSyntax originalNode) : base(originalNode)
        {
            TypeSyntax = originalNode;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            throw new TranslationNotSupportedException("TypeSharpnode");
        }
    }
}