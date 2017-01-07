using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class BinaryExpressionSharpnode : ExpressionSharpnode
    {
        private readonly BinaryExpressionSyntax _syntax;
        public string Operator;
        public ExpressionSharpnode Left;
        public ExpressionSharpnode Right;

        public BinaryExpressionSharpnode(BinaryExpressionSyntax syntax, string @operator) : base(syntax)
        {
            this._syntax = syntax;
            Operator = @operator;
            Left = RoslynToSharpnode.MapExpression(syntax.Left);
            Right = RoslynToSharpnode.MapExpression(syntax.Right);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            SymbolInfo symbolInfo = context.Semantics.GetSymbolInfo(this._syntax);
            ISymbol symbol = symbolInfo.Symbol;
            string qualifiedName = symbol.GetQualifiedName();
            string operatorName = qualifiedName == SeqTranslator.OperatorPlus ? "++" : Operator;
            var left = Left.Translate(context);
            var right = Right.Translate(context);
            IEnumerable<Error> errors = CommonUtils.CombineErrors(left, right);
            return TranslationResult.FromSilvernode(new BinaryExpressionSilvernode(left.Silvernode, operatorName, right.Silvernode, OriginalNode), errors);
        }
    }
}