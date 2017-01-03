using Soothsharp.Translation.Trees.Silver;

namespace Soothsharp.Translation.BackendInterface
{
    /// <summary>
    /// This intermediate class passes a Silver syntax tree to a backend verifier and returns the result.
    /// </summary>
    class Verifier
    {
        public IBackend Backend;

        /// <summary>
        /// Initializes a new <see cref="Verifier"/> that will use the specified backend. 
        /// </summary>
        /// <param name="backend">The backend to use (Silicon or Carbon).</param>
        public Verifier(IBackend backend)
        {
            Backend = backend;
        }

        /// <summary>
        /// Passes a Silver syntax tree to a backend verifier and returns the result.
        /// </summary>
        /// <param name="program">The root of the Silver syntax tree</param>
        public VerificationResult Verify(Silvernode program)
        {
            var result = Backend.Verify(program);
            return result;
        }
    }
}
