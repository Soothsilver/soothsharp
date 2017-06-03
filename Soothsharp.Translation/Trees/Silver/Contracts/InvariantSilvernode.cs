using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class InvariantSilvernode : ContractSilvernode
    {
        private Silvernode Invariant;

        public InvariantSilvernode(Silvernode invariant, SyntaxNode originalNode) : base(originalNode)
        {
            this.Invariant = invariant;
        }

        public override int CompareTo(ContractSilvernode other)
        {
            if (other.GetType() == typeof(InvariantSilvernode))
                return 0;
            else
                return 1;
        }
        
        public override string ToString()
        {
            return "invariant " + this.Invariant + "";
        }
    }
}
