using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    public class BinaryExpressionSharpnode : ExpressionSharpnode
    {
        private readonly BinaryExpressionSyntax _syntax;
        private string Operator;
        private ExpressionSharpnode Left;
        private ExpressionSharpnode Right;

        public BinaryExpressionSharpnode(BinaryExpressionSyntax syntax, string @operator) : base(syntax)
        {
            this._syntax = syntax;
            this.Operator = @operator;
            this.Left = RoslynToSharpnode.MapExpression(syntax.Left);
            this.Right = RoslynToSharpnode.MapExpression(syntax.Right);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            SymbolInfo symbolInfo = context.Semantics.GetSymbolInfo(this._syntax);
            ISymbol symbol = symbolInfo.Symbol;
            string qualifiedName = symbol?.GetQualifiedName();
            string operatorName = qualifiedName == SeqTranslator.OperatorPlus ? "++" : this.Operator;
            var left = this.Left.Translate(context);
            var right = this.Right.Translate(context);
            List<Error> errors = CommonUtils.CombineErrors(left, right).ToList();
            if (Right is SimpleAssignmentExpressionSharpnode ||
                Right is CompoundAssignmentExpressionSharpnode ||
                Right is IncrementExpressionSharpnode)
            {
                errors.Add(new Error(Diagnostics.SSIL131_AssignmentsNotInsideExpressions,
                    Right.OriginalNode));
            }
            if (Left is SimpleAssignmentExpressionSharpnode ||
                Left is CompoundAssignmentExpressionSharpnode ||
                Left is IncrementExpressionSharpnode)
            {
                errors.Add(new Error(Diagnostics.SSIL131_AssignmentsNotInsideExpressions,
                    Left.OriginalNode));
            }
            // Special case for Contract.Truth:
            if (this.Operator == "&&" &&
                left.Silvernode != null && left.Silvernode.ToString().Trim() == "true")
            {
                // This happens in contracts only, prepending is unnecessary
                return TranslationResult.FromSilvernode(right.Silvernode, errors);
            }
       


            return TranslationResult
                .FromSilvernode(
                    new BinaryExpressionSilvernode(left.Silvernode, operatorName, right.Silvernode, this.OriginalNode),
                    errors)
                .AndPrepend(left.PrependTheseSilvernodes.Concat(right.PrependTheseSilvernodes));
            ;
        }
    }
}