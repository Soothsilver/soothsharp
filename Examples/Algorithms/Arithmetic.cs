using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Examples.Algorithms
{
    public static class Arithmetic
    {
        /// <summary>
        /// [Pure] Returns the greater of two numbers.
        /// </summary>
        /// <param name="a">The first number to compare.</param>
        /// <param name="b">The second number to compare.</param>
        /// <returns>The greater number.</returns>
        [Pure]
        public static int Max(int a, int b)
        {
            Contract.Ensures(a > b ? Contract.IntegerResult == a : Contract.IntegerResult == b);

            return a < b ? b : a;
        }

        /// <summary>
        /// [Pure] Returns the lesser of two numbers.
        /// </summary>
        /// <param name="a">The first number to compare.</param>
        /// <param name="b">The second number to compare.</param>
        /// <returns>The lesser number.</returns>
        [Pure]
        public static int Min(int a, int b)
        {
            Contract.Ensures(a < b ? Contract.IntegerResult == a : Contract.IntegerResult == b);
            return a == Max(a, b) ? b : a;
        }

        /// <summary>
        /// [Pure] Gets the absolute value of an integer.
        /// </summary>
        /// <param name="number">The integer.</param>
        [Pure]
        public static int Abs(int number)
        {
            Contract.Ensures(number > 0 ? Contract.IntegerResult == number : Contract.IntegerResult == -number);
            Contract.Ensures(Contract.IntegerResult >= 0);

            return (number > 0 ? number : -number);
        }
    }
}
