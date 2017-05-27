using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    internal class DoStatementSharpnode : StatementSharpnode
    {
        private ExpressionSharpnode Condition;
        private StatementSharpnode Statement;

        public DoStatementSharpnode(DoStatementSyntax stmt) : base(stmt)
        {
            this.Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            this.Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var conditionResult = this.Condition.Translate(context);
            var statementResult = this.Statement.Translate(context);
            var statementBlock = ((StatementSilvernode)statementResult.Silvernode).EncloseInBlockIfNotAlready();
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            errors.AddRange(statementResult.Errors);
            return TranslationResult.FromSilvernode(
                new SimpleSequenceSilvernode(this.OriginalNode,
                statementBlock,
                "\n",
                new WhileSilvernode(
                    conditionResult.Silvernode,
                    statementResult.VerificationConditions,
                    statementBlock, null
                    )
                ), errors
                );
            // TODO add a unit test for the do statement
        }
    }
}