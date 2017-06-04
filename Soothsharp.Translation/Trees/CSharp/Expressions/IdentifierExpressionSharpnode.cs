using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    internal class IdentifierExpressionSharpnode : ExpressionSharpnode
    {
        private ExpressionSyntax IdentifierName;

        public IdentifierExpressionSharpnode(ExpressionSyntax syntax) : base(syntax)
        {
            this.IdentifierName = syntax;
        }

        public ISymbol GetIdentifierSymbol(TranslationContext context)
        {
            SymbolInfo symbolInfo = context.Semantics.GetSymbolInfo(this.IdentifierName);
            ISymbol symbol = symbolInfo.Symbol;
            return symbol;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            ISymbol symbol = GetIdentifierSymbol(context);

            // Special case translations first:
            TranslationResult contractResult = context.Process.ContractsTranslator.TranslateIdentifierAsContract(symbol, this.IdentifierName, context);
            if (contractResult != null) return contractResult;
            TranslationResult constantResult =
                context.Process.ConstantsTranslator.TranslateIdentifierAsConstant(symbol, this.IdentifierName);
            if (constantResult != null) return constantResult;

            // Normal translation:
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