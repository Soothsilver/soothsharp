using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
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
            var methodName = method.Symbol.GetQualifiedName();
            if (methodName == ContractsTranslator.CONTRACT_ENSURES)
            {
                var argex = Arguments[0].Translate(context);
                EnsuresSilvernode ensures = new EnsuresSilvernode(argex.SilverSourceTree, OriginalNode);
                return TranslationResult.Silvernode(ensures, argex.ReportedDiagnostics);
                // TODO more checks etc. etc.
            }
            return TranslationResult.Error(OriginalNode, Diagnostics.SSIL105_FeatureNotYetSupported,
                "methods named " + methodName);
        }
    }
}