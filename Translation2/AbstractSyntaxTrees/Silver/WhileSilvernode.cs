using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.Silver
{
    class WhileSilvernode : Silvernode
    {
        private Silvernode condition;
        private BlockSilvernode statementBlock;
        private List<VerificationConditionSilvernode> verificationConditions;

        public WhileSilvernode(
            Silvernode condition, 
            List<VerificationConditionSilvernode> verificationConditions, 
            BlockSilvernode statementBlock, 
            SyntaxNode originalNode) : base(originalNode)
        {
            this.condition = condition;
            this.verificationConditions = verificationConditions;
            this.statementBlock = statementBlock;
        }

        public override string ToString()
        {
            return "while (" + condition.ToString() + ")"
                + (verificationConditions.Any() ? "\n" + VerificationConditionsToString() + "\n" : " ")
                + statementBlock.ToString();
        }

        private string VerificationConditionsToString()
        {
            return String.Join("\n", verificationConditions.Select(cond => "\t" + cond.ToString()));
        }
    }
}