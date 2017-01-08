using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
// ReSharper disable All

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.translation
{
    class Assignments
    {
        private int a;
        private CellAss c;
        public Assignments()
        {
            Contract.Ensures(Contract.Acc(this.a) && this.a == 0);
            Contract.Ensures(Contract.Acc(this.c) && Contract.Acc(this.c.Value) && this.c.Value == 0);

            this.a = 0;
            this.c = new CellAss();
        }

        public static void Test(Assignments ass, CellAss cell)
        {
            Contract.Requires(Contract.Acc(ass.a) && Contract.Acc(ass.c) && Contract.Acc(ass.c.Value) &&
                              Contract.Acc(cell.Value));
            var d = new Assignments();
            Contract.Assert(d.a == 0);
            Contract.Assert(d.c.Value == 0);
            ass.a = 1;
            Contract.Assert(ass.a == 1);
            ass.c.Value = 2;
            Contract.Assert(ass.c.Value == 2);
            cell.Value = 3;
            Contract.Assert(cell.Value == 3);
            ass.c = cell;
            Contract.Assert(ass.c == cell);
            Contract.Assert(ass.c.Value == 3);
            d.a = 4;
            Contract.Assert(d.a == 4);
            d.c.Value = 5;
            Contract.Assert(d.c.Value == 5);
            d.c = ass.c;
            Contract.Assert(d.c == ass.c);
            d = ass;
            Contract.Assert(d.a == 1);
            Contract.Assert(d == ass);
        }
    }

    class CellAss
    {
        public int Value;
        public CellAss()
        {
            Contract.Ensures(Contract.Acc(this.Value) && this.Value == 0);
            this.Value = 0;
        }
    }
}
