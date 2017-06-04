using Microsoft.CodeAnalysis;
using System;

namespace Soothsharp.Translation.Trees.Silver
{
    public abstract class ContractSilvernode : Silvernode, IComparable<ContractSilvernode>
    {
        protected ContractSilvernode(SyntaxNode syntaxNode) : base(syntaxNode)
        {

        }

        /// <summary>
        /// Contract silvernodes are order in as "requires &lt; ensures" so that preconditions
        /// are before postcondition, regardless of their order in C# source code.
        /// </summary>
        public abstract int CompareTo(ContractSilvernode other);

        public override bool IsVerificationCondition()
        {
            return true;
        }
    }
}
