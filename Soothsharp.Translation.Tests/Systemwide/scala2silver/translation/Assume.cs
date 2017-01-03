using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
using static Soothsharp.Contracts.Permission;
#pragma warning disable 649

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.translation
{
    class AssumeClass
    {
        [Pure]
        public int F()
        {
            Requires(Acc(c, Wildcard));
            return c;
        }

        private int a;
        private int b;
        private int c;

        [Predicate]
        public bool P()
        {
            return Acc(c, Wildcard) && F() > 0;
        }

        [Predicate]
        public bool Q()
        {
            return Acc(b, Wildcard) && b > 0;
        }

        public void Succeeds()
        {
            Inhale(Acc(a, Wildcard) && Acc(c, Wildcard) && Acc(c, Half));
            Assume(a > 0);
            Assert(a > 0);
            Assume(F() > 0);
            Fold(Acc(P(), Write));
            Assume(Acc(Q(), Write));
            Unfold(Acc(Q(), Write));
            Assert(b > 0);

        }
        public void Fails1()
        {
            Inhale(Acc(a, Wildcard));
            // expect SSIL204 at next
            Assert(a > 0);
            Assume(a > 0);

        }
        public void Fails2()
        {
            Inhale(Acc(a, Wildcard));
            Assume(a > 0);
            // expect SSIL204 at next
            Assert(a == 0);

        }
        public void Fails3()
        {
            Inhale(Acc(a, Wildcard) && Acc(b, Wildcard));
            Assume(b == 0);
            // expect SSIL204 at next
            Assert(a == 0);
        }
        public void Fails4()
        {
            Inhale(Acc(a, Wildcard) && Acc(b, Wildcard));
            Inhale(Acc(Q(), Write));
            // expect SSIL204 at next
            Assert(b > 0);
        }
    }
}
