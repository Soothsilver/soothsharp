using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Soothsharp.Translation.Trees.CSharp;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    internal class MemberAccessExpressionSharpnode : ExpressionSharpnode
    {
        private MemberAccessExpressionSyntax Expression;
        public ExpressionSharpnode Container;

        public MemberAccessExpressionSharpnode(MemberAccessExpressionSyntax syntax) : base(syntax)
        {
            this.Expression = syntax;
            this.Container = RoslynToSharpnode.MapExpression(syntax.Expression);
        }


        public override TranslationResult Translate(TranslationContext context)
        {
            SymbolInfo symbolInfo = context.Semantics.GetSymbolInfo(this.Expression);
            ISymbol symbol = symbolInfo.Symbol;
            TranslationResult contractResult = context.Process.ContractsTranslator.TranslateIdentifierAsContract(symbol, this.Expression, context);
            if (contractResult != null) return contractResult;
            TranslationResult constantResult =
                context.Process.ConstantsTranslator.TranslateIdentifierAsConstant(symbol, this.Expression);
            if (constantResult != null) return constantResult;
            var errors = new List<Error>();
            var container = this.Container.Translate(context);
            if (symbol.GetQualifiedName() == SeqTranslator.SeqLength)
            {
                errors.AddRange(container.Errors);

                return TranslationResult.FromSilvernode(
                    new AbsoluteValueSilvernode(container.Silvernode, this.OriginalNode), errors
                    );
            }

            Identifier lastIdentifier = context.Process.IdentifierTranslator.GetIdentifierReference(symbol);

            errors.AddRange(container.Errors);

            return TranslationResult.FromSilvernode(
                new SimpleSequenceSilvernode(this.OriginalNode,
                    container.Silvernode,
                    ".",
                    new IdentifierSilvernode(lastIdentifier, this.Expression.Name)
                  ), errors
                );
            // TODO methods
        }
    }
}