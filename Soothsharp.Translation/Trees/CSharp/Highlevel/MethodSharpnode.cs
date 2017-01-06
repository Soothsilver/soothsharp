using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.CSharp.Highlevel
{
    internal class MethodSharpnode : Sharpnode
    {
        private MethodDeclarationSyntax methodDeclarationSyntax;

        public TypeSharpnode ReturnType;
        public List<ParameterSharpnode> Parameters;
        public BlockSharpnode Body;

        public MethodSharpnode(MethodDeclarationSyntax method) : base(method)
        {
            this.methodDeclarationSyntax = method;
            this.Parameters = method.ParameterList.Parameters.Select(parameterSyntax => new ParameterSharpnode(parameterSyntax)).ToList();
            this.ReturnType = new TypeSharpnode(method.ReturnType);
            this.Body = new BlockSharpnode(method.Body);
        }

        public override TranslationResult Translate(TranslationContext context)
        {

            var method = context.Semantics.GetDeclaredSymbol(this.methodDeclarationSyntax);
            SubroutineBuilder builder = new SubroutineBuilder(
                method,
                false,
                null,
                Parameters,
                Body,
                context,
                OriginalNode);
            return builder.TranslateSelf();
        }
    }
}