using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    public class ObjectCreationExpressionSharpnode : ExpressionSharpnode
    {
        public List<ExpressionSharpnode> Arguments = new List<ExpressionSharpnode>();

        public ObjectCreationExpressionSharpnode(ObjectCreationExpressionSyntax syntax) : base(syntax)
        {
            foreach (var argument in syntax.ArgumentList.Arguments)
            {
                this.Arguments.Add(RoslynToSharpnode.MapExpression(argument.Expression));
                // TODO name:colon, ref/out...
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var constructorSymbol = context.Semantics.GetSymbolInfo(OriginalNode).Symbol as IMethodSymbol;
            var classSymbol = constructorSymbol.ContainingType;
            bool isDefaultConstructor = constructorSymbol.IsImplicitlyDeclared;

            var identifier = context.Process.IdentifierTranslator.RegisterNewUniqueIdentifier();

            var arguments = new List<Silvernode>();
            var errors = new List<Error>();
            // TODO add purifiable thingies before here
            foreach(var arg in Arguments)
            {
                var res = arg.Translate(context.ChangePurityContext(PurityContext.Purifiable));
                arguments.Add(res.Silvernode);
                errors.AddRange(res.Errors);
            }

            Silvernode constructorCall = new CallSilvernode(context.Process.IdentifierTranslator.GetIdentifierReferenceWithTag(classSymbol, isDefaultConstructor ? Constants.InitializerTag : Constants.ConstructorTag),
                arguments,
                SilverType.Ref,
                this.OriginalNode);

            switch(context.PurityContext)
            {
                case PurityContext.PurityNotRequired:
                case PurityContext.Purifiable:
                    return TranslationResult.FromSilvernode(new IdentifierSilvernode(identifier), errors).AndPrepend(
                        new VarStatementSilvernode(identifier, SilverType.Ref, this.OriginalNode),
                        new AssignmentSilvernode(new IdentifierSilvernode(identifier), constructorCall, this.OriginalNode) 
                        );

                case PurityContext.PureOrFail:
                    return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL114_NotPureContext, "Object creation is inherently impure.");
            }

            return TranslationResult.FromSilvernode(
                new NewStarSilvernode(this.OriginalNode)
                );
        }

    }
}