using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using System.Collections.Generic;

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
            var elseResult = Else?.Translate(context);
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