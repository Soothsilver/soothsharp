using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts.Verified
{
    /// <summary>
    /// Contains mathematical pure functions that are formally verified.
    /// </summary>
    [Verified]
    public static class VerifiedMath
    {
        static int Maximum(int a, int b)
        {
            Contract.Ensures(a >= b ? a == Contract.IntegerResult : b == Contract.IntegerResult);

            return (a >= b ? a : b);
        }
        static int Minimum(int a, int b)
        {
            Contract.Ensures(a <= b ? a == Contract.IntegerResult : b == Contract.IntegerResult);

            return (a <= b ? a : b);
        }
        static int Abs(int value)
        {
            Contract.Ensures(value < 0 ? -value == Contract.IntegerResult : value == Contract.IntegerResult);

            return (value < 0 ? -value : value);
        }
    }
}
