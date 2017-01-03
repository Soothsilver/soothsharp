using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.translation
{
    class Assignments
    {
        private int a;
        private CellAss c;
        public Assignments()
        {
            Ensures(Acc(a) && a == 0);
            Ensures(Acc(c) && Acc(c.Value) && c.Value == 0);

            a = 0;
            c = new CellAss();
        }

        public static void Test(Assignments ass, CellAss cell)
        {
            Requires(Acc(ass.a) && Acc(ass.c) && Acc(ass.c.Value) &&
                              Acc(cell.Value));
            var d = new Assignments();
            Assert(d.a == 0);
            Assert(d.c.Value == 0);
            ass.a = 1;
            Assert(ass.a == 1);
            ass.c.Value = 2;
            Assert(ass.c.Value == 2);
            cell.Value = 3;
            Assert(cell.Value == 3);
            ass.c = cell;
            Assert(ass.c == cell);
            Assert(ass.c.Value == 3);
            d.a = 4;
            Assert(d.a == 4);
            d.c.Value = 5;
            Assert(d.c.Value == 5);
            d.c = ass.c;
            Assert(d.c == ass.c);
            d = ass;
            Assert(d.a == 1);
            Assert(d == ass);
        }
    }

    class CellAss
    {
        public int Value;
        public CellAss()
        {
            Ensures(Acc(Value) && Value == 0);
            Value = 0;
        }
    }
}
