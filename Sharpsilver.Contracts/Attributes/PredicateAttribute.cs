using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// The annotated method should be translated as a Silver predicate.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PredicateAttribute : Attribute
    {
    }
}
