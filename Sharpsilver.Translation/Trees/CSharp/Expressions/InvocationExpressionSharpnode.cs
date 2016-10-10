using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation.Trees.CSharp.Expressions;
using Microsoft.CodeAnalysis.CSharp;

namespace Sharpsilver.Translation.Trees.CSharp
{
    public class InvocationExpressionSharpnode : ExpressionSharpnode
    {
        public ExpressionSyntax MethodGroup;
        private ExpressionSharpnode MethodGroupSharpnode;
        public List<ExpressionSharpnode> Arguments = new List<ExpressionSharpnode>();

        public InvocationExpressionSharpnode(InvocationExpressionSyntax syntax) : base(syntax)
        {
            this.MethodGroup = syntax.Expression;
            MethodGroupSharpnode = RoslynToSharpnode.MapExpression(syntax.Expression);
            foreach (var argument in syntax.ArgumentList.Arguments)
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
            bool isImpure = true;
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
                    isImpure = false;
                    languageFeatureName = "acc";
                    break;
                case ContractsTranslator.Old:
                    isImpure = false;
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

            var expressions = new List<Silvernode>();
            var errors = new List<Error>();
            // Get identifier and evaluate arguments
            Error error;
            Identifier identifier;
            SilverType st;
            if (languageFeatureName == null)
            {
                identifier = context.Process.IdentifierTranslator.GetIdentifierReference(method.Symbol as IMethodSymbol);
                IMethodSymbol theMethod = (method.Symbol as IMethodSymbol);
                if (ContractsTranslator.IsMethodPureOrPredicate(theMethod))
                {
                    isImpure = false;
                }
                if (!theMethod.IsStatic)
                {
                    if (MethodGroupSharpnode is IdentifierExpressionSharpnode)
                    {
                        Arguments.Insert(0, new DirectSilvercodeExpressionSharpnode(Constants.SilverThis, MethodGroup));
                    }
                    else if (MethodGroupSharpnode is MemberAccessExpressionSharpnode)
                    {
                        Arguments.Insert(0,((MemberAccessExpressionSharpnode)MethodGroupSharpnode).Container);
                    }
                    else
                    {
                        errors.Add(new Error(Diagnostics.SSIL102_UnexpectedNode, MethodGroup, MethodGroup.Kind()));
                    }
                }
                st = TypeTranslator.TranslateType(methodSymbol.ReturnType, MethodGroup, out error);
            }
            else
            {
                identifier = new Identifier(languageFeatureName);
                st = SilverType.Bool;
                error = null;
            }
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

                TranslationResult result = TranslationResult.FromSilvernode(
                    invocationSilvernode,
                    errors
                    ).AndPrepend(prependors.ToArray());
                if (isImpure)
                {
                    return result.AsImpureAssertion(context, st, "method call");
                } else
                {
                    return result;
                }
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