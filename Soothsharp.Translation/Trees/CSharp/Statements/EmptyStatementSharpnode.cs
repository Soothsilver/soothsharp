using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Statements
{
    class EmptyStatementSharpnode : StatementSharpnode
    {
        public EmptyStatementSharpnode(EmptyStatementSyntax syntax) : base(syntax)
        {
            
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            return TranslationResult.FromSilvernode(new TextStatementSilvernode("", this.OriginalNode));
        }
    }
}
