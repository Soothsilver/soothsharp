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
        /// Returns the greater of two numbers.
        /// </summary>
        /// <param name="a">The first number to compare.</param>
        /// <param name="b">The second number to compare.</param>
        /// <returns>The greater number.</returns>
        public static int Max(int a, int b)
        {
            Contract.Ensures(a > b ? Contract.IntegerResult == a : Contract.IntegerResult == b);

            if (a < b)
            {
                return b;
            }
            else
            {
                return a;
            }
        }

        /// <summary>
        /// Returns the lesser of two numbers.
        /// </summary>
        /// <param name="a">The first number to compare.</param>
        /// <param name="b">The second number to compare.</param>
        /// <returns>The lesser number.</returns>
        public static int Min(int a, int b)
        {
            Contract.Ensures(a < b ? Contract.IntegerResult == a : Contract.IntegerResult == b);

            if (a == Max(a, b))
            {
                return b;
            }
            else
            {
                return a;
            }
        }
    }
}
