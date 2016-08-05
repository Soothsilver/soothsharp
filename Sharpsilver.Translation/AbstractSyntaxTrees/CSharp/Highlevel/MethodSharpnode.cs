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
            this.methodDeclarationSyntax = method;
            this.Parameters = method.ParameterList.Parameters.Select(parameterSyntax => new ParameterSharpnode(parameterSyntax)).ToList();
            this.ReturnType = new TypeSharpnode(method.ReturnType);
            this.Body = new BlockSharpnode(method.Body);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var method = context.Process.SemanticModel.GetDeclaredSymbol(this.methodDeclarationSyntax);
            var identifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifier(method);
            var innerContext = context;
            var body = this.Body.Translate(innerContext);
            
            Error diagnostic;

            var attributes = method.GetAttributes();
            if (attributes.Any(attribute => attribute.AttributeClass.GetQualifiedName() == ContractsTranslator.UnverifiedAttribute))
            {
                return TranslationResult.FromSilvernode(new SinglelineCommentSilvernode($"Method {method.GetQualifiedName()} skipped because it was marked [Unverified].", this.OriginalNode));
            }

            var silverParameters = new List<ParameterSilvernode>();
            TranslationResult r = new TranslationResult();
            for (int i = 0; i < this.Parameters.Count; i++)
            {
                ParameterSharpnode sharpnode = this.Parameters[i];
                var symbol = method.Parameters[i];
                var rrs = sharpnode.Translate(context, symbol);
                silverParameters.Add(rrs.Silvernode as ParameterSilvernode);
                r.Errors.AddRange(rrs.Errors);
            }
            var innerStatements = body.Silvernode as BlockSilvernode;
            Debug.Assert(innerStatements != null, "innerStatements != null");
            innerStatements.Add(new LabelSilvernode(Constants.SilverMethodEndLabel, null));
            var methodSilvernode = 
                new MethodSilvernode(this.methodDeclarationSyntax, // method
                    new IdentifierSilvernode(this.methodDeclarationSyntax.Identifier, identifier), // name
                    silverParameters,
                    new TypeSilvernode(this.methodDeclarationSyntax.ReturnType,                    
                        TypeTranslator.TranslateType(
                            method.ReturnType, this.methodDeclarationSyntax.ReturnType, 
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