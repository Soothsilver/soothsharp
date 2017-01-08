using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.CSharp
{
    class UnknownStatementSharpnode : StatementSharpnode
    {
        private string reason;

        public UnknownStatementSharpnode(StatementSyntax node) : base(node)
        {
        }
        public UnknownStatementSharpnode(StatementSyntax node, string reason) : base(node)
        {
            this.reason = reason;
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            if (this.reason != null)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL108_FeatureNotSupported, this.reason);
            }
            return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL101_UnknownNode, this.OriginalNode.Kind());
        }
    }
}
