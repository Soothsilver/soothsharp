using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Contracts
{
    public class Seq<T>
    {
        private List<T> list = new List<T>(1);
        public Seq(T initialElement)
        {
            list.Add(initialElement);
        }

        private Seq(List<T> list)
        {
            this.list = list;
        }

        public int Length => list.Count;

        public Seq<T> Range(int from, int to)
        {
            return new Seq<T>(list.GetRange(from, to - from));
        }

        public Seq<T> RangeFrom(int from)
        {
            return new Seq<T>(list.Skip(from).ToList());
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public T this[int index] => list[index];

        public static Seq<T> operator +(Seq<T> one, Seq<T> other)
        {
            return new Seq<T>(one.list.Union(other.list).ToList());
        }
    }
}
