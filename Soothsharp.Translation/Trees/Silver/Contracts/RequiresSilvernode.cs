using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.Silver
{
    class RequiresSilvernode : VerificationConditionSilvernode
    {
        private Silvernode Precondition;

        public RequiresSilvernode(Silvernode precondition, SyntaxNode originalNode) : base(originalNode)
        {
            this.Precondition = precondition;
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
            return "requires " + this.Precondition + "";
        }
    }
}
