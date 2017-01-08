using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Exceptions;

namespace Soothsharp.Translation.Trees.CSharp
{
    public class TypeSharpnode : Sharpnode
    {
        public TypeSyntax TypeSyntax;

        public TypeSharpnode(TypeSyntax originalNode) : base(originalNode)
        {
            this.TypeSyntax = originalNode;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            throw new TranslationNotSupportedException("TypeSharpnode");
        }
    }
}