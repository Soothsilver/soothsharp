using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.CSharp.Statements;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    internal class ForStatementSharpnode : StatementSharpnode
    {
        private List<StatementSharpnode> Initializers = new List<StatementSharpnode>();
        private List<ExpressionSharpnode> Incrementors = new List<ExpressionSharpnode>();
        private ExpressionSharpnode Condition;
        private StatementSharpnode Statement;

        public ForStatementSharpnode(ForStatementSyntax stmt) : base(stmt)
        {
            this.Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            this.Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
            // Add declarations
            if (stmt.Declaration != null)
                this.Initializers.Add(new LocalDeclarationSharpnode(stmt.Declaration));
            // Add initializers
            
            if (stmt.Initializers != null) {
                foreach (var initializer in stmt.Initializers)
                {
                    Initializers.Add(new ExpressionStatementSharpnode(initializer));
                }
            }
            foreach (var incrementor in stmt.Incrementors)
            {
                this.Incrementors.Add(RoslynToSharpnode.MapExpression(incrementor));
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var conditionResult = this.Condition.Translate(context);
            var statementResult = this.Statement.Translate(context);
            var statementBlock = ((StatementSilvernode) statementResult.Silvernode).EncloseInBlockIfNotAlready();
          
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            foreach (var incrementor in this.Incrementors)
            {
                var incrementorResult = incrementor.Translate(context);
                statementBlock.Add(incrementorResult.Silvernode as StatementSilvernode);
                errors.AddRange(incrementorResult.Errors);
            }
            errors.AddRange(statementResult.Errors);
            // TODO purifiability?

            WhileSilvernode whileNode =
                new WhileSilvernode(
                    conditionResult.Silvernode,
                    statementResult.VerificationConditions,
                    statementBlock, this.OriginalNode
                    );
            var sequence = new StatementsSequenceSilvernode(this.OriginalNode);
            foreach (var initializer in this.Initializers)
            {
                var initializerResult = initializer.Translate(context);
                errors.AddRange(initializerResult.Errors);
                sequence.List.Add(initializerResult.Silvernode as StatementSilvernode);
            }
            sequence.List.Add(whileNode);
            return TranslationResult.FromSilvernode(sequence, errors);
        }
    }
}