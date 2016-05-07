using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// The annotated class or method makes use of formal verification and should be translated into Silver.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class VerifiedAttribute : Attribute
    {
    }
}
