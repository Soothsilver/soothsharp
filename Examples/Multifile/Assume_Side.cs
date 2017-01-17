using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Csverify.Multifile
{
    class Assume_Side
    {
        public bool ReturnsTrueButIsIncorrect()
        {
            Contract.Ensures(Result<bool>());

            // this fails:
            Contract.Assert(false);

            return true;
        }
    }
}
