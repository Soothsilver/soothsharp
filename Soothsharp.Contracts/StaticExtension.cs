using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// Adds "Implies" and "EquivalentTo" as member methods to System.Boolean.
    /// </summary>
    public static class StaticExtension
    {
        /// <summary>
        /// Returns true if the the receiver logically implies the result, 
        /// i.e. either the receiver is false or the result is true. 
        /// Works in C# and is translated as the implication operator in Silver.
        /// </summary>
        /// <param name="condition">The receiver, the condition operand of the implication.</param>
        /// <param name="result">The right side of the implication.</param>
        /// <returns>True, if the the receiver logically implies the result, i.e. either the receiver is false or the result is true.</returns>
        public static bool Implies(this bool condition, bool result)
        {
            return !condition || result;
        }
        /// <summary>
        /// Returns true if the receiver is logically equivalent to the argument. This represents a mathematical equivalence.
        /// </summary>
        /// <param name="leftSide">The receiver.</param>
        /// <param name="rightSide">The argument.</param>
        /// <returns>True, if the two booleans are the same; false otherwise.</returns>
        public static bool EquivalentTo(this bool leftSide, bool rightSide)
        {
            return leftSide == rightSide;
        }
    }
}
