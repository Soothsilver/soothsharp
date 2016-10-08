using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharpsilver.Translation.BackendInterface;
using Xunit;

namespace Sharpsilver.Translation.Tests
{
    public class SystemTest
    {
        [Theory()]
        [MemberData(nameof(GetTestFiles))]
        public void Sys(string test)
        {
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
            foreach (string line in lines)
            {
                lineNumber++;
                string trimmed = line.Trim().ToLower();
                if (trimmed.StartsWith("//"))
                {
                    string[] words = trimmed.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Length < 2) continue;
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
                BackendInterface.IBackend backend = new CarbonBackend();
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

        public static IEnumerable<object[]> GetTestFiles()
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
