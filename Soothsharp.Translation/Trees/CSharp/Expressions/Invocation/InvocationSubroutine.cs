using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Translation.Trees.CSharp.Invocation
{
    /// <summary>
    /// Base class for translation kinds that result in a silvernode that has a type and that might or might not be impure
    /// (i.e. require prepending).
    /// </summary>
    /// <seealso cref="Soothsharp.Translation.Trees.CSharp.Invocation.InvocationTranslation" />
    abstract class InvocationSubroutine : InvocationTranslation
    {
        protected SilverType SilverType;
        public override void PostprocessPurity(TranslationResult result, TranslationContext context)
        {
            if (this.Impure)
            {
               result.AsImpureAssertion(context, this.SilverType, "method call");
            }
        }
    }
}
