using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver.Statements
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

        protected override IEnumerable<Silvernode> Children
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
                        condition =>
                        {
                            if (condition is TextSilvernode) return new Silvernode[] { condition };
                            return new Silvernode[] { "\t", condition };
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