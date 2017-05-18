using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Examples.Multifile
{
    class Assume_Main
    {
        static void Amain()
        {
            Assume_Side s = new Assume_Side();
            bool truth = s.ReturnsTrueButIsIncorrect();

            Contract.Assert(truth);
        }
    }
}
