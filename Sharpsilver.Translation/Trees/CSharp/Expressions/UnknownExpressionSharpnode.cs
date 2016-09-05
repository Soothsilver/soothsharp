using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.Trees.CSharp
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
            if (featureName == null)
            {
                return TranslationResult.Error(OriginalNode, Diagnostics.SSIL101_UnknownNode, OriginalNode.Kind());
            } else
            {
                return TranslationResult.Error(OriginalNode,
                    Diagnostics.SSIL108_FeatureNotSupported, featureName);
            }
        }
    }
}
