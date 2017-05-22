using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soothsharp.Translation.Backends;
using Xunit;

namespace Soothsharp.Translation.Tests
{
    /// <summary>
    /// The systemwide test runs translation, then verification (using Silicon) and expects either 
    /// the verification to be successful or the translation or verification to fail with reasons
    /// specified in comments in the test files.
    /// </summary>
    public class SystemwideTest
    {
        [Theory()]
        [MemberData(nameof(SystemwideTest.GetTestFiles))]
        public void Sys(string test)
        {
            if (test.Contains("Simple"))
            {

            }
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, test);
            string csharpCode = System.IO.File.ReadAllText(fullFilename);

            var translation = TranslationProcess.Create(
                new List<string> { fullFilename },
                new List<string>(),
                new List<string>(),
                new TranslationConfiguration()
                );

            var result = translation.Execute();

            var lines = csharpCode.Split('\n');
            var expectedErrorcodes = new List<Tuple<string,int>>();
            bool syntaxOnly = false;
            int lineNumber = 0;
            IBackend backend = new CarbonNailgunBackend();
            foreach (string line in lines)
            {
                lineNumber++;
                string trimmed = line.Trim().ToLower();
                if (trimmed.StartsWith("//"))
                {
                    string[] words = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 2) continue;
                    if (words[1] == "use")
                    {
                        if (words.Length >= 3)
                        {
                            if (words[2] == "silicon")
                            {
                                backend = new SiliconNailgunBackend();
                            }
                            else if (words[2] == "carbon")
                            {
                                backend = new CarbonNailgunBackend();
                            }
                        }
                    }
                    if (words[1] == "syntax") syntaxOnly = true;
                    if (words[1] == "expect" && words.Length >= 3)
                    {
                        string code = words[2];
                        int errorNumber = -1;
                        if (words.Length == 5)
                        {
                            if (words[3] == "at")
                            {
                                if (words[4] == "next")
                                {
                                    errorNumber = lineNumber + 1;
                                }
                            }
                        }
                        expectedErrorcodes.Add(new Tuple<string, int>(code, errorNumber));
                    }
                }
            }

            var errors = result.Errors;
            if (!syntaxOnly && result.Errors.Count == 0)
            {
                var verificationResult = backend.Verify(result.Silvernode);
                errors = verificationResult.Errors;
            }
            Assert.True(expectedErrorcodes.Count == errors.Count, "Expected error count: " + expectedErrorcodes.Count + "\nActual errors: " + errors.Count + "\n" + string.Join("\n", errors));
            foreach (var error in errors)
            {
                var appropriateTuple =
                    expectedErrorcodes.Find(tuple => tuple.Item1 == error.Diagnostic.ErrorCode.ToLower() &&
                   (tuple.Item2 == -1 || tuple.Item2 == error.CsharpLine));
                if (appropriateTuple != null)
                {
                    expectedErrorcodes.Remove(appropriateTuple);
                }
                else
                {
                    Assert.True(false, "Actual:\n" + string.Join("\n", errors) + "Expected:\n" + string.Join("\n", expectedErrorcodes));
                }
            }

            Assert.True(result.WasTranslationSuccessful, string.Join("\n", result.Errors));
        }

        private static IEnumerable<object[]> GetTestFiles()
        {
            foreach (
                var filename in
                    System.IO.Directory.EnumerateFiles("Systemwide", "*.cs", System.IO.SearchOption.AllDirectories))
            {
                yield return new object[] { filename };
            }
        }
    }
}
