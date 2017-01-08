using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class SimpleAssignmentExpressionSharpnode : ExpressionSharpnode
    {
        private ExpressionSharpnode Left;
        private ExpressionSharpnode Right;

        public SimpleAssignmentExpressionSharpnode(AssignmentExpressionSyntax syntax) : base(syntax)
        {
            this.Left = RoslynToSharpnode.MapExpression(syntax.Left);
            this.Right = RoslynToSharpnode.MapExpression(syntax.Right);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var left = this.Left.Translate(context); // TODO what if there are calls?
            var right = this.Right.Translate(context.ChangePurityContext(PurityContext.Purifiable));

            IEnumerable<Error> errors = CommonUtils.CombineErrors(left, right);

            var assignment = new AssignmentSilvernode(left.Silvernode, right.Silvernode, this.OriginalNode);
            var sequence = new StatementsSequenceSilvernode(null, right.PrependTheseSilvernodes.Union(new[] { assignment }).ToArray());

            return TranslationResult.FromSilvernode(sequence, errors);
        }
    }
}