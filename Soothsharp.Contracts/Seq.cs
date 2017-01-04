using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soothsharp.Contracts
{
    /// <summary>
    /// Represents a finite immutable mathematical sequence of objects. This class is translated into Viper as Viper's Seq type.
    /// </summary>
    /// <typeparam name="T">Type of the objects in the sequence</typeparam>
    public class Seq<T>
    {
        private List<T> list = new List<T>(1);

        /// <summary>
        /// Initializes a new sequence.
        /// </summary>
        /// <param name="elements">The elements in the sequence. Must not be given as an array - each argument must be a single element.</param>
        public Seq(params T[] elements)
        {
            list = new List<T>(elements);
        }

        /// <summary>
        /// Initializes a new sequence from a <see cref="List{T}"/> instance. This is only meant to be used internally.
        /// </summary>
        /// <param name="list">The new backing list that replaces the old one.</param>
        private Seq(List<T> list)
        {
            this.list = list;
        }

        /// <summary>
        /// Gets the number of elements in the sequence. This is backed by Viper's absolute value operator (<code>|seq|</code>).
        /// </summary>
        public int Length => list.Count;

        /// <summary>
        /// Returns the subsequence that drops the first elements.
        /// </summary>
        /// <param name="howMany">The number of elements to skip at the beginning.</param>
        public Seq<T> Drop(int howMany)
        {
            return new Seq<T>(list.Skip(howMany).ToList());
        }
        /// <summary>
        /// Returns the subsequence that contains only the first elements.
        /// </summary>
        /// <param name="howMany">The number of elements to take at the beginning.</param>
        public Seq<T> Take(int howMany)
        {
            return new Seq<T>(list.Take(howMany).ToList());
        }
        /// <summary>
        /// Returns the subsequence that first TAKES the first "take" elements and then DROPS the first "drop" elements.
        /// </summary>  
        /// <param name="drop">The number of elements to drop at the beginning.</param>
        /// <param name="take">The number of elements to take at the beginning.</param>
        public Seq<T> Take(int drop, int take)
        {
            return new Seq<T>(list.Take(take).Skip(drop).ToList());
        }

        /// <summary>
        /// Determines whether the sequence contains the specified item.
        /// </summary>
        /// <param name="item">The item to test for.</param>
        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public T this[int index] => list[index];

        /// <summary>
        /// Concatenates two sequences.
        /// </summary>
        /// <param name="one">The first sequence.</param>
        /// <param name="other">The second sequence.</param>
        /// <returns>
        /// The concatenated sequence.
        /// </returns>
        public static Seq<T> operator +(Seq<T> one, Seq<T> other)
        {
            return new Seq<T>(one.list.Union(other.list).ToList());
        }
    }
}
