
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// The annotated method should be translated as abstractin Silver. The body of this method
    /// will be ignored - just put anything in there that will satisfy the typing system.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AbstractAttribute : Attribute
    {
    }
}
