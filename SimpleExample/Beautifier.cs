using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;

namespace Sharpsilver.Examples.SimpleExample
{
    class Beautifier
    {
        [Verified]
        public static int Beautify(int uglyNegativeNumber)
        {
            Contract.Requires(uglyNegativeNumber < 0);
            Contract.Ensures(Contract.IntegerResult > 0);

            if (uglyNegativeNumber > -10) return 42;
            return -uglyNegativeNumber;
        }

        [Verified]
        public static int Abs(int value)
        {
            Contract.Ensures(Contract.IntegerResult > 0);
            Contract.Ensures(value >= 0 ? Contract.IntegerResult == value : true);
            Contract.Ensures(value < 0 ? Contract.IntegerResult == -value : true);

            if (value < 0) return -value; else return value;
        }
    }
}
