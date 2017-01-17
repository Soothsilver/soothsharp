using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Csverify.Multifile
{
    class Assume_Main
    {
        static void Secondary()
        {
            Assume_Side s = new Multifile.Assume_Side();
            bool truth = s.ReturnsTrueButIsIncorrect();

            Contract.Assert(truth);
        }
    }
}
