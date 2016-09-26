using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Sharpsilver.Translation.Trees.Silver.Statements;

namespace Sharpsilver.Translation.Trees.Silver
{
    class AssertionLikeSilvernode : StatementSilvernode
    {
        public Silvernode Assertion;
        public string Keyword;
        public AssertionLikeSilvernode(string keyword, Silvernode assertion, SyntaxNode original) : base(original)
        {
            Assertion = assertion;
            Keyword = keyword;
        }

        public override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return Keyword + " ";
                yield return Assertion;
            }
        }
    }
}
