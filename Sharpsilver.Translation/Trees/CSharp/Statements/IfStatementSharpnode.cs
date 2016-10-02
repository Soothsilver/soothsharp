using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.CSharp;
using Sharpsilver.Translation.Trees.Silver;
using Sharpsilver.Translation;
using System.Collections.Generic;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.CSharp
{
    internal class IfStatementSharpnode : StatementSharpnode
    {
        public ExpressionSharpnode Condition;
        public StatementSharpnode Then;
        public StatementSharpnode Else;

        public IfStatementSharpnode(IfStatementSyntax stmt) : base(stmt)
        {
            Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            Then = RoslynToSharpnode.MapStatement(stmt.Statement);
            Else = stmt.Else != null ? RoslynToSharpnode.MapStatement(stmt.Else.Statement) : null;
        }
        public override TranslationResult Translate(TranslationContext context)
        {
            var conditionResult = Condition.Translate(context);
            var thenResult = Then.Translate(context);
            var elseResult = Else != null ? Else.Translate(context) : null;
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            errors.AddRange(thenResult.Errors);
            if (elseResult != null)
                errors.AddRange(elseResult.Errors);

            return TranslationResult.FromSilvernode(new IfSilvernode(
                OriginalNode,
                conditionResult.Silvernode,
                thenResult.Silvernode as StatementSilvernode,
                elseResult?.Silvernode as StatementSilvernode),
                errors);
        }
    }
}