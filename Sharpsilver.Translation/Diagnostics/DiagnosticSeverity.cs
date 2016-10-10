namespace Sharpsilver.Translation
{
    /// <summary>
    /// Whether a diagnostic is an error or a warning.
    /// </summary>
    public enum DiagnosticSeverity
    {
        /// <summary>
        /// The translation should work as normal, except with some minor issues.
        /// </summary>
        Warning,
        /// <summary>
        /// The translation won't work but we can run the rest of it anyway.
        /// </summary>
        Error
    }
}
