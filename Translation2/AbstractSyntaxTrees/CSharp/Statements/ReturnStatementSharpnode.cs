using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Statements
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
            StatementsSilvernode statements = new StatementsSilvernode(OriginalNode);
            var expr = Expression.Translate(context);
            var errors = new List<Error>();
            errors.AddRange(expr.Errors);
            statements.Add(
                new AssignmentSilvernode(
                    new TextSilvernode(Constants.SILVER_RETURN_VARIABLE_NAME, OriginalNode),
                    expr.SilverSourceTree,
                    OriginalNode
                ));
            statements.Add(
                new GotoSilvernode(Constants.SILVER_METHOD_END_LABEL, OriginalNode)
            );
            return TranslationResult.Silvernode(statements, errors);
        }
    }
}