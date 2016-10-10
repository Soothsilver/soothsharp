using System.Collections.Generic;

namespace Sharpsilver.Translation.Trees.Silver
{
    /// <summary>
    /// Represents the "domain CSharpType" construction that is put into all generated Silver files by the transcompiler. 
    /// See the documentation for more deatils.
    /// </summary>
    /// <seealso cref="Sharpsilver.Translation.Trees.Silver.ComplexSilvernode" />
    public class CSharpTypeDomainSilvernode : ComplexSilvernode
    {
        private readonly HighlevelSequenceSilvernode axioms;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpTypeDomainSilvernode"/> class.
        /// </summary>
        /// <param name="axioms">Axioms that should be put inside this domain. Must not be null.</param>
        public CSharpTypeDomainSilvernode(HighlevelSequenceSilvernode axioms) : base(null)
        {
            this.axioms = axioms;
        }

        public override IEnumerable<Silvernode> Children
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
                    "\n}"
                };
            }
        }
    }
}