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
        public Node Right;
        public Node Bottom;

        [Predicate]
        public static bool AccessAllTree(Node self, Node end)
        {
            return (self != end).Implies(
                Acc(self.Right) && Acc(self.Bottom) && 
                Acc(AccessAllTree(self.Right, end)) &&
                Acc(AccessAllTree(self.Bottom, end))
                );
        }

        [Pure]
        public bool CanReachNode(Node target, Node finalStop)
        { 
            Contract.Requires(Truth && AccessAllTree(this, finalStop));

            return Unfolding(
                Truth && Acc(AccessAllTree(this, finalStop)), 
                this == target ? true : 
                (this == finalStop ? false : 
                 Right.CanReachNode(target, finalStop) || Bottom.CanReachNode(target, finalStop)
                )
                );
        }
      
    }
}
