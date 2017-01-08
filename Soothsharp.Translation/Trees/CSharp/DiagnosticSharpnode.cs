using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation.Trees.CSharp
{
    class DiagnosticSharpnode : Sharpnode
    {
        private SoothsharpDiagnostic diagnostic;
        private object[] parameters;
        public DiagnosticSharpnode(SyntaxNode node, SoothsharpDiagnostic diagnostic, params object[] parameters) : base(node)
        {
            this.diagnostic = diagnostic;
            this.parameters = parameters ?? new object[0];
        }

        public override TranslationResult Translate(TranslationContext translationContext)
        {
            return TranslationResult.Error(this.OriginalNode, this.diagnostic, this.parameters);
        }
    }
}
