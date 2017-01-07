using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.CSharp.Invocation;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationImplicationEquivalence : InvocationTranslation
    {
        private string _operator;
        private readonly ExpressionSyntax MethodGroup;

        public InvocationImplicationEquivalence(string @operator, ExpressionSyntax methodGroup)
        {
            this._operator = @operator;
            this.MethodGroup = methodGroup;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            if (MethodGroup is MemberAccessExpressionSyntax)
            {
                MemberAccessExpressionSyntax memberAccess = MethodGroup as MemberAccessExpressionSyntax;
                var leftExpression = RoslynToSharpnode.MapExpression(memberAccess.Expression);
                // TODO verify the purity
                var leftExpressionResult = leftExpression.Translate(context);
                var rightExpressionResult = arguments[0].Translate(context);
                Silvernode implies = new BinaryExpressionSilvernode(
                    leftExpressionResult.Silvernode,
                    _operator,
                    rightExpressionResult.Silvernode,
                    originalNode
                    );
                Errors.AddRange(leftExpressionResult.Errors);
                Errors.AddRange(rightExpressionResult.Errors);
                Prependors.AddRange(leftExpressionResult.PrependTheseSilvernodes);
                Prependors.AddRange(rightExpressionResult.PrependTheseSilvernodes);
                Silvernode = implies;
            }
            else
            {
                Errors.Add(new Error(Diagnostics.SSIL110_InvalidSyntax, originalNode,
                    "member access expression expected"));
                Silvernode = new TextSilvernode(Constants.SilverErrorString, originalNode);
            }
        }
    }
}