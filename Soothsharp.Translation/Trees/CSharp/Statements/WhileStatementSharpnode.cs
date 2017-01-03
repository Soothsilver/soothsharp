using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
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
            var statementBlock = ((StatementSilvernode)statementResult.Silvernode).EncloseInBlockIfNotAlready();
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            errors.AddRange(statementResult.Errors);
            return TranslationResult.FromSilvernode(
                new WhileSilvernode(
                    conditionResult.Silvernode,
                    statementResult.VerificationConditions,
                    statementBlock,
                    OriginalNode
                    ),
                errors
                );
        }
    }
}