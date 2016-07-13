using Sharpsilver.Contracts;

namespace Sharpsilver.Translation.Tests.Systemwide.scala2silver.basic
{
    class Fields
    {
        private int variable;
        private C value;

        public Fields()
        {
            Contract.Ensures(Contract.Read(variable) && variable == 3);

            variable = 3;
            value = new C(3);
        }
    }

    class C
    {
        private int a;

        public C(int a)
        {
            this.a = a;
        }
    }
}
