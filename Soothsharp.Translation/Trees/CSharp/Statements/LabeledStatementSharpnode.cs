using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Statements
{
    class LabeledStatementSharpnode : StatementSharpnode
    {
        private StatementSharpnode Statement;
        private LabeledStatementSyntax Self;
        public LabeledStatementSharpnode(LabeledStatementSyntax stmt) : base(stmt)
        {
            this.Self = stmt;
            this.Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var symbol = context.Semantics.GetDeclaredSymbol(this.Self);
            var identifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifier(symbol);
            var statementResult = this.Statement.Translate(context);
            StatementsSequenceSilvernode seq = new StatementsSequenceSilvernode(this.OriginalNode, 
                new LabelSilvernode(identifier, this.OriginalNode),
                (StatementSilvernode) statementResult.Silvernode 
                );
            return TranslationResult.FromSilvernode(seq, statementResult.Errors);
        }
    }
}
