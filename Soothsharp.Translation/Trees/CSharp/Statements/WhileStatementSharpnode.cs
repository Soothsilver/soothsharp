using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    internal class WhileStatementSharpnode : StatementSharpnode
    {
        private ExpressionSharpnode Condition;
        private StatementSharpnode Statement;

        public WhileStatementSharpnode(WhileStatementSyntax stmt) : base(stmt)
        {
            this.Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            this.Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var conditionResult = this.Condition.Translate(context.ChangePurityContext(PurityContext.Purifiable));
            var statementResult = this.Statement.Translate(context);
            var statementBlock = ((StatementSilvernode)statementResult.Silvernode).EncloseInBlockIfNotAlready();
            statementBlock.Statements.AddRange(conditionResult.PrependTheseSilvernodes);
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            errors.AddRange(statementResult.Errors);
            return TranslationResult.FromSilvernode(
                new WhileSilvernode(
                    conditionResult.Silvernode,
                    statementResult.Contracts,
                    statementBlock, this.OriginalNode
                    ),
                errors
                ).AndPrepend(conditionResult.PrependTheseSilvernodes);
        }
    }
}