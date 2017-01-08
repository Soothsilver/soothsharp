using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    public class IncrementExpressionSharpnode : ExpressionSharpnode
    {
        private IncrementExpressionDirection direction;
#pragma warning disable 414
        // TODO make this work
        private IncrementExpressionOrder order;
#pragma warning restore 414
        private ExpressionSharpnode expression;

        public IncrementExpressionSharpnode(PrefixUnaryExpressionSyntax syntax, IncrementExpressionDirection direction) : base(syntax)
        {
            this.direction = direction;
            this.order = IncrementExpressionOrder.Pre;
            this.expression = RoslynToSharpnode.MapExpression(syntax.Operand);
        }
        public IncrementExpressionSharpnode(PostfixUnaryExpressionSyntax syntax, IncrementExpressionDirection direction) : base(syntax)
        {
            this.direction = direction;
            this.order = IncrementExpressionOrder.Post;
            this.expression = RoslynToSharpnode.MapExpression(syntax.Operand);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var translatedExpression = this.expression.Translate(context);
            // Statement form only.
            // TODO what if it's not a Silver lvalue?
            string @operator = this.direction == IncrementExpressionDirection.Increment ? "+" : "-";
            return TranslationResult.FromSilvernode(
                new AssignmentSilvernode(
                    translatedExpression.Silvernode, 
                    new BinaryExpressionSilvernode(translatedExpression.Silvernode, @operator, new TextSilvernode("1"), this.OriginalNode), this.OriginalNode), translatedExpression.Errors);
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