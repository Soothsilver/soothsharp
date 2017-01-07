using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.CSharp.Expressions;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    public class InvocationExpressionSharpnode : ExpressionSharpnode
    {
        private ExpressionSyntax MethodGroup;
        private ExpressionSharpnode methodGroupSharpnode;
        private List<ExpressionSharpnode> Arguments = new List<ExpressionSharpnode>();

        public InvocationExpressionSharpnode(InvocationExpressionSyntax syntax) : base(syntax)
        {
            this.MethodGroup = syntax.Expression;
            this.methodGroupSharpnode = RoslynToSharpnode.MapExpression(syntax.Expression);
            foreach (var argument in syntax.ArgumentList.Arguments)
            {
                this.Arguments.Add(RoslynToSharpnode.MapExpression(argument.Expression));
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            List<StatementSilvernode> prependors = new List<StatementSilvernode>();

            var method = context.Semantics.GetSymbolInfo(MethodGroup);
            var methodSymbol = method.Symbol as IMethodSymbol;
            if (methodSymbol == null) {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL123_ThereIsThisCSharpError, "This name cannot be unambiguously mapped to a single method.");
            }
            var methodName = methodSymbol.GetQualifiedName();

            // Verification conditions
            bool translateAsAbsoluteValue = false;
            bool translateAsPermissionCreate = false;
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
                case ContractsTranslator.ForAll:
                    return TranslateAsQuantifier(QuantifierKind.ForAll, context);
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
                case ContractsTranslator.PermissionCreate:
                    languageFeatureName = "!PERMISSION_CREATION!";
                    translateAsPermissionCreate = true;
                    break;
                case ContractsTranslator.PermissionFromLocation:
                    languageFeatureName = "perm";
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
                    if (this.methodGroupSharpnode is IdentifierExpressionSharpnode)
                    {
                        Arguments.Insert(0, new DirectSilvercodeExpressionSharpnode(Constants.SilverThis, MethodGroup));
                    }
                    else if (this.methodGroupSharpnode is MemberAccessExpressionSharpnode)
                    {
                        Arguments.Insert(0,((MemberAccessExpressionSharpnode)this.methodGroupSharpnode).Container);
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
            else if (translateAsAbsoluteValue)
            {
                return TranslationResult.FromSilvernode(
                  new AbsoluteValueSilvernode(expressions[0], this.OriginalNode)
                  , errors).AndPrepend(prependors.ToArray());
            }
            else if (translateAsPermissionCreate)
            {
                return TranslationResult.FromSilvernode(
                 new BinaryExpressionSilvernode(
                     expressions[0],
                     "/",
                     expressions[1],
                     this.OriginalNode)
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

        private TranslationResult TranslateAsQuantifier(QuantifierKind quantifierKind, TranslationContext context)
        {
            var errors = new List<Error>();
            int numArguments = this.Arguments.Count;
            if (numArguments != 1)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL301_InternalLocalizedError,
                    "incorrect number of arguments for a quantifier expression");
            }
            if (!(this.Arguments[0] is LambdaSharpnode))
            {
                return TranslationResult.Error(this.OriginalNode,
                    Diagnostics.SSIL124_QuantifierMustGetLambda);
            }
            LambdaSharpnode lambda = (LambdaSharpnode)this.Arguments[0];
            if (lambda.PrepareForInsertionIntoQuantifier(context))
            {
                TranslationResult result = TranslationResult.FromSilvernode(
                    new SimpleSequenceSilvernode(this.OriginalNode,
                        quantifierKind == QuantifierKind.ForAll ? "forall " : "exists ",
                        new IdentifierSilvernode(lambda.VariableIdentifier),
                        " : ",
                        new TypeSilvernode(null, lambda.VariableSilverType),
                        " :: ",
                        lambda.BodySilvernode
                        ),
                    errors
                    );
                return result;
            }
            else
            {
                return lambda.GetErrorTranslationResult();
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

    public enum QuantifierKind
    {
        ForAll,
        Exists
    }
}