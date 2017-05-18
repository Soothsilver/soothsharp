using Soothsharp.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Translation.Tests
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
