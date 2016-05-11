using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation.Translators;
using Sharpsilver.Translation.AbstractSyntaxTrees.Silver;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    class ExpressionStatementSharpnode : StatementSharpnode
    {
        public ExpressionSharpnode Expression;

        public ExpressionStatementSharpnode(ExpressionStatementSyntax originalNode) : base(originalNode)
        {
            this.Expression = RoslynToSharpnode.MapExpression(originalNode.Expression);
        }
        

        public override TranslationResult Translate(TranslationContext context)
        {
            var exResult = Expression.Translate(context);
            if (exResult.SilverSourceTree.IsVerificationCondition())
            {
                return exResult;
            }
            return TranslationResult.Silvernode(
                new ExpressionStatementSilvernode(exResult.SilverSourceTree, OriginalNode)
                , exResult.ReportedDiagnostics);
        }
    }
}
