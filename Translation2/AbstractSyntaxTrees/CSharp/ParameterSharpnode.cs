using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    public class ParameterSharpnode : Sharpnode
    {
        private ParameterSyntax parameterSyntax;

        public ParameterSharpnode(ParameterSyntax parameterSyntax) : base(parameterSyntax)
        {
            this.parameterSyntax = parameterSyntax;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            throw new NotImplementedException();
        }
    }
}