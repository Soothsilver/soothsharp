using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents the result of a main phase translation of a sharpnode.
    /// </summary>
    public class TranslationResult
    {
        // Universal
        public Silvernode Silvernode;
        public bool WasTranslationSuccessful
        {
            get
            {
                return this.Errors.All(err => err.Diagnostic.Severity != DiagnosticSeverity.Error);
            }
        }
        public List<Error> Errors = new List<Error>();
        /// <summary>
        /// If this <see cref="TranslationResult"/> is a result of a sharpnode that's being purified, then this will contain the silvernodes
        /// that must be prepended to this silvernode, at the closest location that allows impure silvernodes. If not, this will be an empty list.
        /// </summary>
        public List<StatementSilvernode> PrependTheseSilvernodes = new List<StatementSilvernode>();

        // For methods and loops.
        public List<VerificationConditionSilvernode> VerificationConditions = new List<VerificationConditionSilvernode>();
        public TranslationResult Arrays_Container;
        public TranslationResult Arrays_Index;

        public static TranslationResult Error(SyntaxNode node, SoothsharpDiagnostic diagnostic, params Object[] diagnosticArguments)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            TranslationResult r = new TranslationResult();
            r.Silvernode = new ErrorSilvernode(node);
            r.Errors.Add(new Error(diagnostic, node,diagnosticArguments));
            return r;
        }
        public static TranslationResult Error(Error error)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            TranslationResult r = new TranslationResult();
            r.Silvernode = new ErrorSilvernode(error.Node);
            r.Errors.Add(error);
            return r;
        }

        public TranslationResult AndPrepend(params StatementSilvernode[] silvernodes)
        {
            this.PrependTheseSilvernodes = new List<StatementSilvernode>(silvernodes);
            return this;
        }

        /// <summary>
        /// Creates a translation result from the specified node and a list of errors.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="errors">The errors. If null, then no errors occurred.</param>
        /// <returns></returns>
        public static TranslationResult FromSilvernode(Silvernode node, IEnumerable<Error> errors = null)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            TranslationResult result = new TranslationResult();
            result.Silvernode = node;
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }

        internal TranslationResult AsImpureAssertion(TranslationContext context, SilverType type, string impurityReason)
        {
           switch(context.PurityContext)
            {
                case PurityContext.PureOrFail:
                    this.Errors.Add(new Error(Diagnostics.SSIL114_NotPureContext, this.Silvernode.OriginalNode, impurityReason));
                    break;
                case PurityContext.Purifiable:
                    var newTempVar = context.Process.IdentifierTranslator.RegisterNewUniqueIdentifier();
                    VarStatementSilvernode v = new VarStatementSilvernode(newTempVar, type, null);
                    AssignmentSilvernode a = new AssignmentSilvernode(new IdentifierSilvernode(newTempVar),
                        this.Silvernode, null);
                    this.PrependTheseSilvernodes.Add(v);
                    this.PrependTheseSilvernodes.Add(a);
                    this.Silvernode = new IdentifierSilvernode(newTempVar);
                    break;
                case PurityContext.PurityNotRequired:
                    break;
            }
            return this;
        }
    }
}
