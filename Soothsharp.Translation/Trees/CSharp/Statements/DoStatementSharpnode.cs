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
            // Translate parts
            var conditionResult = this.Condition.Translate(context.ChangePurityContext(PurityContext.Purifiable));
            var statementResult = this.Statement.Translate(context);

            // Put them together
            var statementBlock = ((StatementSilvernode)statementResult.Silvernode).EncloseInBlockIfNotAlready();
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            errors.AddRange(statementResult.Errors);

            statementBlock.Statements.AddRange(conditionResult.PrependTheseSilvernodes);

            return TranslationResult.FromSilvernode(
                new SimpleSequenceSilvernode(this.OriginalNode,
                new StatementsSequenceSilvernode(statementBlock.OriginalNode,
                statementBlock.Statements.ToArray()),
                "\n",
                new StatementsSequenceSilvernode(null, conditionResult.PrependTheseSilvernodes.ToArray()),
                new WhileSilvernode(
                    conditionResult.Silvernode,
                    statementResult.Contracts,
                    statementBlock, null
                    )
                ), errors
                );
        }
    }
}