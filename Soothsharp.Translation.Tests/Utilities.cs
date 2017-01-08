using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Soothsharp.Translation.BackendInterface;

namespace Soothsharp.Translation.Tests
{
    class Utilities
    {
        public static TranslationProcessResult Translate(string filename)
        {
            TranslationProcess process = TranslationProcess.Create(new List<string> { filename }, new List<string>(), new List<string>(), new TranslationConfiguration());
            var result = process.Execute();
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
            var result = Utilities.Translate(filesBasicScalapaperexampleCs);
            Assert.True(result.WasTranslationSuccessful, string.Join("\n", result.Errors));
        }

        public static void AssertVerificationSuccessful(string filesBasicScalapaperexampleCs)
        {
            var translationResult = Utilities.Translate(filesBasicScalapaperexampleCs);
            Assert.True(translationResult.WasTranslationSuccessful, string.Join("\n", translationResult.Errors));

            // Translation ok, let's verify.
            IBackend backend = new SiliconBackend();
            var verifyResult = backend.Verify(translationResult.Silvernode);
            Assert.True(verifyResult.VerificationSuccessful, string.Join("\n", verifyResult.Errors));
        }
    }
}
