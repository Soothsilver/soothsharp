using Sharpsilver.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ae7
{
    class Program
    {
        static int f()
        {
            Contract.Ensures(Contract.IntegerResult == 3);
            return 4 - 1;
        }


        int field;


        static void ImprovedExhale(bool whatShouldBeAsserted)
        {
            Contract.Requires(whatShouldBeAsserted);
        }


        static void a()
        {
            Contract.Requires(Contract.Acc(field));

            Contract.Assume(field == 3);

            ImprovedExhale(f() > 2);

            if (field != 3)
            {
                Contract.Assert(false);
            }
        }
    }
}
