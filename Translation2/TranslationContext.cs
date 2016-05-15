using Microsoft.CodeAnalysis;
using System;

namespace Sharpsilver.Translation
{
    public class TranslationContext
    {
        public TranslationProcess Process { get; }
        public bool IsPure { get; set; }
        public bool UnderVerifiedAttribute { get; set; }

        public SemanticModel Semantics
        {
            get
            {
                return Process.SemanticModel;
            }
        }


        private TranslationContext(TranslationProcess process)
        {
            Process = process;
        }
        internal static TranslationContext StartNew(TranslationProcess translationProcess)
        {
            return new TranslationContext(translationProcess);
        }
    }
}