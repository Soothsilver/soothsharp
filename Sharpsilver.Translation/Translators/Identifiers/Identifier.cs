using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    public abstract class Identifier
    {
        public IdentifierTranslator identifierTranslator { get; set; }
        public TaggedSymbol Symbol { get; set; }


        public string Silvername { get; set; } = "silvername-not-yet-assigned";
    }
}
