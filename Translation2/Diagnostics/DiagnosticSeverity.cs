using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    public enum DiagnosticSeverity
    {
        /// <summary>
        /// The translation should work as normal, except with some minor issues.
        /// </summary>
        Warning,
        /// <summary>
        /// The translation won't work but we can run the rest of it anyway.
        /// </summary>
        Error
    }
}
