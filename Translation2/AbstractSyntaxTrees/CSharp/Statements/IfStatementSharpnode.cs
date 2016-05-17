using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using Sharpsilver.Translation;
using System.Collections.Generic;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
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
            return TranslationResult.Silvernode(new IfSilvernode(
                OriginalNode,
                conditionResult.SilverSourceTree,
                thenResult.SilverSourceTree,
                elseResult.SilverSourceTree),
                errors);
        }
    }
}