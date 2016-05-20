using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation
{
    /// <summary>
    /// Represents a squiggly-type error that occured as a result of translation or verification. It is not necessarily an error, it might just as well be a warning.
    /// </summary>
    public class Error
    {
        public SharpsilverDiagnostic Diagnostic;
        public object[] DiagnosticArguments;
        public SyntaxNode Node;
        public int CsharpColumn
        {
            get
            {
                if (Node == null) return 0;
                return Node.GetLocation().GetLineSpan().StartLinePosition.Character + 1;
            }
        }
        public int CsharpLine
        {
            get
            {
                if (Node == null) return 0;
                return Node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
            }
        }

        public Error(SharpsilverDiagnostic diagnostic, SyntaxNode node, params object[] diagnosticArguments)
        {
            this.Diagnostic = diagnostic;
            this.DiagnosticArguments = diagnosticArguments;
            this.Node = node;
        }

        public override string ToString()
        {
            return CsharpLine + ":" + CsharpColumn + " " + Diagnostic.ErrorCode + ": " + String.Format(Diagnostic.Caption, DiagnosticArguments);
        }
    }
}
