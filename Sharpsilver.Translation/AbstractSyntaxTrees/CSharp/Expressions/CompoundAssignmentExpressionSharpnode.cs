using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions
{
    internal class CompoundAssignmentExpressionSharpnode : ExpressionSharpnode
    {
        private readonly string operation;

        public CompoundAssignmentExpressionSharpnode(AssignmentExpressionSyntax syntax, string operation) 
            : base(syntax)
        {
            this.operation = operation;
            Left = RoslynToSharpnode.MapExpression(syntax.Left);
            Right = RoslynToSharpnode.MapExpression(syntax.Right);
        }

        public ExpressionSharpnode Right { get; set; }

        public ExpressionSharpnode Left { get; set; }

        public override TranslationResult Translate(TranslationContext context)
        {
            var left = Left.Translate(context);
            var right = Right.Translate(context);
            IEnumerable<Error> errors = CommonUtils.CombineErrors(left, right);
            return 
                TranslationResult.FromSilvernode(
                    new AssignmentSilvernode(
                        left.Silvernode,
                        new BinaryExpressionSilvernode(left.Silvernode, operation, right.Silvernode, OriginalNode),
                    
                    OriginalNode), errors);

        }
    }
}