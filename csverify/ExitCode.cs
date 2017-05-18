using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Frontend
{
    /// <summary>
    /// Represents the return code of a csverify.exe execution
    /// </summary>
    enum ExitCode
    {
        /// <summary>
        /// Represents success of a csverify.exe run (return code of 0).
        /// </summary>
        Success = 0,
        /// <summary>
        /// Represents failure of a csverify.exe run due to a translation or verification error (return code of 1).
        /// </summary>
        VerificationError = 1,
        /// <summary>
        /// Represents failure of a csverify.exe run due to invalid arguments or other exceptions (return code of 2).
        /// </summary>
        InputError = 2

    }
}
