using Microsoft.CodeAnalysis;
using System;

namespace Soothsharp.Translation.Trees.Silver
{
    public abstract class ContractSilvernode : Silvernode, IComparable<ContractSilvernode>
    {
        protected ContractSilvernode(SyntaxNode syntaxNode) : base(syntaxNode)
        {

        }

        public abstract int CompareTo(ContractSilvernode other);

        public override bool IsVerificationCondition()
        {
            return true;
        }
    }
}
