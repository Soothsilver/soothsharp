using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Statements
{
    class ReturnStatementSharpnode : StatementSharpnode
    {
        public ExpressionSharpnode Expression;

        public ReturnStatementSharpnode(ReturnStatementSyntax stmt) : base(stmt)
        {
            if (stmt.Expression != null)
            {
                Expression = RoslynToSharpnode.MapExpression(stmt.Expression);
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var expr = Expression.Translate(context);
            var errors = new List<Error>();
            errors.AddRange(expr.Errors);

            if (context.IsFunctionOrPredicateBlock)
            {
                return TranslationResult.FromSilvernode(expr.Silvernode, errors);
            }

            StatementsSequenceSilvernode statements = new StatementsSequenceSilvernode(OriginalNode);
            statements.List.Add(
                new AssignmentSilvernode(
                    new TextSilvernode(Constants.SilverReturnVariableName, OriginalNode),
                    expr.Silvernode,
                    OriginalNode
                ));
            statements.List.Add(
                new GotoSilvernode(Constants.SilverMethodEndLabel, OriginalNode)
            );
            return TranslationResult.FromSilvernode(statements, errors);
        }
    }
}