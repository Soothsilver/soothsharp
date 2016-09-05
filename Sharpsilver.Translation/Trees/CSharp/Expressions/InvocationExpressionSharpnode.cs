using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Trees.CSharp.Expressions;

namespace Sharpsilver.Translation.Trees.CSharp
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
                case ContractsTranslator.ContractEnsures:
                case ContractsTranslator.ContractRequires:
                case ContractsTranslator.ContractInvariant:
                case ContractsTranslator.ContractRead:
                    return TranslateAsVerificationCondition(methodName, context);
                case ContractsTranslator.Implication:
                    return TranslateAsImplication(context);
            }

            // Get identifier and evaluate arguments
            var identifier = context.Process.IdentifierTranslator.GetIdentifierReference(method.Symbol as IMethodSymbol);
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

        private TranslationResult TranslateAsImplication(TranslationContext context)
        {
            if (MethodGroup is MemberAccessExpressionSyntax)
            {
                MemberAccessExpressionSyntax memberAccess = MethodGroup as MemberAccessExpressionSyntax;
                var leftExpression = RoslynToSharpnode.MapExpression(memberAccess.Expression);
                var leftExpressionResult = leftExpression.Translate(context);
                var rightExpressionResult = Arguments[0].Translate(context.ForcePure());
                Silvernode implies = new BinaryExpressionSilvernode(
                    leftExpressionResult.Silvernode,
                    "==>",
                    rightExpressionResult.Silvernode,
                    OriginalNode
                    );
                var errors = new List<Error>();
                errors.AddRange(leftExpressionResult.Errors);
                errors.AddRange(rightExpressionResult.Errors);
                return TranslationResult.FromSilvernode(implies, errors);
            }
            else
            {
                return TranslationResult.Error(OriginalNode, Diagnostics.SSIL110_InvalidSyntax,
                    "member access expression expected");
            }

        }

        private TranslationResult TranslateAsVerificationCondition(string methodName, TranslationContext context)
        {
            // TODO more checks
            var conditionResult = Arguments[0].Translate(context.ForcePure());
            Silvernode result = null;
            switch(methodName)
            {
                case ContractsTranslator.ContractEnsures:
                    result = new EnsuresSilvernode(conditionResult.Silvernode, OriginalNode);
                    break;
                case ContractsTranslator.ContractRequires:
                    result = new RequiresSilvernode(conditionResult.Silvernode, OriginalNode);
                    break;
                case ContractsTranslator.ContractInvariant:
                    result = new InvariantSilvernode(conditionResult.Silvernode, OriginalNode);
                    break;
                case ContractsTranslator.ContractRead:
                    result = new AccSilvernode(conditionResult.Silvernode, new TextSilvernode("epsilon"), OriginalNode);
                    break;
                default:
                    return TranslationResult.Error(OriginalNode, Diagnostics.SSIL301_InternalLocalizedError, "unknown kind of verification condition");
            }
            return TranslationResult.FromSilvernode(result, conditionResult.Errors);
        }
    }
}