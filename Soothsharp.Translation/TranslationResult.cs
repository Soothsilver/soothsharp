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
        // Universal:

        /// <summary>
        /// Gets or sets the silvernode that represents the result of the translation.
        /// </summary>
        public Silvernode Silvernode { get; set; }
        
        /// <summary>
        /// Gets a value indicating whether no errors occurred during the translation.
        /// </summary>
        public bool WasTranslationSuccessful
        {
            get
            {
                return this.Errors.All(err => err.Diagnostic.Severity != DiagnosticSeverity.Error);
            }
        }

        /// <summary>
        /// Gets a list of errors that triggered during the translation.
        /// </summary>
        public List<Error> Errors { get; } = new List<Error>();

        /// <summary>
        /// If this <see cref="TranslationResult"/> is a result of a sharpnode that's being purified, then this will contain the silvernodes
        /// that must be prepended to this silvernode, at the closest location that allows impure silvernodes. If not, this will be an empty list.
        /// </summary>
        public List<StatementSilvernode> PrependTheseSilvernodes = new List<StatementSilvernode>();

        // For methods and loops.        
        /// <summary>
        /// If this is the result of the translation of a block, then this contains the translations of the contract methods of the block (Requires etc.)
        /// </summary>
        public List<ContractSilvernode> Contracts = new List<ContractSilvernode>();
        /// <summary>
        /// If this is the result of the translation of an array access, then this contains the translation of the expression that results in the array.
        /// </summary>
        public TranslationResult ArraysContainer;
        /// <summary>
        /// If this is the result of the translation of an array access, then this contains the translation of the expression that results in the index to the array.
        /// </summary>
        public TranslationResult ArraysIndex;

        /// <summary>
        /// Creates a new <see cref="TranslationResult"/> off a failed translation. 
        /// </summary>
        /// <param name="node">The node that triggered an error.</param>
        /// <param name="diagnostic">The type of the error.</param>
        /// <param name="diagnosticArguments">The diagnostic arguments.</param>
        public static TranslationResult Error(SyntaxNode node, SoothsharpDiagnostic diagnostic, params Object[] diagnosticArguments)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            TranslationResult r = new TranslationResult();
            r.Silvernode = new ErrorSilvernode(node);
            r.Errors.Add(new Error(diagnostic, node,diagnosticArguments));
            return r;
        }

        /// <summary>
        /// Creates a new <see cref="TranslationResult"/> off a failed translation. 
        /// </summary>
        /// <param name="error">The error that triggered.</param>
        public static TranslationResult Error(Error error)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            TranslationResult r = new TranslationResult();
            r.Silvernode = new ErrorSilvernode(error.Node);
            r.Errors.Add(error);
            return r;
        }

        /// <summary>
        /// Modifies the result by adding silvernodes to the Silver code that should be prepended before this.
        /// </summary>
        /// <param name="silvernodes">The silvernodes to prepend.</param>
        public TranslationResult AndPrepend(params StatementSilvernode[] silvernodes)
        {
            this.PrependTheseSilvernodes = new List<StatementSilvernode>(silvernodes);
            return this;
        }

        /// <summary>
        /// Modifies the result by adding silvernodes to the Silver code that should be prepended before this.
        /// </summary>
        /// <param name="silvernodes">The silvernodes to prepend.</param>
        public TranslationResult AndPrepend(IEnumerable<StatementSilvernode> silvernodes)
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

        /// <summary>
        /// This is called when an invocation sharpnode determines that this silvernode must be a statement in Viper code; this method
        /// then, if in <see cref="PurityContext.PureOrFail"/> context, changes this result into an error; or, if in <see cref="PurityContext.Purifiable"/>
        /// context, moves this result's <see cref="Silvernode"/> to <see cref="PrependTheseSilvernodes"/> and performs the prepending.    
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="type">The Silver type of this silvernode.</param>
        /// <param name="impurityReason">The reason why this silvernode must be a statement.</param>
        internal void AsImpureAssertion(TranslationContext context, SilverType type, string impurityReason)
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
        }
    }
}
