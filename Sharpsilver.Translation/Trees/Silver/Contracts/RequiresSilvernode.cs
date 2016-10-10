using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation.Trees.Silver
{
    class RequiresSilvernode : VerificationConditionSilvernode
    {
        public Silvernode Precondition;

        public RequiresSilvernode(Silvernode precondition, SyntaxNode originalNode) : base(originalNode)
        {
            Precondition = precondition;
        }

        public override int CompareTo(VerificationConditionSilvernode other)
        {
            if (other.GetType() == typeof(RequiresSilvernode))
                return 0;
            else
                return -1;
        }
        
        public override string ToString()
        {
            return "requires (" + Precondition + ")";
        }
    }
}
