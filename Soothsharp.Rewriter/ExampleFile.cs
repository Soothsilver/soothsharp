using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
// ReSharper disable All

namespace Soothsharp.Rewriter
{
    [Verified]
    class ExampleFile
    {
        private bool autoFalse;

        public ExampleFile()
        {
            this.autoFalse = true;
        }

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

            this.autoFalse = false;

            Fold(IsTwo(3));

            int a = Unfolding(IsTwo(5), 4 + 8);
        }
    }
}
