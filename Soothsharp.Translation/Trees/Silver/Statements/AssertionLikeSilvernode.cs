using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class AssertionLikeSilvernode : StatementSilvernode
    {
        private Silvernode Assertion;
        private string Keyword;
        public AssertionLikeSilvernode(string keyword, Silvernode assertion, SyntaxNode original) : base(original)
        {
            this.Assertion = assertion;
            this.Keyword = keyword;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                yield return this.Keyword + " ";
                yield return this.Assertion;
            }
        }
    }
}
