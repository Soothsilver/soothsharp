using System.Collections.Generic;

namespace Soothsharp.Translation.Backends
{
    /// <summary>
    /// Represents the result of a backend verification.
    /// </summary>
    public class VerificationResult
    {
        /// <summary>
        /// Gets a value that indicates whether the program was successfully verified with no errors.
        /// </summary>
        public bool VerificationSuccessful => this.Errors.Count == 0;
        /// <summary>
        /// Gets or sets the list of errors generated during verification.
        /// </summary>
        public List<Error> Errors { get; set; } = new List<Error>();
        /// <summary>
        /// Gets or sets the output produced by the connected backend executable.
        /// </summary>
        public string OriginalOutput = "";

        /// <summary>
        /// Creates a new <see cref="VerificationResult"/> as a failed verification with a single error. 
        /// </summary>
        /// <param name="error">The single error in the verification result.</param>
        /// <returns></returns>
        public static VerificationResult Error(Error error)
        {
            VerificationResult r = new VerificationResult();
            r.Errors.Add(error);
            return r;
        }
    }
}
