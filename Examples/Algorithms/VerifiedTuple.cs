using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples.Algorithms
{
    /// <summary>
    /// Represents two related integers.
    /// </summary>
    public class VerifiedTuple
    {
        public int First;
        public int Second;

        public VerifiedTuple(int first, int second)
        {
            Ensures(Acc(First) && Acc(Second));

            First = first;
            Second = second;
        }

        /// <summary>
        /// Swaps the first and second integer of this tuple.
        /// </summary>
        public void Swap()
        {
            Requires(Acc(First) && Acc(Second));
            Ensures(Acc(First) && Acc(Second));
            Ensures(First == Old(Second) && Second == Old(First));

            int temp = First;
            First = Second;
            Second = temp;
        }
    }
}
