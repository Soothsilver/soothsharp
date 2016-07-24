using static Sharpsilver.Contracts.Contract;

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.translation
{
    class Assignments
    {
        private int a;
        private CellAss c;
        public Assignments()
        {
            Ensures(Write(a) && a == 0);
            Ensures(Write(c) && Write(c.Value) && c.Value == 0);

            a = 0;
            c = new CellAss();
        }

        public static void Test(Assignments ass, CellAss cell)
        {
            Requires(Write(ass.a) && Write(ass.c) && Write(ass.c.Value) &&
                              Write(cell.Value));
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
            Ensures(Write(Value) && Value == 0);
            Value = 0;
        }
    }
}
