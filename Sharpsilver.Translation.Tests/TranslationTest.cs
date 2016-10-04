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
        [MemberData(nameof(GetTestFiles))]
        public void TransOnly(string test)
        { 
            
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, test);
            string csharpCode = System.IO.File.ReadAllText(fullFilename);

            var translation = TranslationProcess.Create(new List<string>() { fullFilename }, new List<string>(), new List<string>(), new TranslationConfiguration());
            var result = translation.Execute();
            
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
