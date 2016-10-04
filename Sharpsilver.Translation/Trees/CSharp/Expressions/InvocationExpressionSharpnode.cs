using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Trees.CSharp.Expressions;
using Sharpsilver.Translation.Trees.Silver.Statements;

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
            List<StatementSilvernode> prependors = new List<StatementSilvernode>();

            var method = context.Semantics.GetSymbolInfo(MethodGroup);
            var methodSymbol = method.Symbol as IMethodSymbol;
            var methodName = methodSymbol.GetQualifiedName();

            // Verification conditions
            bool translateAsPhpStatement = false;
            string languageFeatureName = null;
            switch (methodName)
            {
                case ContractsTranslator.ContractEnsures:
                case ContractsTranslator.ContractRequires:
                case ContractsTranslator.ContractInvariant:
                    return TranslateAsVerificationCondition(methodName, context);
                case ContractsTranslator.Implication:
                    return TranslateAsImplication(context);
                case ContractsTranslator.ContractAssert:
                    languageFeatureName = "assert";
                    translateAsPhpStatement = true;
                    break;
                case ContractsTranslator.ContractAssume:
                    languageFeatureName = "assume";
                    translateAsPhpStatement = true;
                    break;
                case ContractsTranslator.ContractInhale:
                    languageFeatureName = "inhale";
                    translateAsPhpStatement = true;
                    break;
                case ContractsTranslator.ContractExhale:
                    languageFeatureName = "exhale";
                    translateAsPhpStatement = true;
                    break;
                case ContractsTranslator.ContractAcc:
                    languageFeatureName = "acc";
                    break;
                case ContractsTranslator.Old:
                    languageFeatureName = "old";
                    break;
                case ContractsTranslator.Fold:
                    languageFeatureName = "fold";
                    translateAsPhpStatement = true;
                    break;
                case ContractsTranslator.Unfold:
                    languageFeatureName = "unfold";
                    translateAsPhpStatement = true;
                    break;
            }

            // Get identifier and evaluate arguments
            Error error;
            Identifier identifier;
            SilverType st;
            if (languageFeatureName == null)
            {
                identifier = context.Process.IdentifierTranslator.GetIdentifierReference(method.Symbol as IMethodSymbol);
                st = TypeTranslator.TranslateType(methodSymbol.ReturnType, MethodGroup, out error);
            } else
            {
                identifier = new Identifier(languageFeatureName);
                st = SilverType.Bool;
                error = null;
            }
            var expressions = new List<Silvernode>();
            var errors = new List<Error>();
            foreach(var argument in Arguments)
            {
                var result = argument.Translate(context);
                expressions.Add(result.Silvernode);
                prependors.AddRange(result.PrependTheseSilvernodes);
                errors.AddRange(result.Errors);
            }

            if (error != null) errors.Add(error);
            if (translateAsPhpStatement)
            {
                return TranslationResult.FromSilvernode(
                    new AssertionLikeSilvernode(languageFeatureName, expressions[0], this.OriginalNode)
                    , errors).AndPrepend(prependors.ToArray());
            }
            else
            {
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
                    ).AndPrepend(prependors.ToArray()).AsImpureAssertion(context, st, "method call");
            }
        }

        private TranslationResult TranslateAsImplication(TranslationContext context)
        {
            if (MethodGroup is MemberAccessExpressionSyntax)
            {
                MemberAccessExpressionSyntax memberAccess = MethodGroup as MemberAccessExpressionSyntax;
                var leftExpression = RoslynToSharpnode.MapExpression(memberAccess.Expression);
                // TODO verify the purity
                var leftExpressionResult = leftExpression.Translate(context);
                var rightExpressionResult = Arguments[0].Translate(context);
                Silvernode implies = new BinaryExpressionSilvernode(
                    leftExpressionResult.Silvernode,
                    "==>",
                    rightExpressionResult.Silvernode,
                    OriginalNode
                    );
                var errors = new List<Error>();
                errors.AddRange(leftExpressionResult.Errors);
                errors.AddRange(rightExpressionResult.Errors);
                var prepedors = new List<StatementSilvernode>();
                prepedors.AddRange(leftExpressionResult.PrependTheseSilvernodes);
                prepedors.AddRange(rightExpressionResult.PrependTheseSilvernodes);
                return TranslationResult.FromSilvernode(implies, errors).AndPrepend(prepedors.ToArray());
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
            var conditionResult = Arguments[0].Translate(context.ChangePurityContext(PurityContext.PurityNotRequired));
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
                default:
                    return TranslationResult.Error(OriginalNode, Diagnostics.SSIL301_InternalLocalizedError, "unknown kind of verification condition");
            }
            return TranslationResult.FromSilvernode(result, conditionResult.Errors);
        }
    }
}