using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples.Algorithms
{
    public static class Sorting
    {
        /// <summary>
        /// Performs insert sort and returns the sorted sequence. The number in input are expected
        /// to be unique.
        /// </summary>
        /// <param name="input">Integers to sort. A single integer may appear only once.</param>
        /// <returns></returns>
        public static Seq<int> InsertSort(Seq<int> input)
        {
            Contract.Requires(input.Length >= 1);

            // Result is sorted.
            Ensures(
                ForAll((i) => ForAll((j) => (0 <= i && i < j && j < Result<Seq<int>>().Length).Implies(Result<Seq<int>>()[i] <= Result<Seq<int>>()[j]))));

            // Result contains the same numbers. 
            // Ensuring this in a problem where we allow duplicities would be harder.
            Ensures(
                ForAll((i) => (0 <= i && i < input.Length).Implies(Result<Seq<int>>().Contains(input[i]))));



            SortedList list = new SortedList();

            int index = 0;

            while (index < input.Length)
            {
                Invariant(index >= 0);
                Invariant(Acc(list.Elements));
                // Maintains sort:
                Invariant(ForAll((i) => ForAll((j) => (0 <= i && i < j && j < list.Elements.Length).Implies(list.Elements[i] <= list.Elements[j]))));
                // Number is copied:
                Invariant(ForAll(i => (0 <= i && i < index).Implies(list.Elements.Contains(input[i]))));

                list.Insert(input[index]);
                index++;
            }

            return list.Elements;
        }
    }
}
