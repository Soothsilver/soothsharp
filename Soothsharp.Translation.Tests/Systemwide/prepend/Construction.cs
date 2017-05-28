using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.prepend
{
    class Construction
    {
        private int field;
        Construction(int number)
        {
            Contract.Ensures(Contract.Acc(field));
            Contract.Ensures(this.field == number);

            field = number;
        }
        void MoveConstructor()
        {
            int a = 2;
            Construction c = (2 == a) ? new prepend.Construction(3) : null;

            Contract.Assert(c != null);
            // expect SSIL204 at next
            Contract.Assert(c.field == 4);
        }
        void MoveBeforeConstructor()
        {
            Construction c = new Construction(m());

            Contract.Assert(c.field == 2);
        }
        int m()
        {
            Contract.Ensures(Contract.IntegerResult == 2);
            return 2;
        }
    }
}
