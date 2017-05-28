using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.prepend
{
    class SeqAndArray
    {
        void a()
        {
            Seq<int> s = new Seq<int>(1, mbox(), mbox() + 4);

            Contract.Assert(s[0] == 1);
            Contract.Assert(s[1] == 2);
            Contract.Assert(s[2] == 6);
        }
        void b()
        {
            int[] s = {1, mbox(), mbox() + 4};

            Contract.Assert(s[0] == 1);
            Contract.Assert(s[1] == 2);
            Contract.Assert(s[2] == 6);
        }
        void c()
        {

            Seq<int> s = new Seq<int>(10, 20, 30, 40, 50, 60);
            int[] a = {10, 20, 30, 40, 50, 60};

            Contract.Assert(s[mbox()] == 30);
            Contract.Assert(a[mbox()] == 30);
        }
        int mbox()
        {
            Contract.Ensures(Contract.IntegerResult == 2);
            return 4 / 2;
        }
    }
}
