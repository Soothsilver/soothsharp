using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents a squiggly-type error that occured as a result of translation or verification. 
    /// It is not necessarily an error, it might just as well be a warning.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Information about this kind of error.
        /// </summary>
        public SharpsilverDiagnostic Diagnostic;
        /// <summary>
        /// Arguments to be passed to the <see cref="SharpsilverDiagnostic"/> message.
        /// </summary>
        public object[] DiagnosticArguments;
        /// <summary>
        /// The syntax node with which this error is associated, or null if there is no such node.
        /// </summary>
        public SyntaxNode Node;
        /// <summary>
        /// Gets the 1-indexed column of the associated syntax node, or else 0.
        /// </summary>
        public int CsharpColumn
        {
            get
            {
                if (this.Node == null) return 0;
                return this.Node.GetLocation().GetLineSpan().StartLinePosition.Character + 1;
            }
        }
        /// <summary>
        /// Gets the 1-indexed line of the associated syntax node, or else 0.
        /// </summary>
        public int CsharpLine
        {
            get
            {
                if (this.Node == null) return 0;
                return this.Node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
            }
        }


        public Error(SharpsilverDiagnostic diagnostic, SyntaxNode node, params object[] diagnosticArguments)
        {
            this.Diagnostic = diagnostic;
            this.DiagnosticArguments = diagnosticArguments;
            this.Node = node;
        }

        /// <summary>
        /// This ToString() method is used when printing the error to the commandline console.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.CsharpLine + ":" + 
                this.CsharpColumn + " " + 
                this.Diagnostic.ErrorCode + ": " + 
                string.Format(this.Diagnostic.Caption, this.DiagnosticArguments);
        }
    }
}
