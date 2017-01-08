using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class EnsuresSilvernode : VerificationConditionSilvernode
    {
        private Silvernode Postcondition;

        public EnsuresSilvernode(Silvernode postcondition, SyntaxNode originalNode) : base(originalNode)
        {
            this.Postcondition = postcondition;
        }

        public override int CompareTo(VerificationConditionSilvernode other)
        {
            if (other.GetType() == typeof(EnsuresSilvernode))
                return 0;
            else
                return 1;
        }
        
        public override string ToString()
        {
            return "ensures " + this.Postcondition + "";
        }
    }
}
