
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// The annotated method should be translated as a Viper predicate. Use <see cref="AbstractAttribute"/> and <see cref="PredicateAttribute"/> at the same time to create an abstract predicate.   
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class PredicateAttribute : Attribute
    {
    }
}
