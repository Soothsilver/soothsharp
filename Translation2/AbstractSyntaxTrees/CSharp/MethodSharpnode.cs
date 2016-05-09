using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using System.Linq;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class MethodSharpnode : Sharpnode
    {
        private MethodDeclarationSyntax methodDeclarationSyntax;
        private ClassSharpnode parent;

        public Sharpnode ReturnType;
        public List<ParameterSharpnode> Parameters = new List<ParameterSharpnode>();
        public Sharpnode Body;

        public MethodSharpnode(MethodDeclarationSyntax method, ClassSharpnode parent) : base(method)
        {
            this.methodDeclarationSyntax = method;
            this.parent = parent;
            this.Parameters = method.ParameterList.Parameters.Select(parameterSyntax => new ParameterSharpnode(parameterSyntax)).ToList();
            this.ReturnType = new TypeSharpnode(method.ReturnType);
            this.Body = new BlockSharpnode(method.Body);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            TranslationResult result = new TranslationResult();
            var method = context.Process.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
            result.SilverSourceTree = new SequenceSilvernode(OriginalNode,
                  new TextSilvernode("method "),
                  new IdentifierSilvernode(method.Name),
                  new TextSilvernode("("),
                  new TextSilvernode(")"),
                  new TextSilvernode(" returns "),
                  new TextSilvernode("("),
                  new TextSilvernode("ssil_result : Int"),
                  new TextSilvernode(")"),
                  new TextSilvernode("\n")
                  new TextSilvernode("{"),
                  new TextSilvernode("}")
                );
        }
    }
}