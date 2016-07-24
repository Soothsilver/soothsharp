using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.StandaloneVerifier
{
    /// <summary>
    /// Represents the return code of a csverify.exe execution
    /// </summary>
    enum ErrorCode
    {
        /// <summary>
        /// Represents success of a csverify.exe run (return code of 0).
        /// </summary>
        Success = 0,
        /// <summary>
        /// Represents failure of a csverify.exe run (return code of 1).
        /// </summary>
        Error = 1
    }
}
