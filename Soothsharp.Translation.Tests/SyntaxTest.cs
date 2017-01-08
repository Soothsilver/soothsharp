using Soothsharp.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Soothsharp.Translation.Tests
{
    public class SyntaxTest
    {
        [Theory()]
        [MemberData(nameof(SyntaxTest.GetTestFiles))]
        public void TransOnly(string test)
        { 
            
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, test);

            var translation = TranslationProcess.Create(new List<string>() { fullFilename }, new List<string>(), new List<string>(), new TranslationConfiguration());
            var result = translation.Execute();
            
            Assert.True(result.WasTranslationSuccessful, string.Join("\n", result.Errors));
        }

        private static IEnumerable<object[]> GetTestFiles()
        {
            foreach (
                var filename in
                    System.IO.Directory.EnumerateFiles("Syntax", "*.cs", System.IO.SearchOption.AllDirectories))
            {
                yield return new object[] {  filename };
            }
        }
    }
}
