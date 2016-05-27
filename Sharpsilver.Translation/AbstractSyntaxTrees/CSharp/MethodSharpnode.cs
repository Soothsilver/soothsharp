using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using System.Linq;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class MethodSharpnode : Sharpnode
    {
        private MethodDeclarationSyntax methodDeclarationSyntax;
        private ClassSharpnode parent;
        private string name;

        public TypeSharpnode ReturnType;
        public List<ParameterSharpnode> Parameters = new List<ParameterSharpnode>();
        public BlockSharpnode Body;

        public MethodSharpnode(MethodDeclarationSyntax method, ClassSharpnode parent) : base(method)
        {
            this.methodDeclarationSyntax = method;
            this.parent = parent;
            this.name = method.Identifier.Text;
            this.Parameters = method.ParameterList.Parameters.Select(parameterSyntax => new ParameterSharpnode(parameterSyntax)).ToList();
            this.ReturnType = new TypeSharpnode(method.ReturnType);
            this.Body = new BlockSharpnode(method.Body);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            TranslationResult result = new TranslationResult();
            var method = context.Process.SemanticModel.GetDeclaredSymbol(methodDeclarationSyntax);
            var identifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifier(method);
            var innerContext = context;
            var body = Body.Translate(innerContext);
            
            Error diagnostic;

            var attributes = method.GetAttributes();
            if (attributes.Any(attribute => attribute.AttributeClass.GetQualifiedName() == ContractsTranslator.UnverifiedAttribute))
            {
                return TranslationResult.FromSilvernode(new SinglelineCommentSilvernode($"Method {method.GetQualifiedName()} skipped because it was marked [Unverified].", OriginalNode));
            }

            var SilverParameters = new List<ParameterSilvernode>();
            TranslationResult r = new TranslationResult();
            for (int i = 0; i < Parameters.Count; i++)
            {
                ParameterSharpnode sharpnode = Parameters[i];
                var symbol = method.Parameters[i];
                var rrs = sharpnode.Translate(context, symbol);
                SilverParameters.Add(rrs.Silvernode as ParameterSilvernode);
                r.Errors.AddRange(rrs.Errors);
            }
            var innerStatements = body.Silvernode as BlockSilvernode;
            innerStatements.Add(new LabelSilvernode(Constants.SilverMethodEndLabel, null));
            var methodSilvernode = 
                new MethodSilvernode(
                    methodDeclarationSyntax, // method
                    new IdentifierSilvernode(methodDeclarationSyntax.Identifier, identifier), // name
                    SilverParameters,
                    new TypeSilvernode(
                        methodDeclarationSyntax.ReturnType,                    
                        TypeTranslator.TranslateType(
                            method.ReturnType,
                            methodDeclarationSyntax.ReturnType, 
                            out diagnostic)
                        ), // return type
                    body.VerificationConditions, // verification conditions 
                    innerStatements // code
                );
            r.Silvernode = methodSilvernode;
            r.Errors.AddRange(body.Errors);
            if (diagnostic != null) r.Errors.Add(diagnostic);
            return r;
        }
    }
}