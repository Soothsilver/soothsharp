using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.CSharp.Expressions
{
    public class IncrementExpressionSharpnode : ExpressionSharpnode
    {
        private IncrementExpressionDirection Direction;
#pragma warning disable 414
        // TODO make this work
        private IncrementExpressionOrder Order;
#pragma warning restore 414
        private ExpressionSharpnode Expression;

        public IncrementExpressionSharpnode(PrefixUnaryExpressionSyntax syntax, IncrementExpressionDirection direction) : base(syntax)
        {
            Direction = direction;
            Order = IncrementExpressionOrder.Pre;
            Expression = RoslynToSharpnode.MapExpression(syntax.Operand);
        }
        public IncrementExpressionSharpnode(PostfixUnaryExpressionSyntax syntax, IncrementExpressionDirection direction) : base(syntax)
        {
            Direction = direction;
            Order = IncrementExpressionOrder.Post;
            Expression = RoslynToSharpnode.MapExpression(syntax.Operand);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var expression = Expression.Translate(context);
            // Statement form only.
            string @operator = Direction == IncrementExpressionDirection.Increment ? "+" : "-";
            return TranslationResult.FromSilvernode(new AssignmentSilvernode(new TextSilvernode(expression.Silvernode.ToString(), expression.Silvernode.OriginalNode), 
                new BinaryExpressionSilvernode(expression.Silvernode, @operator, new TextSilvernode("1", null), OriginalNode), OriginalNode), expression.Errors);
        }
    }

    public enum IncrementExpressionOrder
    {
        Pre,
        Post
    }

    public enum IncrementExpressionDirection
    {
        Increment,
        Decrement
    }
}