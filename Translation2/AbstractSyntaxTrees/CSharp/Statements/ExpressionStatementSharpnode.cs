using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Sharpsilver.Translation;
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
            if (exResult.Silvernode.IsVerificationCondition())
            {
                return exResult;
            }
            if (exResult.Silvernode is CallSilvernode)
            {
                // It is a method call -- and the result is ignored.
                CallSilvernode call = (exResult.Silvernode) as CallSilvernode;
                if (call.Type != SilverType.Void)
                {
                    var tempVar = context.Process.IdentifierTranslator.RegisterNewUniqueIdentifier();
                    var sequence = new SequenceSilvernode(OriginalNode,
                        new LocalVariableDeclarationSilvernode(tempVar, call.Type, null),
                        new AssignmentSilvernode(new TextSilvernode(tempVar.ToString(), null),
                            call, null)
                        );
                    return TranslationResult.FromSilvernode(sequence, exResult.Errors);
                }
                else
                {
                    return TranslationResult.FromSilvernode(call, exResult.Errors);
                }

            }
            return TranslationResult.Error(Expression.OriginalNode, Diagnostics.SSIL107_ThisExpressionCannotBeStatement);
        }
    }
}
