using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Sil.Translation
{
    public class TranslationResult
    {
        public SilverSourceCode SilverSourceCode;
        public bool WasTranslationSuccessful;
        public List<Error> Errors = new List<Error>();
    }
}
