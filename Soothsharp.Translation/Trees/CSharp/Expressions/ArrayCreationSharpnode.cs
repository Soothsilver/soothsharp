using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Soothsharp.Translation.Translators;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.Trees.CSharp.Expressions
{
    class ArrayCreationSharpnode : ExpressionSharpnode
    {
        private readonly List<ExpressionSharpnode> Arguments = new List<ExpressionSharpnode>();
        private Error error;

        private void LoadFrom(InitializerExpressionSyntax syntax)
        {
            if (syntax == null)
            {
                error = new Error(Diagnostics.SSIL108_FeatureNotSupported, this.OriginalNode,
                    "rank-initialized arrays");
                return;
            }
            foreach (var s in syntax.Expressions)
            {
                Arguments.Add(RoslynToSharpnode.MapExpression(s));
            }
        }

        public ArrayCreationSharpnode(InitializerExpressionSyntax syntax) : base(syntax)
        {
            this.LoadFrom(syntax);
        }

        public ArrayCreationSharpnode(ArrayCreationExpressionSyntax syntax) : base(syntax)
        {
            this.LoadFrom(syntax.Initializer);
        }

        public ArrayCreationSharpnode(ImplicitArrayCreationExpressionSyntax syntax) : base(syntax)
        {
            this.LoadFrom(syntax.Initializer);
        }

        public override TranslationResult Translate(TranslationContext context)
        {
            // see thesis for details

            if (error != null)
            {
                return TranslationResult.Error(error);
            }
            List<Error> errors = new List<Error>();
    
            var temporaryHoldingVariable = context.Process.IdentifierTranslator.RegisterNewUniqueIdentifier();

            // ReSharper disable once UseObjectOrCollectionInitializer
            var arguments = new List<Silvernode>();
            var prepend = new List<StatementSilvernode>();
            arguments.Add("Seq(");
            foreach (var arg in this.Arguments)
            {
                var res = arg.Translate(context.ChangePurityContext(PurityContext.Purifiable));
                arguments.Add(res.Silvernode);
                if (arg != this.Arguments.Last())
                {
                    arguments.Add(", ");
                }
                errors.AddRange(res.Errors);
                prepend.AddRange(res.PrependTheseSilvernodes);
            }
            arguments.Add(")");

            Silvernode arrayConstruction = new SimpleSequenceSilvernode(null, arguments.ToArray());
            prepend.AddRange(new StatementSilvernode[] {
                new VarStatementSilvernode(temporaryHoldingVariable, SilverType.Ref, this.OriginalNode),
                new SimpleSequenceStatementSilvernode(this.OriginalNode,
                    new IdentifierSilvernode(temporaryHoldingVariable), " := ", "new(",
                    ArraysTranslator.IntegerArrayContents, ")"),
                new AssignmentSilvernode(
                    new SimpleSequenceSilvernode(this.OriginalNode, new IdentifierSilvernode(temporaryHoldingVariable),
                        ".", ArraysTranslator.IntegerArrayContents), arrayConstruction, this.OriginalNode)
            });
            switch (context.PurityContext)
            {
                case PurityContext.PurityNotRequired:
                case PurityContext.Purifiable:
                    return TranslationResult.FromSilvernode(new IdentifierSilvernode(temporaryHoldingVariable), errors).AndPrepend(prepend);

                case PurityContext.PureOrFail:
                    return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL114_NotPureContext, "Array creation is inherently impure.");
            }
            throw new Exception("This should never be reached.");
        }
    }
}
