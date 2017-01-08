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
            Contract.Requires(Contract.Acc(this.c, Permission.Wildcard));
            return this.c;
        }

        private int a;
        private int b;
        private int c;

        [Predicate]
        public bool P()
        {
            return Contract.Acc(this.c, Permission.Wildcard) && F() > 0;
        }

        [Predicate]
        public bool Q()
        {
            return Contract.Acc(this.b, Permission.Wildcard) && this.b > 0;
        }

        public void Succeeds()
        {
            Contract.Inhale(Contract.Acc(this.a, Permission.Wildcard) && Contract.Acc(this.c, Permission.Wildcard) && Contract.Acc(this.c, Permission.Half));
            Contract.Assume(this.a > 0);
            Contract.Assert(this.a > 0);
            Contract.Assume(F() > 0);
            Contract.Fold(Contract.Acc(P(), Permission.Write));
            Contract.Assume(Contract.Acc(Q(), Permission.Write));
            Contract.Unfold(Contract.Acc(Q(), Permission.Write));
            Contract.Assert(this.b > 0);

        }
        public void Fails1()
        {
            Contract.Inhale(Contract.Acc(this.a, Permission.Wildcard));
            // expect SSIL204 at next
            Contract.Assert(this.a > 0);
            Contract.Assume(this.a > 0);

        }
        public void Fails2()
        {
            Contract.Inhale(Contract.Acc(this.a, Permission.Wildcard));
            Contract.Assume(this.a > 0);
            // expect SSIL204 at next
            Contract.Assert(this.a == 0);

        }
        public void Fails3()
        {
            Contract.Inhale(Contract.Acc(this.a, Permission.Wildcard) && Contract.Acc(this.b, Permission.Wildcard));
            Contract.Assume(this.b == 0);
            // expect SSIL204 at next
            Contract.Assert(this.a == 0);
        }
        public void Fails4()
        {
            Contract.Inhale(Contract.Acc(this.a, Permission.Wildcard) && Contract.Acc(this.b, Permission.Wildcard));
            Contract.Inhale(Contract.Acc(Q(), Permission.Write));
            // expect SSIL204 at next
            Contract.Assert(this.b > 0);
        }
    }
}
