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
            if (this.MethodGroup is MemberAccessExpressionSyntax)
            {
                MemberAccessExpressionSyntax memberAccess = this.MethodGroup as MemberAccessExpressionSyntax;
                var leftExpression = RoslynToSharpnode.MapExpression(memberAccess.Expression);
                // TODO verify the purity
                var leftExpressionResult = leftExpression.Translate(context);
                var rightExpressionResult = arguments[0].Translate(context);
                Silvernode implies = new BinaryExpressionSilvernode(
                    leftExpressionResult.Silvernode, this._operator,
                    rightExpressionResult.Silvernode,
                    originalNode
                    );
                this.Errors.AddRange(leftExpressionResult.Errors);
                this.Errors.AddRange(rightExpressionResult.Errors);
                this.Prependors.AddRange(leftExpressionResult.PrependTheseSilvernodes);
                this.Prependors.AddRange(rightExpressionResult.PrependTheseSilvernodes);
                this.Silvernode = implies;
            }
            else
            {
                this.Errors.Add(new Error(Diagnostics.SSIL110_InvalidSyntax, originalNode,
                    "member access expression expected"));
                this.Silvernode = new TextSilvernode(Constants.SilverErrorString, originalNode);
            }
        }
    }
}