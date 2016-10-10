namespace Sharpsilver.Translation
{
    /// <summary>
    /// Contains settings from the front-end that affect the <see cref="TranslationProcess"/> globally. 
    /// </summary>
    public class TranslationConfiguration
    {
        /// <summary>
        /// If true, additional information is printed to the console during verification.
        /// </summary>
        public bool Verbose { get; set; } = false;
        /// <summary>
        /// If true, classes and methods not annotated with a verification settings attributes, will be considered marked [Verified]. If false, they will be considered marked [Unverified]. By default, TRUE.
        /// </summary>
        public bool VerifyUnmarkedItems { get; set; } = true;
    }
}
