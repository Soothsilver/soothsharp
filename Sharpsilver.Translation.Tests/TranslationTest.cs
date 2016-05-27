using Sharpsilver.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sharpsilver.Translation.Tests
{
    public class TranslationTest
    {
        [Theory()]
        [MemberData("GetTestFiles")]
        public void TranslationToSilverOk(string filename)
        { 
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, filename);
            string csharpCode = System.IO.File.ReadAllText(fullFilename);

            var translation = new TranslationProcess();
            var result = translation.TranslateCode(csharpCode, false);
            
                Assert.True(result.WasTranslationSuccessful, string.Join("\n", result.Errors));
        }

        public static IEnumerable<object[]> GetTestFiles()
        {
            foreach (
                var filename in
                    System.IO.Directory.EnumerateFiles("Files", "*.cs", System.IO.SearchOption.AllDirectories))
            {
                yield return new object[] {  filename };
            }
        }
    }
}
