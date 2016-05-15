using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// The annnotated method is pure and should be translated as a Silver pure function.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    class PureAttribute : Attribute
    {

    }
}
