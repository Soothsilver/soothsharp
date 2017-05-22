using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Contracts;
using static Soothsharp.Contracts.Contract;
using static Soothsharp.Contracts.Permission;
// ReSharper disable SimplifyConditionalTernaryExpression

namespace Soothsharp.Examples.Algorithms
{
    public class Node
    {
        public Node Next;

        [Predicate]
        public static bool AccessAllTree(Node self, Node end)
        {
            return (self != end).Implies( Acc(self.Next) && Acc(AccessAllTree(self.Next, end)));
        }

        [Pure]
        public bool CanReachOneBeforeTheOther(Node target, Node finalStop)
        { 
            Contract.Requires(Truth && AccessAllTree(this, finalStop));

            return Unfolding(Truth && Acc(AccessAllTree(this, finalStop)), this == target ? true : (this == finalStop ? false : Next.CanReachOneBeforeTheOther(target, finalStop)));
        }

        /*
        [Predicate]
        public static bool Good(Node node)
        {
            return Acc(node.Bottom) && Acc(node.Right);
        }

        /// <summary>
        /// Determines whether it's possible to reach the target node from this node.
        /// </summary>
        /// <param name="target">The target to reach.</param>
        [Pure]
        public bool CanReachNode(Node target)
        {
            Requires(Good(this));


            Ensures(Unfolding(Good(this), (Bottom != null && Bottom.CanReachNode(target)))); //|| (Right != null && Right.CanReachNode(target)) || this == target);

            return true; //(Bottom != null && Bottom.CanReachNode(target)) || (Right != null && Right.CanReachNode(target)) ||
                   //this == target;
        }*/
    }
}
