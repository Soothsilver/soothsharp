using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// Contains static functions that translate into Silver language constructs, such as preconditions or postconditions.
    /// </summary>
    public static class Contract
    {
        /// <summary>
        /// Within contracts, represents the return value of a function or a method, if it is of the type System.Int32.
        /// </summary>
        public static int IntegerResult { get; } = 0;

        /// <summary>
        /// Adds a proof obligation: The verifier must ensure that the specified postcondition is true when this method returns.
        /// </summary>
        /// <param name="postcondition">The condition that will be verified to be true at each exit point.</param>
        public static void Ensures(bool postcondition)
        {
        }
    }
}
