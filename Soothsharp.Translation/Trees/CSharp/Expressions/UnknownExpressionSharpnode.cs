using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.CSharp
{
    class UnknownExpressionSharpnode : ExpressionSharpnode
    {
        string featureName;
        public UnknownExpressionSharpnode(ExpressionSyntax node, string featureName = null) : base(node)
        {
            this.featureName = featureName;
        }


        public override TranslationResult Translate(TranslationContext translationContext)
        {
            if (this.featureName == null)
            {
                return TranslationResult.Error(this.OriginalNode, Diagnostics.SSIL101_UnknownNode, this.OriginalNode.Kind());
            } else
            {
                return TranslationResult.Error(this.OriginalNode,
                    Diagnostics.SSIL108_FeatureNotSupported, this.featureName);
            }
        }
    }
}
