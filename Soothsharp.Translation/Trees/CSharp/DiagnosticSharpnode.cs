using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.CSharp
{
    class DiagnosticSharpnode : Sharpnode
    {
        private SharpsilverDiagnostic diagnostic;
        private object[] parameters;
        public DiagnosticSharpnode(SyntaxNode node, SharpsilverDiagnostic diagnostic, params object[] parameters) : base(node)
        {
            this.diagnostic = diagnostic;
            this.parameters = parameters ?? new object[0];
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return TranslationResult.Error(OriginalNode, diagnostic, parameters);
        }
    }
}
