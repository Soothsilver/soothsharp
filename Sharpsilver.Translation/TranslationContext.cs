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
        public bool IsPure { get; private set; } = false;
        /// <summary>
        /// Indicates whether the containing type or method is marked [Verified].
        /// </summary>
        public bool UnderVerifiedAttribute { get; private set; } = false;
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
            this.UnderVerifiedAttribute = copyFrom.UnderVerifiedAttribute;
            this.IsPure = copyFrom.IsPure;
            this.Process = copyFrom.Process;
            this.UnderVerifiedAttribute = copyFrom.UnderVerifiedAttribute;
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
        public TranslationContext ForcePure()
        {
            return new TranslationContext(this)
            {
                IsPure = true
            };
        }
    }
}