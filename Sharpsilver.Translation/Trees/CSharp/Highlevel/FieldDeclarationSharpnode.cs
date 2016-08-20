using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation.Trees.CSharp.Highlevel
{
    public class FieldDeclarationSharpnode : Sharpnode
    {
        private FieldDeclarationSyntax fieldDeclarationSyntax;

        public ISymbol GetSymbol(SemanticModel semanticModel)
        {
            var symbol = semanticModel.GetDeclaredSymbol(fieldDeclarationSyntax.Declaration.Variables.First());
            return symbol;
        }

        public FieldDeclarationSharpnode(FieldDeclarationSyntax fieldDeclarationSyntax) : base(fieldDeclarationSyntax)
        {
            this.fieldDeclarationSyntax = fieldDeclarationSyntax;
        }



        public override TranslationResult Translate(TranslationContext context)
        {
            // This is not translated.
            return TranslationResult.FromSilvernode(new EmptySilvernode(fieldDeclarationSyntax));
        }
    }
}