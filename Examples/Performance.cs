using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
using static Soothsharp.Contracts.Permission;

namespace Soothsharp.Examples.Performance
{
    /// <summary>
    /// This file is meant to test the performance of Soothsharp.
    /// </summary>
    class PerformanceClass
    {
    }
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
                Contract.Invariant(ForAll((i) => (0 <= i && i < ind).Implies(array[i] == result[i])));

                result = result + new Seq<int>(array[ind]);
                ind++;
            }

            return result;
        }

        // To implement the reverse method, SeqToArray, we would need either the ability to append arrays or to create an array with
        // a length specified at runtime. The former is not implemented and the latter seems hard, using our method of translating arrays.
    }
    class DataEx
    {
        public int Value;
    }
    class CEx
    {
        public static int ReadValue(DataEx d)
        {
            Requires(Acc(d.Value, Wildcard));

            return d.Value;
        }
        public static int ReadValueError(DataEx d)
        {
            Requires(Acc(d.Value, Wildcard));

            d.Value = 3; // <= This will trigger an error.
            return d.Value;
        }
    }
    static class StaticClass
    {
        public static int Maximum(int a, int b)
        {
            Ensures(a > b ? IntegerResult == a : IntegerResult == b);
            if (a >= b)
                return a;
            else
                return b;
        }
        public static int MaximumFailing(int a, int b)
        {
            Ensures(a > b ? IntegerResult == a : IntegerResult == b);
            return a;
        }
        static int BinarySearch(Seq<int> xs, int key)
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
        [Unverified]
        public static void NobodyCares()
        {
            int nope = 4;
        }
    }


    [Unverified]
    static class UnverifiedClass
    {
        [Verified]
        public static void Sth()
        {
            int thisWillNeverRun = 4;
        }
    }
}

namespace Soothsharp.Examples.Performance
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
    /// <summary>
    /// A verifiably sorted list of integers.
    /// </summary>
    public class SortedList
    {
        /// <summary>
        /// Gets the list of integers in the list, in ascending order.
        /// </summary>
        public Seq<int> Elements;

        /// <summary>
        /// Initializes the sorted list. It will start out empty.
        /// </summary>
        public SortedList()
        {
            Elements = new Seq<int>();


            Ensures(Acc(Elements));
            Ensures(Elements.Length == 0);
        }

        /// <summary>
        /// Inserts a new integer into the list. The index of the newly inserted integer is returned.
        /// </summary>
        /// <param name="element">The integer to add to the list.</param>
        /// <returns>Index of the new element that this method adds to the list.</returns>
        /// <remarks>
        /// Source: http://viper.ethz.ch/examples/sorted-list-immutable-sequence.html
        /// </remarks>
        public int Insert(int element)
        {
            Requires(Acc(Elements) &&
                     ForAll((i) => ForAll((j) => (0 <= i && i < j && j < Elements.Length).Implies(Elements[i] <= Elements[j])))
            );
            Ensures(Acc(Elements) &&
                    ForAll((i) => ForAll((j) => (0 <= i && i < j && j < Elements.Length).Implies(Elements[i] <= Elements[j])))
            );
            Ensures(0 <= IntegerResult && IntegerResult <= Old(Elements.Length));
            Ensures(Elements ==
                    Old(Elements).Take(IntegerResult) + new Seq<int>(element) + Old(Elements).Drop(IntegerResult));
            Ensures(Elements.Contains(element));
            Ensures(ForAll(i => Old(Elements).Contains(i).Implies(Elements.Contains(i))));

            int index = 0;
            while (index < Elements.Length && Elements[index] < element)
            {
                Invariant(Acc(Elements, Permission.Half));
                Invariant(0 <= index && index <= this.Elements.Length);
                Invariant(ForAll((i) => (0 <= i && i < index).Implies(Elements[i] < element)));

                index++;
            }

            Elements = Elements.Take(index) + new Seq<int>(element) + Elements.Drop(index);
            return index;
        }
    }
    class InvalidArrayWriteRead
    {
        public static void ArrayRead(int[] array)
        {
            Contract.Requires(AccArray(array));
            Contract.Requires(array.Length == 2);

            int invalidRead = array[8];
        }
        public static void ArrayWrote(int[] array)
        {
            Contract.Requires(AccArray(array));
            Contract.Requires(array.Length == 2);

            array[16] = 50;
        }
    }
    class CustomGoto
    {
        public static void main()
        {
            hello:;
            int a = 4;
            Contract.Assert(a == 4);
            if (a == 2) goto hi;
            goto hello;
            hi:
            Contract.Assert(false);
        }
    }
}
