using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    /// <summary>
    /// Represents a finite sequence of objects. This class is translated into Silver as Silver's Seq type.
    /// </summary>
    /// <typeparam name="T">Type of the objects in the sequence</typeparam>
    public class Seq<T>
    {
        private List<T> list = new List<T>(1);
        /// <summary>
        /// Initializes a new sequence with a single element.
        /// </summary>
        /// <param name="initialElement">The only element that will be in the sequence</param>
        public Seq(T initialElement)
        {
            list.Add(initialElement);
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
        /// Gets the number of elements in the sequence. This is backed by Silver's absolute value operator (<code>|seq|</code>).
        /// </summary>
        public int Length => list.Count;

        /// <summary>
        /// Returns the subsequence that starts at an index and ends at another index.
        /// </summary>
        /// <param name="from">The beginning index, inclusive.</param>
        /// <param name="to">The end index, inclusive.</param>
        public Seq<T> Range(int from, int to)
        {
            return new Seq<T>(list.GetRange(from, to - from));
        }

        /// <summary>
        /// Returns the subsequence that starts at the given index.
        /// </summary>
        /// <param name="from">The beginning index, inclusive.</param>
        public Seq<T> RangeFrom(int from)
        {
            return new Seq<T>(list.Skip(from).ToList());
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
