using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Trees.Silver;
using Microsoft.CodeAnalysis.CSharp;

namespace Sharpsilver.Translation.Trees.CSharp.Statements
{
    class LabeledStatementSharpnode : StatementSharpnode
    {
        public StatementSharpnode Statement;
        public LabeledStatementSyntax Self;
        public LabeledStatementSharpnode(LabeledStatementSyntax stmt) : base(stmt)
        {
            Self = stmt;
            Statement = RoslynToSharpnode.MapStatement(stmt.Statement);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            var symbol = context.Semantics.GetDeclaredSymbol(Self);
            var identifier = context.Process.IdentifierTranslator.RegisterAndGetIdentifier(symbol);
            var statementResult = Statement.Translate(context);
            StatementsSequenceSilvernode seq = new StatementsSequenceSilvernode(OriginalNode, 
                new LabelSilvernode(identifier, OriginalNode),
                (StatementSilvernode) statementResult.Silvernode 
                );
            return TranslationResult.FromSilvernode(seq, statementResult.Errors);
        }
    }
}
