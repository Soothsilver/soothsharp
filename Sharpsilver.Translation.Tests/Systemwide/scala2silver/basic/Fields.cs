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

            value = new C(2);
            object d = new D();
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
    class D
    {

    }
}
