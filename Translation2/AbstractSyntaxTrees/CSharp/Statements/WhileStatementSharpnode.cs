using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using System.Collections.Generic;

namespace Sharpsilver.Translation
{
    internal class WhileStatementSharpnode : StatementSharpnode
    {
        public ExpressionSharpnode Condition;
        public StatementSharpnode Statement;

        public WhileStatementSharpnode(WhileStatementSyntax stmt) : base(stmt)
        {
            Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var conditionResult = Condition.Translate(context);
            var statementResult = Statement.Translate(context);
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            errors.AddRange(statementResult.Errors);
            return TranslationResult.Silvernode(
                new WhileSilvernode(
                    conditionResult.SilverSourceTree,
                    statementResult.SilverSourceTree,
                    OriginalNode
                    )
                );
        }
    }
}