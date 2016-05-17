using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    public class InvocationExpressionSharpnode : ExpressionSharpnode
    {
        public ExpressionSyntax MethodGroup;
        public List<ExpressionSharpnode> Arguments = new List<ExpressionSharpnode>();

        public InvocationExpressionSharpnode(InvocationExpressionSyntax syntax) : base(syntax)
        {
            this.MethodGroup = syntax.Expression;
            foreach(var argument in syntax.ArgumentList.Arguments)
            {
                this.Arguments.Add(RoslynToSharpnode.MapExpression(argument.Expression));
                // TODO name:colon, ref/out...
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var method = context.Semantics.GetSymbolInfo(MethodGroup);
            var methodSymbol = method.Symbol as IMethodSymbol;
            var methodName = methodSymbol.GetQualifiedName();

            // Verification conditions
            switch(methodName)
            {
                case ContractsTranslator.CONTRACT_ENSURES:
                case ContractsTranslator.CONTRACT_REQUIRES:
                case ContractsTranslator.CONTRACT_INVARIANT:
                    return TranslateAsVerificationCondition(methodName, context);
            }

            // Get identifier and evaluate arguments
            string identifier = context.Process.IdentifierTranslator.GetMethodIdentifierAtCallsite(method.Symbol as IMethodSymbol);
            var expressions = new List<Silvernode>();
            var errors = new List<Error>();
            foreach(var argument in Arguments)
            {
                var result = argument.Translate(context);
                expressions.Add(result.Silvernode);
                errors.AddRange(result.Errors);
            }

            Error error;
            SilverType st = TypeTranslator.TranslateType(methodSymbol.ReturnType, MethodGroup, out error);
            if (error != null) errors.Add(error);

            // Return result
            Silvernode invocationSilvernode = new CallSilvernode(
                    identifier,
                    expressions,
                    st,
                    OriginalNode
                );
            return TranslationResult.FromSilvernode(
                invocationSilvernode,
                errors
                );
        }

        private TranslationResult TranslateAsVerificationCondition(string methodName, TranslationContext context)
        {
            // TODO more checks
            var conditionResult = Arguments[0].Translate(context.ForcePure());
            Silvernode result = null;
            switch(methodName)
            {
                case ContractsTranslator.CONTRACT_ENSURES:
                    result = new EnsuresSilvernode(conditionResult.Silvernode, OriginalNode);
                    break;
                case ContractsTranslator.CONTRACT_REQUIRES:
                    result = new RequiresSilvernode(conditionResult.Silvernode, OriginalNode);
                    break;
                case ContractsTranslator.CONTRACT_INVARIANT:
                    result = new InvariantSilvernode(conditionResult.Silvernode, OriginalNode);
                    break;
                default:
                    return TranslationResult.Error(OriginalNode, Diagnostics.SSIL301_InternalLocalizedError, "unknown kind of verification condition");
            }
            return TranslationResult.FromSilvernode(result, conditionResult.Errors);
        }
    }
}