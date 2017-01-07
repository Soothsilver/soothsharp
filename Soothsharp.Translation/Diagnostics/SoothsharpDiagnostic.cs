namespace Soothsharp.Translation
{
    /// <summary>
    /// Represents a type of an error that can occur during translation or verification. 
    /// An instance of this class represents a generic error message, not associated with a particular syntax node or line of code.
    /// That is for the class <see cref="Error"/>. 
    /// </summary>
    public class SoothsharpDiagnostic
    {
        private SoothsharpDiagnostic(string errorCode, string caption, string details, DiagnosticSeverity severity)
        {
            this.ErrorCode = errorCode;
            this.Caption = caption;
            this.Details = details;
            this.Severity = severity;
        }

        /// <summary>
        /// Gets the error code, such as SSIL101.
        /// </summary>
        public string ErrorCode { get; }
        /// <summary>
        /// Gets the text to be printed to console or in the Error List in Visual Studio. Its placeholders ({0}, {1}, ...) must be replaced.
        /// </summary>
        public string Caption { get; }
        /// <summary>
        /// Gets additional text that might be printed to console and that is visible upon expansion in the Error List in Visual Studio. 
        /// It may not contain placeholders. Details might be null.
        /// </summary>
        public string Details { get; }
        /// <summary>
        /// Gets the severity of the diagnostic, i.e. whether it's an error or a warning.
        /// </summary>
        public DiagnosticSeverity Severity { get; }

        /// <summary>
        /// Creates a new <see cref="SoothsharpDiagnostic"/> associated with an error code. 
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="caption">The main error text.</param>
        /// <param name="details">Optional details, or null.</param>
        /// <param name="severity">Whether it's an error or a warning.</param>
        public static SoothsharpDiagnostic Create(
            string errorCode, 
            string caption, 
            string details, 
            DiagnosticSeverity severity)
        {
            SoothsharpDiagnostic sd = new SoothsharpDiagnostic(errorCode, caption, details, severity);
            return sd;
        }
        
    }
}
