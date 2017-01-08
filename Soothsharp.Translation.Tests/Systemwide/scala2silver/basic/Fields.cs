using Soothsharp.Contracts;

namespace Soothsharp.Translation.Tests.Systemwide.scala2silver.basic
{
    class Fields
    {
        private int variable;
        private C value;

        public Fields()
        {
            this.variable = 3;

            Contract.Assert(this.variable < 4);
            Contract.Assert(this.variable > 2);

            this.value = new C(2);

            Contract.Assert(this.value.a == 2);
        }
    }

    class C
    {
        public int a;

        
        public C(int a)
        {
            Contract.Ensures(Contract.Acc(this.a) && this.a == a);

            this.a = a;

        }
        
    }
}
