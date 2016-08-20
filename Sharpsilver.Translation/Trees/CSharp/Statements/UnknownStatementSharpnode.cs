using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    class UnknownStatementSharpnode : StatementSharpnode
    {
        private string _reason;

        public UnknownStatementSharpnode(StatementSyntax node) : base(node)
        {
        }
        public UnknownStatementSharpnode(StatementSyntax node, string reason) : base(node)
        {
            _reason = reason;
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            if (_reason != null)
            {
                return TranslationResult.Error(OriginalNode, Diagnostics.SSIL108_FeatureNotSupported, _reason);
            }
            return TranslationResult.Error(OriginalNode, Diagnostics.SSIL101_UnknownNode, OriginalNode.Kind());
        }
    }
}
