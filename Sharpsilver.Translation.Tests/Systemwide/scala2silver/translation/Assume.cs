using Sharpsilver.Contracts;
using static Sharpsilver.Contracts.Contract;
using static Sharpsilver.Contracts.Permission;

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.translation
{
    class AssumeClass
    {
        [Pure]
        public int f()
        {
            Requires(Acc(c, Wildcard));
            return c;
        }

        int a; int b; int c;

        public AssumeClass(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        [Predicate]
        public bool P()
        {
            return Acc(c, Wildcard) && f() > 0;
        }

        [Predicate]
        public bool Q()
        {
            return Acc(b, Wildcard) && b > 0;
        }

        public void Succeeds()
        {
            Inhale(Acc(a, Wildcard) && Acc(c, Wildcard) && Acc(c, Permission.Half));
            Assume(a > 0);
            Assert(a > 0);
            Assume(f() > 0);
            Fold(Acc(P(), Write));
            Assume(Acc(Q(), Write));
            Unfold(Acc(Q(), Write));
            Assert(b > 0);

        }
        public void Fails1()
        {
            Inhale(Acc(a, Wildcard));
            Assert(a > 0);
            Assume(a > 0);

        }
        public void Fails2()
        {
            Inhale(Acc(a, Wildcard));
            Assume(a > 0);
            Assert(a == 0);

        }
        public void Fails3()
        {
            Inhale(Acc(a, Wildcard) && Acc(b, Wildcard));
            Assume(b == 0);
            Assert(a == 0);
        }
        public void Fails4()
        {
            Inhale(Acc(a, Wildcard) && Acc(b, Wildcard));
            Inhale(Acc(Q(), Write));
            Assert(b > 0);
        }
    }
}
