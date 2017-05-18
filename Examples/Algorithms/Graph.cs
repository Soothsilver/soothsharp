using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;

namespace Soothsharp.Examples.Algorithms
{
    class Node
    {
        public Seq<Node> AdjacentNodes;

        public Node()
        {
            this.AdjacentNodes = new Seq<Algorithms.Node>();
        }

        public Node(Seq<Node> neighbours)
        {
            this.AdjacentNodes = neighbours;
        }

        /// <summary>
        /// Determines whether it's possible to reach the target node from this node.
        /// </summary>
        /// <param name="target">The target to reach.</param>
        public bool CanReachNode(Node target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of nodes that must be traversed in order to reach the target from this node, including the target.
        /// </summary>
        /// <param name="target">The target.</param>
        public int DistanceToNode(Node target)
        {
            throw new NotImplementedException();
        }
    }
}
