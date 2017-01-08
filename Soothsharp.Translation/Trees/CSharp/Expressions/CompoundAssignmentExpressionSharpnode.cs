using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    internal class CompoundAssignmentExpressionSharpnode : ExpressionSharpnode
    {
        private readonly string operation;

        public CompoundAssignmentExpressionSharpnode(AssignmentExpressionSyntax syntax, string operation) 
            : base(syntax)
        {
            this.operation = operation;
            this.Left = RoslynToSharpnode.MapExpression(syntax.Left);
            this.Right = RoslynToSharpnode.MapExpression(syntax.Right);
        }

        private ExpressionSharpnode Right { get; }

        private ExpressionSharpnode Left { get; }

        public override TranslationResult Translate(TranslationContext context)
        {
            var left = this.Left.Translate(context);
            var right = this.Right.Translate(context);
            IEnumerable<Error> errors = CommonUtils.CombineErrors(left, right);
            return 
                TranslationResult.FromSilvernode(
                    new AssignmentSilvernode(
                        left.Silvernode,
                        new BinaryExpressionSilvernode(left.Silvernode, this.operation, right.Silvernode, this.OriginalNode), this.OriginalNode), errors);

        }
    }
}