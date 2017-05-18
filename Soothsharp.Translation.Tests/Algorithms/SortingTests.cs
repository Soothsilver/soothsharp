using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Examples.Algorithms;
using Xunit;

namespace Soothsharp.Translation.Tests.Algorithms
{
    public class SortingTests
    {
        [Fact]
        public void InsertSortTest()
        {
            var r = Sorting.InsertSort(new Contracts.Seq<int>(50, 40, 10, 80, 60));

            Assert.Equal(5, r.Length);
            Assert.Equal(10, r[0]);
            Assert.Equal(40, r[1]);
            Assert.Equal(50, r[2]);
            Assert.Equal(60, r[3]);
            Assert.Equal(80, r[4]);
        }
    }
}
