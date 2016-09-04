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
        public void Sys(string filename)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, filename);
            string csharpCode = System.IO.File.ReadAllText(fullFilename);

            var translation = TranslationProcess.Create(
                new List<string> { fullFilename },
                new List<string>(),
                new List<string>(),
                new TranslationConfiguration()
                );

            var result = translation.Execute();

            var lines = csharpCode.Split('\n');
            var expectedErrorcodes = new List<string>();
            bool syntaxOnly = false;
            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("//"))
                {
                    string ubertrimmed = line.Substring(2).Trim().ToLower();
                    if (ubertrimmed.StartsWith("expect"))
                    {
                        string megatrimmed = ubertrimmed.Substring(7).Trim();
                        expectedErrorcodes.Add(megatrimmed);
                    }
                    else if (ubertrimmed.StartsWith("syntax"))
                    {
                        syntaxOnly = true;
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
            Assert.True(expectedErrorcodes.Count == errors.Count, "Expected error count: " + expectedErrorcodes.Count + "\nActual errors: " + errors.Count + "\n" + string.Join("\n", result.Errors));
            foreach (var error in errors)
            {
                if (expectedErrorcodes.Contains(error.Diagnostic.ErrorCode.ToLower()))
                {
                    expectedErrorcodes.Remove(error.Diagnostic.ErrorCode);
                }
                else
                {
                    Assert.True(false, string.Join("\n", errors));
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
