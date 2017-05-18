using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Examples.Algorithms;
using Xunit;

namespace Soothsharp.Translation.Tests.Algorithms
{
    public class ArithmeticTests
    {
        [Fact]
        public void MaxTest()
        {
            Assert.Equal(2, Arithmetic.Max(2, 1));
            Assert.Equal(6, Arithmetic.Max(1, 6));
            Assert.Equal(3, Arithmetic.Max(3, -8));
        }

        [Fact]
        public void MinTest()
        {
            Assert.Equal(-4, Arithmetic.Min(7, -4));
            Assert.Equal(-7, Arithmetic.Min(-7, -4));
            Assert.Equal(2, Arithmetic.Min(2, 4));
        }

        [Fact]
        public void AbsTest()
        {
            Assert.Equal(4, Arithmetic.Abs(-4));
            Assert.Equal(7, Arithmetic.Abs(7));
            Assert.Equal(0, Arithmetic.Abs(0));
        }
    }
}
