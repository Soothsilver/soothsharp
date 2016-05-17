using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
using System.Collections.Generic;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions
{
    public class BinaryExpressionSharpnode : ExpressionSharpnode
    {
        public string Operator;
        public ExpressionSharpnode Left;
        public ExpressionSharpnode Right;

        public BinaryExpressionSharpnode(BinaryExpressionSyntax syntax, string @operator) : base(syntax)
        {
            Operator = @operator;
            Left = RoslynToSharpnode.MapExpression(syntax.Left);
            Right = RoslynToSharpnode.MapExpression(syntax.Right);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var left = Left.Translate(context);
            var right = Right.Translate(context);
            IEnumerable<Error> errors = CommonUtils.CombineErrors(left, right);
            return TranslationResult.Silvernode(new BinaryExpressionSilvernode(left.SilverSourceTree, Operator, right.SilverSourceTree, OriginalNode), errors);
        }
    }
}