using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.Translators
{
    internal class IdentifierExpressionSharpnode : ExpressionSharpnode
    {
        public ExpressionSyntax IdentifierName;
        public IdentifierExpressionSharpnode(ExpressionSyntax syntax) : base(syntax)
        {
            IdentifierName = syntax;
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            SymbolInfo symbol = context.Semantics.GetSymbolInfo(IdentifierName);
            if (symbol.Symbol.GetQualifiedName() == ContractsTranslator.CONTRACT_INT_RESULT)
            {
                return TranslationResult.Silvernode(
                                new TextSilvernode(Constants.SILVER_RETURN_VARIABLE_NAME, IdentifierName)
                                );
            }
            return TranslationResult.Silvernode(
                new TextSilvernode(symbol.Symbol.Name, IdentifierName)
                );
        }
    }
}