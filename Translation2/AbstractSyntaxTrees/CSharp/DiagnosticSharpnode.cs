using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation.AbstractSyntaxTrees.CSharp
{
    class DiagnosticSharpnode : Sharpnode
    {
        private SharpsilverDiagnostic diagnostic;
        private object[] parameters;
        public DiagnosticSharpnode(SyntaxNode node, SharpsilverDiagnostic diagnostic, params object[] parameters) : base(node)
        {
            this.diagnostic = diagnostic;
            this.parameters = parameters;
            if (this.parameters == null)
            {
                this.parameters = new object[0];
            }
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return TranslationResult.Error(OriginalNode, diagnostic, parameters);
        }
    }
}
