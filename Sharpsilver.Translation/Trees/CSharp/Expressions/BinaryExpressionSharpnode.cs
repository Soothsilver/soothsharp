using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation.Trees.CSharp.Expressions
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
            return TranslationResult.FromSilvernode(new BinaryExpressionSilvernode(left.Silvernode, Operator, right.Silvernode, OriginalNode), errors);
        }
    }
}