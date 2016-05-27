using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Sharpsilver.Translation.BackendInterface;

namespace Sharpsilver.Translation.Tests
{
    class Utilities
    {
        public static TranslationResult Translate(string filename)
        {
            TranslationProcess process = new TranslationProcess();
            var result = process.TranslateCode(filename, false);
            return result;
        }
        public static string ReadAllText(string filename)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, filename);
            string csharpCode = System.IO.File.ReadAllText(fullFilename);
            return csharpCode;
        }

        public static void AssertTranslationCorrect(string filesBasicScalapaperexampleCs)
        {
            var result = Translate(filesBasicScalapaperexampleCs);
            Assert.True(result.WasTranslationSuccessful, string.Join("\n", result.Errors));
        }

        public static void AssertVerificationSuccessful(string filesBasicScalapaperexampleCs)
        {
            var translationResult = Translate(filesBasicScalapaperexampleCs);
            Assert.True(translationResult.WasTranslationSuccessful, string.Join("\n", translationResult.Errors));

            // Translation ok, let's verify.
            IBackend backend = new SiliconBackend();
            var verifyResult = backend.Verify(translationResult.Silvernode);
            Assert.True(verifyResult.VerificationSuccessful, string.Join("\n", verifyResult.Errors));
        }
    }
}
