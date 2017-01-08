using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Soothsharp.Translation.Exceptions;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    public class ParameterSharpnode : Sharpnode
    {
        public ParameterSyntax ParameterSyntax;
        private TypeSharpnode Type;

        public ParameterSharpnode(ParameterSyntax parameterSyntax) : base(parameterSyntax)
        {
            this.ParameterSyntax = parameterSyntax;
            this.Type = new TypeSharpnode(parameterSyntax.Type);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            throw new TranslationNotSupportedException("ParameterSharpnode");
        }

        public TranslationResult Translate(TranslationContext context, IParameterSymbol symbol)
        {
            Error err;
            ISymbol parameterSymbol = context.Semantics.GetDeclaredSymbol(this.ParameterSyntax);
            Identifier identifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifier(parameterSymbol);
            ParameterSilvernode ps = new ParameterSilvernode(identifier,
                new TypeSilvernode(this.Type.TypeSyntax, TypeTranslator.TranslateType(symbol.Type, this.Type.TypeSyntax, out err)), this.OriginalNode);
            var errlist = new List<Error>();
            if (err != null) errlist.Add(err);
            return TranslationResult.FromSilvernode(ps, errlist);
        }
    }
}