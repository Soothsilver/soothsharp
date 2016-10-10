using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Sharpsilver.Translation.Trees.CSharp;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation
{
    internal class IdentifierExpressionSharpnode : ExpressionSharpnode
    {
        public ExpressionSyntax IdentifierName;

        public IdentifierExpressionSharpnode(ExpressionSyntax syntax) : base(syntax)
        {
            IdentifierName = syntax;
        }

        public ISymbol GetIdentifierSymbol(TranslationContext context)
        {
            SymbolInfo symbolInfo = context.Semantics.GetSymbolInfo(IdentifierName);
            ISymbol symbol = symbolInfo.Symbol;
            return symbol;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            ISymbol symbol = this.GetIdentifierSymbol(context);
            TranslationResult contractResult = context.Process.ContractsTranslator.TranslateIdentifierAsContract(symbol, IdentifierName, context);
            if (contractResult != null) return contractResult;

            var identifierNode = new IdentifierSilvernode(context.Process.IdentifierTranslator.GetIdentifierReference(symbol));
            if (symbol.ContainingSymbol.Kind == SymbolKind.Method)
            {
                return TranslationResult.FromSilvernode(identifierNode);
            }
            else if (symbol.ContainingSymbol.Kind == SymbolKind.NamedType)
            {
                return TranslationResult.FromSilvernode(new SimpleSequenceSilvernode(this.OriginalNode,
                    Constants.SilverThis,
                    ".",
                    identifierNode));
            }
            else
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL108_FeatureNotSupported, "members of unnamed types");
            }
        }
    }
}