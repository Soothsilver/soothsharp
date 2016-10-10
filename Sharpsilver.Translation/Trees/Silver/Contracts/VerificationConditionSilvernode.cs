using Microsoft.CodeAnalysis;
using System;

namespace Sharpsilver.Translation.Trees.Silver
{
    public abstract class VerificationConditionSilvernode : Silvernode, IComparable<VerificationConditionSilvernode>
    {
        protected VerificationConditionSilvernode(SyntaxNode syntaxNode) : base(syntaxNode)
        {

        }

        public abstract int CompareTo(VerificationConditionSilvernode other);

        public override bool IsVerificationCondition()
        {
            return true;
        }
    }
}
