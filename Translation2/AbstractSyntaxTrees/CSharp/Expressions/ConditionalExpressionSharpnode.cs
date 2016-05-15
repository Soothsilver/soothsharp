using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Translators;
using System.Collections.Generic;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Expressions
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
            var a = cres.SilverSourceTree;
            errors.AddRange(cres.ReportedDiagnostics);
            var trueres = WhenTrue.Translate(context);
            var b = trueres.SilverSourceTree;
            errors.AddRange(trueres.ReportedDiagnostics);
            var falseres = WhenFalse.Translate(context);
            var c = falseres.SilverSourceTree;
            errors.AddRange(falseres.ReportedDiagnostics);
            return TranslationResult.Silvernode(new ConditionalExpressionSilvernode(
                a,
                b,
                c,
                OriginalNode
                ),
                errors);
        }
    }
}