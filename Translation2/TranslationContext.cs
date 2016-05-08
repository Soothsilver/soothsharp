using System;

namespace Sharpsilver.Translation
{
    public class TranslationContext
    {
        public TranslationProcess Process { get; }
        public bool IsPure { get; set; }
        public bool UnderVerifiedAttribute { get; set; }



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