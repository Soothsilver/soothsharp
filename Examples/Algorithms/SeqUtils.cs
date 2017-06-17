using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples.Algorithms
{
    public static class SeqUtils
    {

        /// <summary>
        /// Creates a mathematical sequence that copies the contents of an array.
        /// </summary>
        /// <param name="array">The array to make into a sequence.</param>
        public static Seq<int> ArrayToSeq(int[] array)
        {
            Contract.Requires(AccArray(array));
            Contract.Ensures(AccArray(array));
            Contract.Ensures(Contract.Result<Seq<int>>().Length == array.Length);
            Contract.Ensures(ForAll((i) => (0 <= i && i < array.Length).Implies(array[i] == Contract.Result<Seq<int>>()[i])));

            if (array.Length == 0)
            {
                return new Seq<int>();
            }

            var result = new Seq<int>(array[0]);
            int ind = 1;

            while (ind != array.Length)
            {
                Contract.Invariant(AccArray(array));
                Contract.Invariant(result.Length == ind);
                Contract.Invariant(array.Length >= ind);
                Contract.Invariant(ForAll((i) => (0 <= i && i < ind).Implies(array[i] == result[i])));

                result = result + new Seq<int>(array[ind]);
                ind++;
            }
            
            return result;
        }

        // To implement the reverse method, SeqToArray, we would need either the ability to append arrays or to create an array with
        // a length specified at runtime. The former is not implemented and the latter seems hard, using our method of translating arrays.
    }
}
