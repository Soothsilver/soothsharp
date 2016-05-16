using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Translators;
using System.Collections.Generic;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions
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
            return TranslationResult.Silvernode(new PrefixUnaryExpressionSilvernode(Operator, left.SilverSourceTree, OriginalNode), left.Errors);
        }
    }
}