using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Statements
{
    class ReturnStatementSharpnode : StatementSharpnode
    {
        private ExpressionSharpnode Expression;

        public ReturnStatementSharpnode(ReturnStatementSyntax stmt) : base(stmt)
        {
            if (stmt.Expression != null)
            {
                this.Expression = RoslynToSharpnode.MapExpression(stmt.Expression);
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var expr = this.Expression.Translate(context.ChangePurityContext(PurityContext.Purifiable));
            var errors = new List<Error>();
            errors.AddRange(expr.Errors);

            if (context.IsFunctionOrPredicateBlock)
            {
                return TranslationResult.FromSilvernode(expr.Silvernode, errors);
            }

            StatementsSequenceSilvernode statements = new StatementsSequenceSilvernode(this.OriginalNode);
            statements.List.AddRange(expr.PrependTheseSilvernodes);
            statements.List.Add(
                new AssignmentSilvernode(
                    new TextSilvernode(Constants.SilverReturnVariableName, this.OriginalNode),
                    expr.Silvernode, this.OriginalNode
                ));
            statements.List.Add(
                new GotoSilvernode(Constants.SilverMethodEndLabel, this.OriginalNode)
            );
            return TranslationResult.FromSilvernode(statements, errors);
        }
    }
}