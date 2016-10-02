using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharpsilver.Translation
{
    public class Identifier
    {
        public IdentifierTranslator identifierTranslator { get; set; }
        public TaggedSymbol Symbol { get; set; }


        public Identifier(TaggedSymbol symbol, IdentifierTranslator identifierTranslator)
        {
            this.Symbol = symbol;
            this.identifierTranslator = identifierTranslator;
        }
        public Identifier(string silvername)
        {
            this.Silvername = silvername;
        }

        public string Silvername { get; set; } = SILVERNAME_NOT_YET_ASSIGNED;

        public const string SILVERNAME_NOT_YET_ASSIGNED = "!silvername-not-yet-assigned!";

        public override string ToString()
        {
            return Silvername;
        }
    }
}
