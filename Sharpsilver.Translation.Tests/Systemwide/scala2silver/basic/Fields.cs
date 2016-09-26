using Sharpsilver.Contracts;

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.basic
{
    class Fields
    {
        private int variable;
        private C value;

        public Fields()
        {
            variable = 3;

            Contract.Assert(variable < 4);
            Contract.Assert(variable > 2);

            value = new C(2);

            Contract.Assert(value.a == 2);
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
