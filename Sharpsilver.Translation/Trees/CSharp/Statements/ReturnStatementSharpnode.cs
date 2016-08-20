using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.CSharp.Statements
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
            SequenceSilvernode statements = new SequenceSilvernode(OriginalNode);
            var expr = Expression.Translate(context);
            var errors = new List<Error>();
            errors.AddRange(expr.Errors);
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