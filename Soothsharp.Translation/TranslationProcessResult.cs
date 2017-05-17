using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents all the results of a single run of a translation process. This is used by the front-end, by the plugin,
    /// and by automated tests.
    /// </summary>
    public class TranslationProcessResult
    {
        /// <summary>
        /// Gets the Viper tree that's the result of the translation. May be null if the translation failed totally, such
        /// as if the source files could not be opened.
        /// </summary>
        public Silvernode Silvernode { get; }

        /// <summary>
        /// Gets a value that indicates whether the translation completed without any errors. If that's the case, 
        /// <see cref="Silvernode"/> is guaranteed to be non-null. 
        /// </summary>
        public bool WasTranslationSuccessful
        {
            get
            {
                return this.Errors.All(err => err.Diagnostic.Severity != DiagnosticSeverity.Error);
            }
        }

        /// <summary>
        /// Gets the list of errors triggered during the translation process.
        /// </summary>
        public List<Error> Errors { get; } 

        public TranslationProcessResult(Silvernode silvernode, List<Error> errors)
        {
            this.Silvernode = silvernode;
            this.Errors = errors;
        }

        public static TranslationProcessResult Error(SyntaxNode syntaxNode, SoothsharpDiagnostic diagnostic, params object[] arguments)
        {
            return new TranslationProcessResult(null, new List<Error> { new Error(diagnostic, syntaxNode, arguments) });
        }
    }
}
