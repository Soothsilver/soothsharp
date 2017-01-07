using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    class InvocationQuantifier : InvocationTranslation
    {
        private QuantifierKind kind;

        public InvocationQuantifier(QuantifierKind kind)
        {
            this.kind = kind;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            
            int numArguments = arguments.Count;
            if (numArguments != 1)
            {
                Errors.Add(new Error(Diagnostics.SSIL301_InternalLocalizedError,
                    originalNode,
                    "incorrect number of arguments for a quantifier expression"));
                Silvernode = new ErrorSilvernode(originalNode);
                return;
            }
            if (!(arguments[0] is LambdaSharpnode))
            {
                Errors.Add(new Error(Diagnostics.SSIL124_QuantifierMustGetLambda,
                      originalNode));
                Silvernode = new ErrorSilvernode(originalNode);
                return;
            }
            LambdaSharpnode lambda = (LambdaSharpnode)arguments[0];
            if (lambda.PrepareForInsertionIntoQuantifier(context))
            {
                Silvernode = new SimpleSequenceSilvernode(originalNode,
                    kind == QuantifierKind.ForAll ? "forall " : "exists ",
                    new IdentifierSilvernode(lambda.VariableIdentifier),
                    " : ",
                    new TypeSilvernode(null, lambda.VariableSilverType),
                    " :: ",
                    lambda.BodySilvernode
                    );
            }
            else
            {
                var res = lambda.GetErrorTranslationResult();
                Silvernode = res.Silvernode;
                Errors.AddRange(res.Errors);
            }
        }
    }
}
