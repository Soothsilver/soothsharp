using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Examples.Algorithms;
using Xunit;

namespace Soothsharp.Translation.Tests.Algorithms
{
    public class SearchTests
    {
        [Fact]
        public void GetSmallestNumberTest()
        {
            Assert.Equal(2, Search.GetSmallestNumber(new Contracts.Seq<int>(3, 8, 2, 6)));
            Assert.Equal(0, Search.GetSmallestNumber(new Contracts.Seq<int>(3, 0, 6)));
            Assert.Equal(9, Search.GetSmallestNumber(new Contracts.Seq<int>(9)));
        }

        [Fact]
        public void BinarySearchTest()
        {
            Assert.Equal(2, Search.BinarySearch(new Contracts.Seq<int>(0, 10, 20, 30, 40, 50, 60, 70), 20));
            Assert.Equal(7, Search.BinarySearch(new Contracts.Seq<int>(0, 10, 20, 30, 40, 50, 60, 70), 70));
            Assert.Equal(0, Search.BinarySearch(new Contracts.Seq<int>(0, 10, 20, 30, 40, 50, 60, 70), 0));
            Assert.Equal(-1, Search.BinarySearch(new Contracts.Seq<int>(0, 10, 20, 30, 40, 50, 60, 70), 55));
            Assert.Equal(-1, Search.BinarySearch(new Contracts.Seq<int>(), 55));
        }
    }
}
