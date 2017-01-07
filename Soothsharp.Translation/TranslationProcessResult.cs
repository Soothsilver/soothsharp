using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    public class TranslationProcessResult
    {
        public Silvernode Silvernode;
        public bool WasTranslationSuccessful
        {
            get
            {
                return Errors.All(err => err.Diagnostic.Severity != DiagnosticSeverity.Error);
            }
        }
        public List<Error> Errors { get; } 
        public TranslationProcessResult(Silvernode silvernode, List<Error> errors)
        {
            Silvernode = silvernode;
            Errors = errors;
        }
        public static TranslationProcessResult Error(SyntaxNode syntaxNode, SoothsharpDiagnostic diagnostic, params object[] arguments)
        {
            return new TranslationProcessResult(null, new List<Error> { new Error(diagnostic, syntaxNode, arguments) });
        }
    }
}
