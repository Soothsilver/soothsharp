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
        [InlineData("Files\\Simple.cs")]
        public void TranslationToSilverOk(string filename)
        { 
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string fullFilename = System.IO.Path.Combine(dir, filename);
            string csharpCode = System.IO.File.ReadAllText(fullFilename);

            var translation = new TranslationProcess();
            var result = translation.TranslateCode(csharpCode, false);
            
                Assert.True(result.WasTranslationSuccessful, String.Join("\n", result.Errors));
        }
    }
}
