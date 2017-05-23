using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Rewriter
{
    [Verified]
    class ExampleFile
    {
        private bool autoFalse;

        [Predicate]
        public static bool IsTwo(int number)
        {
            Contract.Ensures(IntegerResult == 2);
            return number == 2;
        }

        public void Method()
        {
            Contract.Requires(Acc(autoFalse) && autoFalse == false);
            Contract.Requires(IsTwo(3));
            Contract.Ensures(false);


            Fold(IsTwo(3));

            int a = Unfolding(IsTwo(5), 4 + 8);
        }
    }
}
