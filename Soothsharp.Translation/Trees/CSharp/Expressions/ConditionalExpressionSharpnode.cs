using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.Silver;
using System.Linq;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class ConditionalExpressionSharpnode : ExpressionSharpnode
    {
        private ExpressionSharpnode Condition;
        private ExpressionSharpnode WhenTrue;
        private ExpressionSharpnode WhenFalse;
        public ConditionalExpressionSharpnode(ConditionalExpressionSyntax syntax) : base(syntax)
        {
            this.Condition = RoslynToSharpnode.MapExpression(syntax.Condition);
            this.WhenTrue = RoslynToSharpnode.MapExpression(syntax.WhenTrue);
            this.WhenFalse = RoslynToSharpnode.MapExpression(syntax.WhenFalse);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var errors = new List<Error>();
            var cres = this.Condition.Translate(context);
            var a = cres.Silvernode;
            errors.AddRange(cres.Errors);
            var trueres = this.WhenTrue.Translate(context);
            var b = trueres.Silvernode;
            errors.AddRange(trueres.Errors);
            var falseres = this.WhenFalse.Translate(context);
            var c = falseres.Silvernode;
            errors.AddRange(falseres.Errors);
            return TranslationResult.FromSilvernode(new ConditionalExpressionSilvernode(
                a,
                b,
                c, this.OriginalNode
                ),
                errors).AndPrepend(cres.PrependTheseSilvernodes.Concat(trueres.PrependTheseSilvernodes).Concat(falseres.PrependTheseSilvernodes));
        }
    }
}