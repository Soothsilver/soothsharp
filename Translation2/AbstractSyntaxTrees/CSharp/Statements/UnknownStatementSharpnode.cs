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
        public UnknownStatementSharpnode(StatementSyntax node) : base(node)
        {
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return TranslationResult.Error(OriginalNode, Diagnostics.SSIL101_UnknownNode, OriginalNode.Kind());
        }
    }
}
