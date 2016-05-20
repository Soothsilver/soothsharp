using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Sharpsilver.Translation.AbstractSyntaxTrees.CSharp;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;
using Sharpsilver.Translation.Translators;

namespace Sharpsilver.Translation
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
                return TranslationResult.FromSilvernode(
                                new TextSilvernode(Constants.SilverReturnVariableName, IdentifierName)
                                );
            }
            return TranslationResult.FromSilvernode(
                new TextSilvernode(symbol.Symbol.Name, IdentifierName)
                );
        }
    }
}