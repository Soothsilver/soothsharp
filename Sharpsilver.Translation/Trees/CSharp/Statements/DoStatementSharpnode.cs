using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;

namespace Sharpsilver.Translation
{
    internal class DoStatementSharpnode : StatementSharpnode
    {
        public ExpressionSharpnode Condition;
        public StatementSharpnode Statement;

        public DoStatementSharpnode(DoStatementSyntax stmt) : base(stmt)
        {
            Condition = RoslynToSharpnode.MapExpression(stmt.Condition);
            Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            return TranslationResult.Error(OriginalNode, Diagnostics.SSIL105_FeatureNotYetSupported, "do statement");
        }
    }
}