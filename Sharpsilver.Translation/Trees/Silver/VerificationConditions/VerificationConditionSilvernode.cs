using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
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
