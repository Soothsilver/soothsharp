using Microsoft.CodeAnalysis;
using System;

namespace Sharpsilver.Translation
{
    /// <summary>
    /// A context modifies how should C# code be translated into Silver. A context determines, for example,
    /// whether we requires pure expressions without side-effects.
    /// </summary>
    public class TranslationContext
    {
        /// <summary>
        /// Gets the instance of this transcompilation process.
        /// </summary>
        public TranslationProcess Process { get; }
        /// <summary>
        /// Indicates whether only pure expressions are allowed in this context.
        /// </summary>
        public PurityContext PurityContext { get; private set; } = PurityContext.PurityNotRequired;
        /// <summary>
        /// Indicates whether classes and method with no [Verified] or [Unverified] attribute should be verified.
        /// </summary>
        public bool VerifyUnmarkedItems { get; private set; } = false;
        /// <summary>
        /// Gets the semantic model of the C# compilation.
        /// </summary>
        public SemanticModel Semantics { get; private set; }

        /// <summary>
        /// Creates a new translation context as a copy of a previous one.
        /// </summary>
        /// <param name="copyFrom">The context that should be copied.</param>
        public TranslationContext(TranslationContext copyFrom)
        {
            this.PurityContext = copyFrom.PurityContext;
            this.Process = copyFrom.Process;
            this.Semantics = copyFrom.Semantics;
            this.VerifyUnmarkedItems = copyFrom.VerifyUnmarkedItems;
        }

        private TranslationContext(TranslationProcess process, SemanticModel semantics)
        {
            Process = process;
            Semantics = semantics;
        }
        public static TranslationContext StartNew(TranslationProcess translationProcess, SemanticModel semantics, bool verifyUnmarkedItems)
        {
            return new TranslationContext(translationProcess, semantics)
            {
                VerifyUnmarkedItems = verifyUnmarkedItems
            };
        }

        /// <summary>
        /// Creates a context copy that is pure in addition to the properties of the copied context.
        /// </summary>
        /// <returns>A copy of the context that is marked pure.</returns>
        public TranslationContext ChangePurityContext(PurityContext newPurityContext)
        {
            return new TranslationContext(this)
            {
                PurityContext = newPurityContext
            };
        }
    }
    /// <summary>
    /// Some sharpnodes can only be translated in impure context. The <see cref="PurityContext"/> determines what should be done
    /// if they're encountered elsewhere. 
    /// </summary>
    public enum PurityContext
    {
        /// <summary>
        /// Sharpnodes that result in impure silvernodes may translate normally.
        /// </summary>
        PurityNotRequired,
        /// <summary>
        /// Sharpnodes must result in pure silvernodes, but prepatory impure silvernodes may be stored with the <see cref="TranslationResult"/>'s prepend nodes.
        /// </summary>
        Purifiable,
        /// <summary>
        /// Sharpnodes must result in pure silvernodes. If they can't do that, an error should be triggered.
        /// </summary>
        PureOrFail
    }
}