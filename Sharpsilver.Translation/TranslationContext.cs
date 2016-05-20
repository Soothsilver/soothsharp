using Microsoft.CodeAnalysis;
using System;

namespace Sharpsilver.Translation
{
    public class TranslationContext
    {
        public TranslationProcess Process { get; }
        public bool IsPure { get; private set; } = false;
        public bool UnderVerifiedAttribute { get; private set; } = false;

        public SemanticModel Semantics
        {
            get
            {
                return Process.SemanticModel;
            }
        }

        /// <summary>
        /// Creates a new translation context as a copy of a previous one.
        /// </summary>
        /// <param name="copyFrom">The context that should be copied.</param>
        public TranslationContext(TranslationContext copyFrom)
        {
            this.UnderVerifiedAttribute = copyFrom.UnderVerifiedAttribute;
            this.IsPure = copyFrom.IsPure;
            this.Process = copyFrom.Process;
        }

        private TranslationContext(TranslationProcess process)
        {
            Process = process;
        }
        internal static TranslationContext StartNew(TranslationProcess translationProcess)
        {
            return new TranslationContext(translationProcess);
        }

        internal TranslationContext ForcePure()
        {
            return new TranslationContext(this)
            {
                IsPure = true
            };
        }
    }
}