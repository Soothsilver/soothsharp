using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace Soothsharp.Translation
{
    /// <summary>
    /// Contains a helper class for determining whether a piece of code should be translated.
    /// </summary>
    static class VerificationSettings
    {
        /// <summary>
        /// Determines whether a class or a method should be translated to Viper.
        /// </summary>
        /// <param name="attributes">The attributes of that class or method.</param>
        /// <param name="verifyUnmarkedItems">Whether classes and methods with no attributes should be translated.</param>
        public static VerificationSetting ShouldVerify(ImmutableArray<AttributeData> attributes, bool verifyUnmarkedItems)
        {
            bool markedUnverified = false;
            bool markedVerified = false;
            foreach(AttributeData data in attributes)
            {
                string name = data.AttributeClass.GetQualifiedName();
                if (name == ContractsTranslator.UnverifiedAttribute) markedUnverified = true;
                if (name == ContractsTranslator.VerifiedAttribute) markedVerified = true;
            }
            if (markedVerified && markedUnverified)
                return VerificationSetting.Contradiction;
            if (markedVerified)
                return VerificationSetting.Verify;
            if (markedUnverified)
                return VerificationSetting.DoNotVerify;
            if (verifyUnmarkedItems)
                return VerificationSetting.Verify;
            else
                return VerificationSetting.DoNotVerify;

        }
    }
    /// <summary>
    /// Indicates whether a class or method should be translated.
    /// </summary>
    enum VerificationSetting
    {
        /// <summary>
        /// This class or method should be translated.
        /// </summary>
        Verify,
        /// <summary>
        /// This class or method should be ignored.
        /// </summary>
        DoNotVerify,
        /// <summary>
        /// This class or method was annotated both [Verified] and [Unverified]. An error should trigger.
        /// </summary>
        Contradiction
    }
}
