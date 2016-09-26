using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
using Sharpsilver.Translation.Trees.CSharp.Expressions;
using Sharpsilver.Translation.Trees.Silver;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.CSharp
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
            if (exResult.Silvernode.IsVerificationCondition())
            {
                return exResult;
            }
            else if (exResult.Silvernode is CallSilvernode)
            {
                // It is a method call -- and the result is ignored.
                CallSilvernode call = (exResult.Silvernode) as CallSilvernode;
                if (call.Type != SilverType.Void)
                {
                    var tempVar = context.Process.IdentifierTranslator.RegisterNewUniqueIdentifier();
                    var sequence = new SequenceSilvernode(OriginalNode,
                        new VarStatementSilvernode(tempVar, call.Type, null),
                        new AssignmentSilvernode(
                            new IdentifierSilvernode(tempVar),
                            call, null)
                        );
                    return TranslationResult.FromSilvernode(sequence, exResult.Errors);
                }
                else
                {
                    return TranslationResult.FromSilvernode(call, exResult.Errors);
                }
            }
            else if (exResult.Silvernode is BinaryExpressionSilvernode)
            {
                return exResult;
            }
            else if (exResult.Silvernode is AssignmentSilvernode)
            {
                return exResult;
            }
            else if (exResult.Silvernode is StatementSilvernode)
            {
                return exResult;
            }

            return TranslationResult.Error(Expression.OriginalNode, Diagnostics.SSIL107_ThisExpressionCannotBeStatement);
        }
    }
}
