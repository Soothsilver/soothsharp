using System;
using System.Collections.Generic;

namespace Sharpsilver.Translation.Trees.Silver
{
    public class CSharpTypeDomainSilvernode : ComplexSilvernode
    {
        private readonly HighlevelSequenceSilvernode axioms;

        public CSharpTypeDomainSilvernode(HighlevelSequenceSilvernode axioms) : base(null)
        {
            this.axioms = axioms;
        }

        protected override IEnumerable<Silvernode> Children
        {
            get
            {
                return new Silvernode[]
                {
                    "domain " + Constants.CSharpTypeDomain + " {\n",
                    "\tfunction " + Constants.TypeOfFunction + "(object: Ref): " + Constants.CSharpTypeDomain + "\n",
                    "\tfunction " + Constants.IsSubTypeFunction + "(subtype: " + Constants.CSharpTypeDomain +
                    ", supertype: " + Constants.CSharpTypeDomain + "): Bool\n",
                    axioms,
                    "}"
                };
            }
        }
    }
}