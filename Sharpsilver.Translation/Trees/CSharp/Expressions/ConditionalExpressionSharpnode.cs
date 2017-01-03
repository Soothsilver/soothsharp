using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class ConditionalExpressionSharpnode : ExpressionSharpnode
    {
        public ExpressionSharpnode Condition;
        public ExpressionSharpnode WhenTrue;
        public ExpressionSharpnode WhenFalse;
        public ConditionalExpressionSharpnode(ConditionalExpressionSyntax syntax) : base(syntax)
        {
            Condition = RoslynToSharpnode.MapExpression(syntax.Condition);
            WhenTrue = RoslynToSharpnode.MapExpression(syntax.WhenTrue);
            WhenFalse = RoslynToSharpnode.MapExpression(syntax.WhenFalse);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var errors = new List<Error>();
            var cres = Condition.Translate(context);
            var a = cres.Silvernode;
            errors.AddRange(cres.Errors);
            var trueres = WhenTrue.Translate(context);
            var b = trueres.Silvernode;
            errors.AddRange(trueres.Errors);
            var falseres = WhenFalse.Translate(context);
            var c = falseres.Silvernode;
            errors.AddRange(falseres.Errors);
            return TranslationResult.FromSilvernode(new ConditionalExpressionSilvernode(
                a,
                b,
                c,
                OriginalNode
                ),
                errors);
        }
    }
}