using Sharpsilver.Translation.Trees.Silver;

namespace Sharpsilver.Translation.BackendInterface
{
    /// <summary>
    /// A backend class executes its corresponding executable (carbon or silicon), passing its the transcompiler's output. The backend class
    /// is also responsible for producing <see cref="Error"/>s by parsing the executable's output. 
    /// </summary>
    public interface IBackend
    {
        /// <summary>
        /// Performs formal verification on Silver code represented by its root silvernode and returns any errors.
        /// </summary>
        /// <param name="silvernode">Root of the Silver syntax tree.</param>
        VerificationResult Verify(Silvernode silvernode);
    }
}
