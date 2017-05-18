using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Examples.Algorithms;
using Xunit;

namespace Soothsharp.Translation.Tests.Algorithms
{
    public class TupleTests
    {
        [Fact]
        public void Test()
        {
            VerifiedTuple t = new VerifiedTuple(10, 20);
            Assert.Equal(10, t.First);
            Assert.Equal(20, t.Second);
            t.Swap();
            Assert.Equal(20, t.First);
            Assert.Equal(10, t.Second);
            t.Swap();
            t.Swap();
            Assert.Equal(20, t.First);
            Assert.Equal(10, t.Second);
            t.Swap();
            Assert.Equal(10, t.First);
            Assert.Equal(20, t.Second);
            t.First = 40;
            Assert.Equal(40, t.First);
        }
    }
}
