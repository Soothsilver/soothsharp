using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
using System.Collections.Generic;

namespace Sharpsilver.Translation.Trees.CSharp.Expressions
{
    public class PrefixUnaryExpressionSharpnode : ExpressionSharpnode
    {
        public string Operator;
        public ExpressionSharpnode Expression;

        public PrefixUnaryExpressionSharpnode(PrefixUnaryExpressionSyntax syntax, string @operator) : base(syntax)
        {
            Operator = @operator;
            Expression = RoslynToSharpnode.MapExpression(syntax.Operand);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var left = Expression.Translate(context);
            return TranslationResult.FromSilvernode(new PrefixUnaryExpressionSilvernode(Operator, left.Silvernode, OriginalNode), left.Errors);
        }
    }
}