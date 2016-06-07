using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel
{
    internal class MethodSharpnode : Sharpnode
    {
        private MethodDeclarationSyntax methodDeclarationSyntax;

        public TypeSharpnode ReturnType;
        public List<ParameterSharpnode> Parameters;
        public BlockSharpnode Body;

        public MethodSharpnode(MethodDeclarationSyntax method) : base(method)
        {
            methodDeclarationSyntax = method;
            Parameters = method.ParameterList.Parameters.Select(parameterSyntax => new ParameterSharpnode(parameterSyntax)).ToList();
            ReturnType = new TypeSharpnode(method.ReturnType);
            Body = new BlockSharpnode(method.Body);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
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

            var silverParameters = new List<ParameterSilvernode>();
            TranslationResult r = new TranslationResult();
            for (int i = 0; i < Parameters.Count; i++)
            {
                ParameterSharpnode sharpnode = Parameters[i];
                var symbol = method.Parameters[i];
                var rrs = sharpnode.Translate(context, symbol);
                silverParameters.Add(rrs.Silvernode as ParameterSilvernode);
                r.Errors.AddRange(rrs.Errors);
            }
            var innerStatements = body.Silvernode as BlockSilvernode;
            Debug.Assert(innerStatements != null, "innerStatements != null");
            innerStatements.Add(new LabelSilvernode(Constants.SilverMethodEndLabel, null));
            var methodSilvernode = 
                new MethodSilvernode(
                    methodDeclarationSyntax, // method
                    new IdentifierSilvernode(methodDeclarationSyntax.Identifier, identifier), // name
                    silverParameters,
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