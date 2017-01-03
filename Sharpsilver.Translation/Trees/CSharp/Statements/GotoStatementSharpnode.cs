using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Statements
{
    class GotoStatementSharpnode : StatementSharpnode
    {
        public ExpressionSharpnode Identifier;

        public GotoStatementSharpnode(GotoStatementSyntax stmt) : base(stmt)
        {
            Identifier = RoslynToSharpnode.MapExpression(stmt.Expression);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            if (!(Identifier is IdentifierExpressionSharpnode))
            {
                return TranslationResult.Error(OriginalNode, Diagnostics.SSIL110_InvalidSyntax, "illegal goto target");
            }
            IdentifierExpressionSharpnode identifierExpression = Identifier as IdentifierExpressionSharpnode;
            ISymbol symbol = identifierExpression.GetIdentifierSymbol(context);
            var identifier = context.Process.IdentifierTranslator.GetIdentifierReference(symbol);
            return TranslationResult.FromSilvernode(
                new GotoSilvernode(identifier, OriginalNode)
                );
        }
    }
}
