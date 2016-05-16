using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts.Internals
{
    /// <summary>
    /// This method can only be used within contracts. It will cause a translator error if it is used elsewhere.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    internal class WithinContractsOnlyAttribute : Attribute
    {
    }
}
