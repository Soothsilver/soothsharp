using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace Sharpsilver.Translation
{
    static class VerificationSettings
    {
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
    enum VerificationSetting
    {
        Verify,
        DoNotVerify,
        Contradiction
    }
}
