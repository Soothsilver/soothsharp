using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using System.Collections.Generic;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp.Statements;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver.Statements;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    internal class ForStatementSharpnode : StatementSharpnode
    {
        public List<StatementSharpnode> Initializers = new List<StatementSharpnode>();
        public List<ExpressionSharpnode> Incrementors = new List<ExpressionSharpnode>();
        public ExpressionSharpnode Condition;
        public StatementSharpnode Statement;

        public ForStatementSharpnode(ForStatementSyntax stmt) : base(stmt)
        {
            Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
            // Add declarations
            if (stmt.Declaration != null)
                Initializers.Add(new LocalDeclarationSharpnode(stmt.Declaration));
            // Add initializers
            /*
            if (stmt.Initializers != null) {
                foreach (var initializer in stmt.Initializers)
                {
                    Initializers.Add(new ExpressionStatementSharpnode(initializer));
                }
            }*/
            foreach (var incrementor in stmt.Incrementors)
            {
                Incrementors.Add(RoslynToSharpnode.MapExpression(incrementor));
            }
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var conditionResult = Condition.Translate(context);
            var statementResult = Statement.Translate(context);
            var statementBlock = ((StatementSilvernode) statementResult.Silvernode).EncloseInBlockIfNotAlready();
          
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            foreach (var incrementor in Incrementors)
            {
                var incrementorResult = incrementor.Translate(context);
                statementBlock.Add(incrementorResult.Silvernode as StatementSilvernode);
                errors.AddRange(incrementorResult.Errors);
            }
            errors.AddRange(statementResult.Errors);
            // TODO declaration, initializers, incrementors

            WhileSilvernode whileNode =
                new WhileSilvernode(
                    conditionResult.Silvernode,
                    statementResult.VerificationConditions,
                    statementBlock,
                    OriginalNode
                    );
            var sequence = new SequenceSilvernode(OriginalNode);
            foreach (var initializer in Initializers)
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