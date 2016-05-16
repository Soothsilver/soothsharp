using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// Represents a fractional permission that defines the level of access to an address on the heap. Immutable.
    /// </summary>
    public class Permission
    {
        private int numerator;
        private int denominator;

        /// <summary>
        /// Creates a fractional permission. The arguments must be integer literals.
        /// TODO check that they are literals
        /// </summary>
        /// <param name="numerator">The numerator of the fraction.</param>
        /// <param name="denominator">The denominator of the fraction.</param>
        public Permission(int numerator, int denominator)
        {
            this.numerator = numerator;
            this.denominator = denominator;
        }
        /// <summary>
        /// Gets the full write permission (100%).
        /// </summary>
        public static Permission Write
        {
            get
            {
                return new Permission(1, 1);
            }
        }
    }
}
