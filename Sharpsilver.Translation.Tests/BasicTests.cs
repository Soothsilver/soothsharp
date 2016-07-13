using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sharpsilver.Translation.Tests
{
    public class BasicTests
    {
      //  [Fact]
        public void ScalaPaperExample()
        {
            Utilities.AssertTranslationCorrect(@"Files\Basic\ScalaPaperExample.cs");
        }
        //[Fact]
        public void ScalaPaperExampleSilicon()
        {
            Utilities.AssertVerificationSuccessful(@"Files\Basic\ScalaPaperExample.cs");
        }
    }
}
