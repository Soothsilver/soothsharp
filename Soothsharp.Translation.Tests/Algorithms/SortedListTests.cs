using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Examples.Algorithms;
using Xunit;

namespace Soothsharp.Translation.Tests.Algorithms
{
    public class SortedListTests
    {
        [Fact]
        public void BasicTest()
        {
            SortedList l = new SortedList();
            l.Insert(40);
            l.Insert(10);
            l.Insert(50);
            l.Insert(30);
            l.Insert(20);

            Assert.Equal(10, l.Elements[0]);
            Assert.Equal(20, l.Elements[1]);
            Assert.Equal(30, l.Elements[2]);
            Assert.Equal(40, l.Elements[3]);
            Assert.Equal(50, l.Elements[4]);
            Assert.Equal(5, l.Elements.Length);
        }
    }
}
