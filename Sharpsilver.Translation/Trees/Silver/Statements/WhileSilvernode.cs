using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class WhileSilvernode : StatementSilvernode
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

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return "while (";
                yield return condition;
                yield return ")";

                var children = new List<Silvernode>();
                if (verificationConditions.Any())
                {
                    children.Add("\n" + Tabs());
                    children.AddRange(verificationConditions.WithSeparator<Silvernode>("\n" + Tabs()).SelectMany(
                        innerCondition
                        =>
                        {
                            if (innerCondition
 is TextSilvernode) return new[] { innerCondition
 };
                            return new[] { "\t", innerCondition
 };
                        }
                        ));
                    children.Add("\n" + Tabs());
                }
                else
                {
                    children.Add(" ");
                }
                foreach (var a in children)
                {
                    yield return a;
                }
                yield return statementBlock;
            }
        }
    }
}