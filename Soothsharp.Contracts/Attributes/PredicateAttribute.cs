
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// The annotated method should be translated as a non-abstract Silver predicate.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PredicateAttribute : Attribute
    {
    }
}
