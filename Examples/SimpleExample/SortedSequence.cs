using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Contracts;
using static Sharpsilver.Contracts.Contract;

namespace Sharpsilver.Examples
{
    public class SortedSequence
    {
        public Seq<int> Data;

        public int Insert(int element)
        {
            Requires(Contract.Acc(Data) /* && sorted etc etc TODO */);
            Ensures(Contract.Acc(Data));
            Ensures(0 <= IntegerResult && IntegerResult < Old(Data.Length));
            Ensures(Data == Old(Data.Range(0, IntegerResult) + new Seq<int>(element) + Old(this.Data).RangeFrom(IntegerResult)));

            int idx = 0;
            while (idx < Data.Length && Data[idx] < element)
            {
                idx = idx + 1;
            }
            Data = Data.Range(0, idx) + new Seq<int>(element) + Data.RangeFrom(idx);
            return idx;
        }
    }

    public static class MainClass
    {
        [Unverified]
        public static void Main(string[] args)
        {
            SortedSequence seq = new SortedSequence();

        }
    }
}
