using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Soothsharp.Translation.Trees.CSharp
{
    class DiagnosticExpressionSharpnode : ExpressionSharpnode
    {
        private SoothsharpDiagnostic diagnostic;
        private object[] parameters;
        public DiagnosticExpressionSharpnode(ExpressionSyntax node, SoothsharpDiagnostic diagnostic, params object[] parameters) : base(node)
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
