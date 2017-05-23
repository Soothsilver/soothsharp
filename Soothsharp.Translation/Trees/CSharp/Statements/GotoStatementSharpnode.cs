using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Statements
{
    class GotoStatementSharpnode : StatementSharpnode
    {
        private readonly ExpressionSharpnode Identifier;

        public GotoStatementSharpnode(GotoStatementSyntax stmt) : base(stmt)
        {
            this.Identifier = RoslynToSharpnode.MapExpression(stmt.Expression);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            if (!(this.Identifier is IdentifierExpressionSharpnode))
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL110_InvalidSyntax, "illegal goto target");
            }
            IdentifierExpressionSharpnode identifierExpression = (IdentifierExpressionSharpnode) this.Identifier;
            ISymbol symbol = identifierExpression.GetIdentifierSymbol(context);
            var identifier = context.Process.IdentifierTranslator.GetIdentifierReference(symbol);
            return TranslationResult.FromSilvernode(
                new GotoSilvernode(identifier, this.OriginalNode)
                );
        }
    }
}
