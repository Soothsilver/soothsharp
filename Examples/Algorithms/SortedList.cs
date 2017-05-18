using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;

namespace Soothsharp.Examples.Algorithms
{
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
                ForAll((i)=>ForAll((j)=> (0 <= i && i < j && j < Elements.Length).Implies(Elements[i] <= Elements[j])))
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
}
