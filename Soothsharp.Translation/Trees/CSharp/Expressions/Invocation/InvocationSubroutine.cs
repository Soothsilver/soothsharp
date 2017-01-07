using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    abstract class InvocationSubroutine : InvocationTranslation
    {
        protected SilverType SilverType;
        public override void PostprocessPurity(TranslationResult result, TranslationContext context)
        {
            if (Impure)
            {
               result = result.AsImpureAssertion(context, SilverType, "method call");
            }
        }
    }
}
