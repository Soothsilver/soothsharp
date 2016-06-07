using Sharpsilver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sharpsilTesting
{
    [Verified]
    static class SimplePostcondition
    {
        static int return23()
        {
            Contract.Ensures(Contract.IntegerResult == 23);
            return 2;
        }
    }
}
