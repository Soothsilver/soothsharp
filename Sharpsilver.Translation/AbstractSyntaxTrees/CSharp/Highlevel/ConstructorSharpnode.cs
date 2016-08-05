using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Highlevel
{
    internal class ConstructorSharpnode : Sharpnode
    {
        private ClassDeclarationSyntax parentClass;
        public BlockSharpnode Body { get; set; }
        public List<ParameterSharpnode> Parameters { get; set; }
        private ConstructorDeclarationSyntax methodSyntax;

        public ConstructorSharpnode(ConstructorDeclarationSyntax method)
            : base(method)
        {
            this.parentClass = (ClassDeclarationSyntax) method.Parent;
            this.Parameters = method.ParameterList.Parameters.Select(parameterSyntax => new ParameterSharpnode(parameterSyntax)).ToList();
            this.Body = new BlockSharpnode(method.Body);
            this.methodSyntax = method;

        }


        public ConstructorSharpnode(ClassSharpnode theClass)
           : base(null)
        {
            this.parentClass = theClass.DeclarationSyntax;
            this.Parameters = new List<ParameterSharpnode>();
            this.Body = new CSharp.BlockSharpnode(null);
        }


        public override TranslationResult Translate(TranslationContext context)
        {
            var classSymbol = context.Semantics.GetDeclaredSymbol(this.parentClass); 
            var identifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifierWithTag(classSymbol, "init");
            var innerContext = context;
            var body = this.Body.Translate(innerContext);
            var methodSymbol = this.methodSyntax != null ? context.Semantics.GetDeclaredSymbol(this.methodSyntax) : null;
            
            Error diagnostic;
            var silverParameters = new List<ParameterSilvernode>();
            TranslationResult r = new TranslationResult();

            if (methodSymbol != null)
            {

                var attributes = methodSymbol.GetAttributes();
                if (
                    attributes.Any(
                        attribute =>
                            attribute.AttributeClass.GetQualifiedName() == ContractsTranslator.UnverifiedAttribute))
                {
                    return
                        TranslationResult.FromSilvernode(
                            new SinglelineCommentSilvernode(
                                $"Constructor for {classSymbol.GetQualifiedName()} skipped because it was marked [Unverified].",
                                this.OriginalNode));
                }

                for (int i = 0; i < this.Parameters.Count; i++)
                {
                    ParameterSharpnode sharpnode = this.Parameters[i];
                    var symbol = methodSymbol.Parameters[i];
                    var rrs = sharpnode.Translate(context, symbol);
                    silverParameters.Add(rrs.Silvernode as ParameterSilvernode);
                    r.Errors.AddRange(rrs.Errors);
                }
            }
            var innerStatements = body.Silvernode as BlockSilvernode;
            Debug.Assert(innerStatements != null, "innerStatements != null");
            innerStatements.Add(new LabelSilvernode(Constants.SilverMethodEndLabel, null));

            var methodSilvernode = 
                new MethodSilvernode(this.methodSyntax, // method
                    new IdentifierSilvernode(identifier), // name
                    silverParameters,
                    new TypeSilvernode(null,                    
                        TypeTranslator.TranslateType(
                            classSymbol, null, 
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