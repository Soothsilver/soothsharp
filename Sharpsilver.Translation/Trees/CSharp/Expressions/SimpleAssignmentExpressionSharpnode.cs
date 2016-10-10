using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation.Trees.CSharp.Expressions
{
    public class SimpleAssignmentExpressionSharpnode : ExpressionSharpnode
    {
        public ExpressionSharpnode Left;
        public ExpressionSharpnode Right;

        public SimpleAssignmentExpressionSharpnode(AssignmentExpressionSyntax syntax) : base(syntax)
        {
            Left = RoslynToSharpnode.MapExpression(syntax.Left);
            Right = RoslynToSharpnode.MapExpression(syntax.Right);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var left = Left.Translate(context); // TODO what if there are calls?
            var right = Right.Translate(context.ChangePurityContext(PurityContext.Purifiable));

            IEnumerable<Error> errors = CommonUtils.CombineErrors(left, right);

            var assignment = new AssignmentSilvernode(left.Silvernode, right.Silvernode, OriginalNode);
            var sequence = new StatementsSequenceSilvernode(null, right.PrependTheseSilvernodes.Union(new[] { assignment }).ToArray());

            return TranslationResult.FromSilvernode(sequence, errors);
        }
    }
}