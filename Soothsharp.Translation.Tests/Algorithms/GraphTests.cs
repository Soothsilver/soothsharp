using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using Soothsharp.Examples.Algorithms;
using Xunit;

namespace Soothsharp.Translation.Tests.Algorithms
{
    public class GraphTests
    {
        [Fact]
        public void Line()
        {
            Node finalStop = new Node();
            Node c = new Node();
            Node b = new Node();
            Node a = new Node();
            c.Next = finalStop;
            b.Next = finalStop;
            a.Next = finalStop;

            Assert.True(a.CanReachOneBeforeTheOther(c, finalStop));
            Assert.True(b.CanReachOneBeforeTheOther(c, finalStop));
            Assert.False(c.CanReachOneBeforeTheOther(a, finalStop));
        }
    }
}
