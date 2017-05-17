using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.CSharp.Expressions;
using Soothsharp.Translation.Trees.CSharp.Expressions.Invocation;
using Soothsharp.Translation.Trees.CSharp.Invocation;
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
            var method = context.Semantics.GetSymbolInfo(this.MethodGroup);
            var methodSymbol = method.Symbol as IMethodSymbol;
            if (methodSymbol == null)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL123_ThereIsThisCSharpError,
                    "This name cannot be unambiguously mapped to a single method.");
            }
            var methodName = methodSymbol.GetQualifiedName();

            InvocationTranslation translationStyle;
            switch (methodName)
            {

                case ContractsTranslator.ContractEnsures:
                case ContractsTranslator.ContractRequires:
                case ContractsTranslator.ContractInvariant:
                    translationStyle = new InvocationVerificationCondition(methodName);
                    break;
                case ContractsTranslator.Implication:
                    translationStyle = new InvocationImplicationEquivalence("==>", this.MethodGroup);
                    break;
                case ContractsTranslator.Equivalence:
                    translationStyle = new InvocationImplicationEquivalence("<==>", this.MethodGroup);
                    break;
                case ContractsTranslator.ForAll:
                    translationStyle = new InvocationQuantifier(QuantifierKind.ForAll);
                    break;
                case ContractsTranslator.Exists:
                    translationStyle = new InvocationQuantifier(QuantifierKind.Exists);
                    break;
                case ContractsTranslator.ContractAssert:
                    translationStyle = new InvocationPhpStatement("assert");
                    break;
                case ContractsTranslator.ContractAssume:
                    translationStyle = new InvocationPhpStatement("assume");
                    break;
                case ContractsTranslator.ContractInhale:
                    translationStyle = new InvocationPhpStatement("inhale");
                    break;
                case ContractsTranslator.ContractExhale:
                    translationStyle = new InvocationPhpStatement("exhale");
                    break;
                case ContractsTranslator.ContractAcc:
                    translationStyle = new InvocationViperBuiltInFunction("acc", false);
                    break;
                case ContractsTranslator.ContractAccArray:
                    translationStyle = new InvocationAccArray();
                    break;
                case ContractsTranslator.Old:
                    translationStyle = new InvocationViperBuiltInFunction("old", false);
                    break;

                case ContractsTranslator.Fold:
                    translationStyle = new InvocationPhpStatement("fold");
                    break;
                case ContractsTranslator.Unfold:
                    translationStyle = new InvocationPhpStatement("unfold");
                    break;
                case ContractsTranslator.PermissionCreate:
                    translationStyle = new InvocationPermissionCreate();
                    break;
                case ContractsTranslator.PermissionFromLocation:
                    translationStyle = new InvocationViperBuiltInFunction("perm", false);
                    break;
                case SeqTranslator.Contains:
                    translationStyle = new InvocationSeqContains(this.MethodGroup, this.methodGroupSharpnode);
                    break;
                case SeqTranslator.Drop:
                    translationStyle = new InvocationSeqTakeDrop(false, true, this.MethodGroup, this.methodGroupSharpnode);
                    break;
                case SeqTranslator.Take:
                    translationStyle = new InvocationSeqTakeDrop(true, false, this.MethodGroup, this.methodGroupSharpnode);
                    break;
                case SeqTranslator.TakeDrop:
                    translationStyle = new InvocationSeqTakeDrop(true, true, this.MethodGroup, this.methodGroupSharpnode);
                    break;
                case ContractsTranslator.Result:
                    translationStyle = new InvocationResult();
                    break;
                case ContractsTranslator.Folding:
                    translationStyle = new InvocationFoldingUnfolding(true);
                    break;
                case ContractsTranslator.Unfolding:
                    translationStyle = new InvocationFoldingUnfolding(false);
                    break;
                default:
                    translationStyle = new InvocationStandardMethod(this.MethodGroup, this.methodGroupSharpnode, method);
                    break;
            }

            translationStyle.Run(this.Arguments, this.OriginalNode, context);
            Silvernode silvernode = translationStyle.Silvernode;

            TranslationResult result = TranslationResult.FromSilvernode(silvernode,
                translationStyle.Errors).AndPrepend(translationStyle.Prependors.ToArray());

            translationStyle.PostprocessPurity(result, context);
            return result;
        }
    }
}