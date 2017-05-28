using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp
{
    internal class IfStatementSharpnode : StatementSharpnode
    {
        private ExpressionSharpnode Condition;
        private StatementSharpnode Then;
        private StatementSharpnode Else;

        public IfStatementSharpnode(IfStatementSyntax stmt) : base(stmt)
        {
            this.Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            this.Then = RoslynToSharpnode.MapStatement(stmt.Statement);
            this.Else = stmt.Else != null ? RoslynToSharpnode.MapStatement(stmt.Else.Statement) : null;
        }
        public override TranslationResult Translate(TranslationContext context)
        {
            var conditionResult = this.Condition.Translate(context.ChangePurityContext(PurityContext.Purifiable));
            var thenResult = this.Then.Translate(context);
            var elseResult = this.Else?.Translate(context);
            var errors = new List<Error>();
            errors.AddRange(conditionResult.Errors);
            errors.AddRange(thenResult.Errors);
            if (elseResult != null)
                errors.AddRange(elseResult.Errors);

            Silvernode output= new IfSilvernode(this.OriginalNode,
                    conditionResult.Silvernode,
                    thenResult.Silvernode as StatementSilvernode,
                    elseResult?.Silvernode as StatementSilvernode);

            return TranslationResult.FromSilvernode(output, errors).AndPrepend(conditionResult.PrependTheseSilvernodes);
        }
    }
}