﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// Adds "Implies" as a member method to System.Boolean.
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
        [Pure]
        public static bool Implies(this bool condition, bool result)
        {
            return !condition || result;
        }
        /// <summary>
        /// Returns true if the receiver is logically equivalent to the argument. This represents a mathematical equivalence.
        /// </summary>
        /// <param name="leftSide">The receiver.</param>
        /// <param name="rightSide">The argument.</param>
        /// <returns></returns>
        [Pure]
        public static bool EquivalentTo(this bool leftSide, bool rightSide)
        {
            return leftSide == rightSide;
        }
    }
}
