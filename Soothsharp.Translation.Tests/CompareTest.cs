using Soothsharp.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Soothsharp.Translation.Tests
{
    public class CompareTest
    {
        [Theory()]
        [MemberData(nameof(CompareTest.GetTestFiles))]
        public void Compare(string test)
        { 
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, test);
            string fullSilvername = System.IO.Path.Combine(dir, 
                System.IO.Path.GetDirectoryName(test),
                System.IO.Path.GetFileNameWithoutExtension(test)
                + ".vpr");

            var translation = TranslationProcess.Create(new List<string>() { fullFilename }, new List<string>(), new List<string>(), new TranslationConfiguration());
            var result = translation.Execute();
            Assert.True(result.WasTranslationSuccessful, string.Join("\n", result.Errors));

            string silverResult = Reduce(result.Silvernode.ToString());
            string silverExpected = Reduce(System.IO.File.ReadAllText(fullSilvername));

            Assert.Equal(silverExpected, silverResult);
        }

        private string Reduce(string aString)
        {
            return aString.Trim().Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
        }

        private static IEnumerable<object[]> GetTestFiles()
        {
            foreach (
                var filename in
                    System.IO.Directory.EnumerateFiles("Compare", "*.cs", System.IO.SearchOption.AllDirectories))
            {
                yield return new object[] {  filename };
            }
        }
    }
}
