using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Examples.Algorithms;
using Xunit;

namespace Soothsharp.Translation.Tests.Algorithms
{
    public class SeqUtilsTests
    {
        public void TransformTest()
        {
            Assert.Equal(2, SeqUtils.ArrayToSeq(new[] {1, 8}).Length);
            Assert.Equal(8, SeqUtils.ArrayToSeq(new[] {1, 8})[1]);
        }
    }
}
