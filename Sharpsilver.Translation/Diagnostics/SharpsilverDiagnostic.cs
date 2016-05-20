using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    /// <summary>
    /// Represents a type of an error that can occur during translation or verification.
    /// </summary>
    public class SharpsilverDiagnostic
    {
        private SharpsilverDiagnostic(string errorCode, string caption, string details, DiagnosticSeverity severity)
        {
            this.ErrorCode = errorCode;
            this.Caption = caption;
            this.Details = details;
            this.Severity = severity;
        }

        public string ErrorCode { get; }
        public string Caption { get; }
        public string Details { get; }
        public DiagnosticSeverity Severity { get; }


        public static SharpsilverDiagnostic Create(
            string errorCode, 
            string caption, 
            string details, DiagnosticSeverity severity)
        {
            SharpsilverDiagnostic sd = new SharpsilverDiagnostic(errorCode, caption, details, severity);
            return sd;
        }
        
    }
}
