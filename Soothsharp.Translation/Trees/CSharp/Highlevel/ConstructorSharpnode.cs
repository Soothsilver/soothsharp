using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.CSharp.Highlevel
{
    internal class ConstructorSharpnode : Sharpnode
    {
        private ClassDeclarationSyntax parentClass;
        private BlockSharpnode Body { get; }
        private List<ParameterSharpnode> Parameters { get; }
        private ConstructorDeclarationSyntax methodSyntax;

        public ConstructorSharpnode(ConstructorDeclarationSyntax method)
            : base(method)
        {
            this.parentClass = (ClassDeclarationSyntax) method.Parent;
            this.Parameters = method.ParameterList.Parameters.Select(parameterSyntax => new ParameterSharpnode(parameterSyntax)).ToList();
            this.Body = new BlockSharpnode(method.Body);
            this.methodSyntax = method;

        }


        public override TranslationResult Translate(TranslationContext context)
        {
            var methodSymbol = this.methodSyntax != null ? context.Semantics.GetDeclaredSymbol(this.methodSyntax) : null;
            var classSymbol = context.Semantics.GetDeclaredSymbol(this.parentClass);


            SubroutineBuilder builder = new SubroutineBuilder(
                methodSymbol,
                true,
                classSymbol, this.Parameters, this.Body,
                context, 
                this.OriginalNode);
            return builder.TranslateSelf();
        }
    }
}