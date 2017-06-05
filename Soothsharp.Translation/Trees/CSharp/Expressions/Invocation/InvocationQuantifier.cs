using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    /// <summary>
    /// Translates <see cref="Contracts.Contract.ForAll(Func{int, bool})"/>
    /// and <see cref="Contracts.Contract.Exists(Func{int, bool})"/>.  
    /// </summary>
    /// <seealso cref="Soothsharp.Translation.Trees.CSharp.Invocation.InvocationTranslation" />
    class InvocationQuantifier : InvocationTranslation
    {
        private QuantifierKind kind;

        public InvocationQuantifier(QuantifierKind kind)
        {
            this.kind = kind;
        }

        public override void Run(List<ExpressionSharpnode> arguments, SyntaxNode originalNode, TranslationContext context)
        {
            
            // Error checking
            int numArguments = arguments.Count;
            if (numArguments != 1)
            {
                this.Errors.Add(new Error(Diagnostics.SSIL301_InternalLocalizedError,
                    originalNode,
                    "incorrect number of arguments for a quantifier expression"));
                this.Silvernode = new ErrorSilvernode(originalNode);
                return;
            }
            if (!(arguments[0] is LambdaSharpnode))
            {
                this.Errors.Add(new Error(Diagnostics.SSIL124_QuantifierMustGetLambda,
                      originalNode));
                this.Silvernode = new ErrorSilvernode(originalNode);
                return;

            }

            // Use the lambda expression to create the Viper code
            LambdaSharpnode lambda = (LambdaSharpnode)arguments[0];
            if (lambda.PrepareForInsertionIntoQuantifier(context))
            {
                this.Silvernode = new SimpleSequenceSilvernode(originalNode, this.kind == QuantifierKind.ForAll ? "forall " : "exists ",
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
                this.Silvernode = res.Silvernode;
                this.Errors.AddRange(res.Errors);
            }
        }
    }
}
