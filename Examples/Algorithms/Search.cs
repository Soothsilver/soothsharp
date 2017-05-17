using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples.Algorithms
{
    public static class Search
    {


        /// <summary>
        /// Gets the smallest integer in the sequence.
        /// </summary>
        /// <param name="xs">The sequence to search.</param>
        public static int GetSmallestNumber(Seq<int> xs)
        {
            throw new NotImplementedException();
        }

         /// <summary>
         /// Searches a sorted sequence of integers for a value. If the value is found, this method returns
         /// the index of the value in the sequence. If the value is not present, the method returns -1.
         /// </summary>
         /// <param name="xs">The sorted immutable sequence of integers.</param>
         /// <param name="key">The value to search for in the sequence.</param>
         /// <returns>Index of the value in the sequence, or -1 if the value is not present.</returns>
        public static int BinarySearch(Seq<int> xs, int key)
        {
            // As a precondition, we assume the sequence is sorted:
            Requires(ForAll(i => ForAll(j => (0 <= i && j < xs.Length && i < j).Implies(xs[i] < xs[j]))));
            // The returned integer is either -1 or an index in the sequence:
            Ensures(-1 <= IntegerResult && IntegerResult < xs.Length);
            // If it's not -1, then the searched value as at the returned index:
            Ensures((0 <= IntegerResult).Implies(xs[IntegerResult] == key));
            // If it is -1, then the searched value is not in the sequence.
            Ensures((-1 == IntegerResult).Implies(ForAll(i => (0 <= i && i < xs.Length).Implies(xs[i] != key))));

            // The rest of this method can be assumed to be correct if the above contracts are correct
            // and verification passes
            int low = 0;
            int high = xs.Length;
            int index = -1;
            // Binary search follows:
            while (low < high && index == -1)
            {
                Invariant(0 <= low && low <= high && high <= xs.Length);
                Invariant((index == -1).Implies(ForAll(i =>
                    (0 <= i && i < xs.Length && !(low <= i && i < high)).Implies(xs[i] != key)
                    )));

                Invariant(-1 <= index && index < xs.Length);
                Invariant((0 <= index).Implies(xs[index] == key));

                int mid = (low + high) / 2;
                if (xs[mid] < key)
                {
                    low = mid + 1;
                }
                else
                {
                    if (key < xs[mid])
                    {
                        high = mid;
                    }
                    else
                    {
                        index = mid;
                        high = mid;
                    }
                }
            }
            return index;
        }

    }
}
