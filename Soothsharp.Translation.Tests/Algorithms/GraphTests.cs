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
            Node x00 = new Node();
            Node x01 = new Node();
            Node x02 = new Node();
            Node x10 = new Node();
            Node x11 = new Node();
            Node x12 = new Node();
            Node x20 = new Node();
            Node x21 = new Node();
            Node x22 = new Node();

            x00.Bottom = x01;
            x00.Right = x10;
            x01.Bottom = x02;
            x01.Right = x11;
            x02.Bottom = finalStop;
            x02.Right = x12;
            
            x10.Bottom = x11;
            x10.Right = x20;
            x11.Bottom = x12;
            x11.Right = x21;
            x12.Bottom = finalStop;
            x12.Right = x22;

            x20.Bottom = x21;
            x20.Right = finalStop;
            x21.Bottom = x22;
            x21.Right = finalStop;
            x22.Bottom = finalStop;
            x22.Right = finalStop;


            Assert.True(x00.CanReachNode(x22, finalStop));
            Assert.True(x10.CanReachNode(x21, finalStop));
            Assert.False(x21.CanReachNode(x10, finalStop));
        }
    }
}
